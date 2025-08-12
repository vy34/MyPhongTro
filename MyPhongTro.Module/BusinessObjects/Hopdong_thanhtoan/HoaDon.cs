using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using MyPhongTro.Module.BusinessObjects.Chutro;
using MyPhongTro.Module.BusinessObjects.Quanlyphongtro;
using System.ComponentModel;
using System.Drawing;


namespace MyPhongTro.Module.BusinessObjects.Hopdong_thanhtoan
{
    [DefaultClassOptions]
    [ImageName("hoadon")]
    [System.ComponentModel.DisplayName("Hóa đơn")]
    [NavigationItem("Hợp đồng và hoá đơn")]
    [DefaultProperty("SoHoaDon")]
    [DefaultListViewOptions(MasterDetailMode.ListViewOnly, true, NewItemRowPosition.Top)]
    //[Persistent("DatabaseTableName")]
    // Specify more UI options using a declarative approach (https://docs.devexpress.com/eXpressAppFramework/112701/business-model-design-orm/data-annotations-in-data-model).
    public class HoaDon(Session session) : BaseObject(session)
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://docs.devexpress.com/eXpressAppFramework/113146/business-model-design-orm/business-model-design-with-xpo/base-persistent-classes).
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://docs.devexpress.com/eXpressAppFramework/112834/getting-started/in-depth-tutorial-winforms-webforms/business-model-design/initialize-a-property-after-creating-an-object-xpo?v=22.1).
            if (Session.IsNewObject(this))
            {
                ChuTro chutro = Session.FindObject<ChuTro>(CriteriaOperator.Parse("Oid = ?", SecuritySystem.CurrentUserId));
                if (chutro != null)
                {
                    Chutro = chutro; // Tự động gán chủ trọ là người dùng hiện tại
                }

                string sql = "select max(So) as so from HoaDon where Chutro = '" + SecuritySystem.CurrentUserId + "'";
                var ret = Session.ExecuteScalar(sql);
                int so = 1;
                if (ret != null) so = tmLib.ViCom.CInt(ret) + 1; // Lấy số hợp đồng lớn nhất của chủ trọ hiện tại
                So = so; // Số hợp đồng mặc định là 1

                Ngay = TCom.GetServerDateOnly(); // Ngày mặc định là ngày hiện tại
            }
        }


        private ChuTro _Chutro;
        [Association]
        [VisibleInListView(false), VisibleInLookupListView(false), VisibleInDetailView(false), VisibleInDashboards(false)]
        public ChuTro Chutro
        {
            get { return _Chutro; }
            set { SetPropertyValue<ChuTro>(nameof(Chutro), ref _Chutro, value); }
        }


        [NonPersistent] // Thuộc tính không được lưu vào cơ sở dữ liệu
        [VisibleInListView(false), VisibleInDetailView(false)]
        private bool _IsHoadonTiencoc;
        public bool IsHoadonTiencoc
        {
            get { return _IsHoadonTiencoc; }
            set { SetPropertyValue<bool>(nameof(IsHoadonTiencoc), ref _IsHoadonTiencoc, value); }
        }



        private HopDong _Hopdong;
        [Association]
        [XafDisplayName("Hợp đồng")]
        [RuleRequiredField("Hợp đồng không được để trống", DefaultContexts.Save)]
        public HopDong Hopdong
        {
            get { return _Hopdong; }
            set
            {
                bool isModified = SetPropertyValue<HopDong>(nameof(Hopdong), ref _Hopdong, value);
                if (isModified && !IsDeleted && !IsLoading && value != null && !IsHoadonTiencoc)
                {
                    if (HoaDonCTs.Count > 0)// Nếu đã có chi tiết hóa đơn thì xóa hết
                    {
                        while (HoaDonCTs.Count > 0)
                        {
                            HoaDonCTs.Remove(HoaDonCTs[0]);
                        }
                    }
                    if (value != null) // Nếu hợp đồng không null thì tạo chi tiết hóa đơn mới
                    {
                        var hoadonTruoc = value.HoaDons // Lấy hóa đơn trước đó của hợp đồng này
                                            .Where(hd => hd.Oid != this.Oid) // Bỏ qua hóa đơn hiện tại nếu đã có Oid
                                            .OrderByDescending(hd => hd.Ngay) // Sắp xếp theo ngày giảm dần
                                            .FirstOrDefault(); // Lấy hóa đơn mới nhất trước ngày hiện tại

                        foreach (var hopDongCT in value.HopDongCTs)
                        {
                            HoaDonCT hdct = new(Session)
                            {
                                Hoadon = this,
                                Khoanthu = hopDongCT.Khoanthu,
                                DonGia = hopDongCT.Dongia,
                            };

                            if (hdct.Khoanthu != null && (hdct.Khoanthu.TenKhoanThu.Contains("điện", StringComparison.OrdinalIgnoreCase) || hdct.Khoanthu.TenKhoanThu.Contains("nước", StringComparison.OrdinalIgnoreCase)))
                            {
                                if (hoadonTruoc != null)
                                { 
                                    var hoaDonCTTruoc = hoadonTruoc.HoaDonCTs.FirstOrDefault(ct => ct.Khoanthu?.Oid == hdct.Khoanthu?.Oid);// Tìm chi tiết hóa đơn trước đó cùng loại khoản thu

                                    if (hoaDonCTTruoc != null)
                                    {
                                        hdct.Chisodau = hoaDonCTTruoc.Chisocuoi;
                                    }
                                    else
                                    {
                                        hdct.Chisodau = hopDongCT.Chisodau; // Nếu không tìm thấy, lấy chỉ số đầu từ hợp đồng chi tiết
                                    }
                                }
                                else
                                {
                                    hdct.Chisodau = hopDongCT.Chisodau;// Đây là hóa đơn đầu tiên, lấy chỉ số đầu từ hợp đồng chi tiết
                                }
                            }
                            else
                            {
                                hdct.Chisodau = hopDongCT.Chisodau; // Đối với các khoản thu khác (không phải điện, nước), chỉ số đầu mặc định là 0 hoặc giá trị của hopDongCT
                            }
                            this.HoaDonCTs.Add(hdct);
                        }
                    }
                }
            }
        }

        // Replace this property:
        [VisibleInListView(false), VisibleInDetailView(false)]
        public string SoHoaDon
        {
            get
            {
                var so = So.ToString();
                if (Hopdong?.Phong != null)
                    so += "/" + Hopdong.Phong.Sophong;
                return so;
            }
        }

        private int _So;
        [XafDisplayName("Số hóa đơn"), ModelDefault("AllowEdit", "false")]
        public int So
        {
            get { return _So; }
            set { SetPropertyValue<int>(nameof(So), ref _So, value); }
        }

        private DateOnly _Ngay;
        [XafDisplayName("Ngày")]
        [ModelDefault("EditMask", "dd/MM/yyyy")]
        [ModelDefault("DisplayFormat", "{0:dd/MM/yyyy}")]
        public DateOnly Ngay
        {
            get { return _Ngay; }
            set { SetPropertyValue<DateOnly>(nameof(Ngay), ref _Ngay, value); }
        }

        [XafDisplayName("Tổng tiền")]
        [ModelDefault("DisplayFormat", "{0:### ### ###}")]     //tự động
        [ModelDefault("EditMask", "### ### ###")]
        public decimal TongTien
        {
            get
            {
                decimal tien = HoaDonCTs.Sum(x => x.ThanhTien);
                return tien;

            }
        }


        [XafDisplayName("Đã thu")]
        [ModelDefault("DisplayFormat", "{0:### ### ###}")]     //tự động
        [ModelDefault("EditMask", "### ### ###")]
        public decimal Dathu
        {
            get
            {
                decimal tien = PhieuThus.Sum(x => x.Sotien);
                return tien;
            }

        }

        [XafDisplayName("Tiền nợ")]
        [ModelDefault("DisplayFormat", "{0:### ### ###}")]     //tự động
        [ModelDefault("EditMask", "### ### ###")]
        public decimal Conno
        {
            get
            {
                return TongTien - Dathu;
            }
        }

        private string _Noidung;
        [XafDisplayName("Nội dung")]
        [Size(255)]
        public string Noidung
        {
            get { return _Noidung; }
            set { SetPropertyValue<string>(nameof(Noidung), ref _Noidung, value); }
        }





        private byte[] _QRCodeImage;
        [ImageEditor( 
           DetailViewImageEditorFixedHeight = 450,
           DetailViewImageEditorFixedWidth = 400
        )]
        [XafDisplayName("Mã QR")]
        [VisibleInDetailView(true), VisibleInListView(false)]
        public byte[] QRCodeImage
        {
            get { return _QRCodeImage; }
            set { SetPropertyValue<byte[]>(nameof(QRCodeImage), ref _QRCodeImage, value); }
        }





        [DevExpress.Xpo.Aggregated, Association]
        public XPCollection<HoaDonCT> HoaDonCTs
        {
            get { return GetCollection<HoaDonCT>(nameof(HoaDonCTs)); }

        }


        [DevExpress.Xpo.Aggregated, Association]                
        public XPCollection<PhieuThu> PhieuThus
        {
            get { return GetCollection<PhieuThu>(nameof(PhieuThus)); }
        }



        [DevExpress.Xpo.Aggregated, Association]
        public XPCollection<Anh> Anhs
        {
            get { return GetCollection<Anh>(nameof(Anhs)); }
        }

    }
}