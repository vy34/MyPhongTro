using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using MyPhongTro.Module.BusinessObjects.Chutro;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace MyPhongTro.Module.BusinessObjects.Hopdong_thanhtoan
{
    [DefaultClassOptions]
    [ImageName("hoadon")]
    [System.ComponentModel.DisplayName("Hóa đơn")]
    [NavigationItem("Hợp đồng thanh toán")]
    [DefaultProperty("So")]
    [DefaultListViewOptions(MasterDetailMode.ListViewOnly, true, NewItemRowPosition.Top)]
    //[Persistent("DatabaseTableName")]
    // Specify more UI options using a declarative approach (https://docs.devexpress.com/eXpressAppFramework/112701/business-model-design-orm/data-annotations-in-data-model).
    public class HoaDon(Session session) : BaseObject(session)
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://docs.devexpress.com/eXpressAppFramework/113146/business-model-design-orm/business-model-design-with-xpo/base-persistent-classes).
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://docs.devexpress.com/eXpressAppFramework/112834/getting-started/in-depth-tutorial-winforms-webforms/business-model-design/initialize-a-property-after-creating-an-object-xpo?v=22.1).
        if( Session.IsNewObject(this))
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


        private HopDong _Hopdong;
        [Association]
        [XafDisplayName("Hợp đồng")]
        [RuleRequiredField("Hợp đồng không được để trống", DefaultContexts.Save)]
        public HopDong Hopdong
        {
            get { return _Hopdong; }
            set { SetPropertyValue<HopDong>(nameof(Hopdong), ref _Hopdong, value); }
        }



        private int _So;
        [XafDisplayName("Số hóa đơn"),ModelDefault("AllowEdit","false")]
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
        [ModelDefault("DisplayFormat", "{0:### ###}")]     //tự động
        [ModelDefault("EditMask", "### ###")]
        public decimal Dathu
        {
            get
            {
                decimal tien = PhieuThus.Sum(x => x.Sotien);
                return tien;
            }
        }

        
        [XafDisplayName("Tiền nợ")]
        [ModelDefault("DisplayFormat", "{0:### ###}")]     //tự động
        [ModelDefault("EditMask", "### ###")]
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