using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.Security;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.BaseImpl.PermissionPolicy;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using MyPhongTro.Module.BusinessObjects.Cauhinhhethong;
using MyPhongTro.Module.BusinessObjects.Hopdong_thanhtoan;
using MyPhongTro.Module.BusinessObjects.Quanlykhanhthue;
using MyPhongTro.Module.BusinessObjects.Quanlyphongtro;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace MyPhongTro.Module.BusinessObjects.Chutro
{
    [DefaultClassOptions]
    [ImageName("chutro")]
    [System.ComponentModel.DisplayName("Chủ trọ")]
    [NavigationItem("Chủ trọ")]
    //[DefaultProperty("Tenchutro")]
    [DefaultListViewOptions(MasterDetailMode.ListViewOnly, true, NewItemRowPosition.Top)]
    //[Persistent("DatabaseTableName")]
    // Specify more UI options using a declarative approach (https://docs.devexpress.com/eXpressAppFramework/112701/business-model-design-orm/data-annotations-in-data-model).
    public class ChuTro(Session session) : ApplicationUser(session) 
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://docs.devexpress.com/eXpressAppFramework/113146/business-model-design-orm/business-model-design-with-xpo/base-persistent-classes).
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://docs.devexpress.com/eXpressAppFramework/112834/getting-started/in-depth-tutorial-winforms-webforms/business-model-design/initialize-a-property-after-creating-an-object-xpo?v=22.1).
            if (Session.IsNewObject(this))
            {
                NgayDangky = TCom.GetServerDateOnly();
                // Tự động tạo số đăng ký dựa trên ngày đăng ký mới nhất
                int maxSo = Session.Query<ChuTro>().Max(x => (int?)x.Sodangky) ?? 0;
                Sodangky = maxSo + 1; // Tăng số đăng ký lên 1
                Phiduytri phiduytri = Session.FindObject<Phiduytri>(CriteriaOperator.Parse("Noidung = ? && Chutro.Oid = ? ","Dùng thử", this));
                if (phiduytri == null)
                {
                    phiduytri = new Phiduytri(Session)
                    {
                        Chutro = this,
                        Ngaynop = TCom.GetServerDateOnly(),
                        Sotien = 0, // Số tiền dùng thử là 0
                        Noidung = "Dùng thử",

                    };
                    phiduytri.Hangsudung = phiduytri.Ngaynop.AddMonths(3); // Sử dụng 1 tháng dùng thử
                    phiduytri.Save();
                }
            }
        }

        protected override void OnSaving()
        {
            Captaikhoan();
            base.OnSaving();
            
        }
        //protected override void OnDeleting()
        //{
        //    base.OnDeleting();
        //    int so = Session.CollectReferencingObjects(this).Count;
        //    if (so > 0)
        //    {
        //        throw new UserFriendlyException("Không thể xóa chủ trọ này vì có " + so + " đối tượng liên quan. Vui lòng xóa các đối tượng liên quan trước.");
        //    }
        //}


        private string _Tenchutro;
        [XafDisplayName("Tên chủ trọ"), Size(255)]
        [RuleRequiredField("Tenchutro", DefaultContexts.Save, "Bạn phải nhập tên chủ trọ")]
        public string Tenchutro
        {
            get { return _Tenchutro; }
            set { SetPropertyValue<string>(nameof(Tenchutro), ref _Tenchutro, value); }
        }

        private string _Dienthoai;
        [XafDisplayName("Điện thoại")]
        [RuleRequiredField("Dienthoai", DefaultContexts.Save, "Phải có điện thoại chủ trọ là tài khoản")]
        public string Dienthoai
        {
            get { return _Dienthoai; }
            set
            {
                if (SetPropertyValue(nameof(Dienthoai), ref _Dienthoai, value))
                {
                    if (Session.IsNewObject(this) && !string.IsNullOrEmpty(value)) // Chỉ tạo tài khoản người dùng nếu là chủ trọ mới và số điện thoại không rỗng
                    {
                        // Gán UserName duy nhất dựa trên Dienthoai
                        string baseUserName = value;
                        string tenDangNhap = baseUserName;
                        int sott = 0;

                        ApplicationUser user = Session.FindObject<ApplicationUser>(CriteriaOperator.Parse("UserName = ?", tenDangNhap)); // Kiểm tra xem tên đăng nhập đã tồn tại chưa
                        while (user != null)
                        {
                            sott++;
                            tenDangNhap = baseUserName + sott;
                            user = Session.FindObject<ApplicationUser>( CriteriaOperator.Parse("UserName = ?", tenDangNhap));
                        }

                        this.UserName = tenDangNhap;
                        this.SetPassword(tenDangNhap + "@#");
                    }
                }
            }
        }

        private string _Email;
        [XafDisplayName("Email")]
        public string Email
        {
            get { return _Email; }
            set { SetPropertyValue<string>(nameof(Email), ref _Email, value); }
        }


        private string _CCCD;
        public string CCCD
        {
            get { return _CCCD; }
            set { SetPropertyValue<string>(nameof(CCCD), ref _CCCD, value); }
        }

        private string _Diachi;
        [Size(255), XafDisplayName("Địa chỉ")]
        public string Diachi
        {
            get { return _Diachi; }
            set { SetPropertyValue<string>(nameof(Diachi), ref _Diachi, value); }
        }

        private DateOnly _NgayDangky;
        [XafDisplayName("Ngày đăng ký")]
        [ModelDefault("EditMask", "dd/MM/yyyy")]
        [ModelDefault("DisplayFormat", "{0:dd/MM/yyyy}")]
        public DateOnly NgayDangky
        {
            get { return _NgayDangky; }
            set { SetPropertyValue<DateOnly>(nameof(NgayDangky), ref _NgayDangky, value); }
        }

        private int _Sodangky;
        [XafDisplayName("Số đăng ký"), ModelDefault("AllowEdit", "false")]
        public int Sodangky
        {
            get { return _Sodangky; }
            set { SetPropertyValue<int>(nameof(Sodangky), ref _Sodangky, value); }
        }



        private string _MsThue;
        [Size(50), XafDisplayName("MS Thuế")]
        public string MsThue
        {
            get { return _MsThue; }
            set { SetPropertyValue<string>(nameof(MsThue), ref _MsThue, value); }
        }


        private decimal _MsThueVAT;
        [Size(50), XafDisplayName("Mức thuế VAT ")]
        [ModelDefault("DisplayFormat", "{0:0.#}%")] // định dạng hiển thị %
        [ModelDefault("EditMask", "0.#")]// định dạng khi người dùng nhập
        public decimal MsThueVAT
        {
            get { return _MsThueVAT; }
            set { SetPropertyValue<decimal>(nameof(MsThueVAT), ref _MsThueVAT, value); }
        }

        private decimal _MsThueTNCN;

        [ModelDefault("DisplayFormat", "{0:0.#}%")]
        [ModelDefault("EditMask", "0.#")]
        [XafDisplayName("Mức thuế TNCN")]
        public decimal MsThueTNCN
        {
            get => _MsThueTNCN;
            set => SetPropertyValue(nameof(MsThueTNCN), ref _MsThueTNCN, value);
        }


        [Delayed(true), VisibleInListViewAttribute(false)]
        [ImageEditor(
            ListViewImageEditorMode = ImageEditorMode.PopupPictureEdit,
            DetailViewImageEditorMode = ImageEditorMode.PictureEdit,
            DetailViewImageEditorFixedHeight = 240,
            DetailViewImageEditorFixedWidth = 300,
            ListViewImageEditorCustomHeight = 40)]
        [XafDisplayName("Ảnh QR code")]
        public byte[] AnhQRCode
        {
            get { return GetDelayedPropertyValue<byte[]>(nameof(AnhQRCode)); }
            set { SetDelayedPropertyValue<byte[]>(nameof(AnhQRCode), value); }
        }



        [DevExpress.Xpo.Aggregated, Association]
        public XPCollection<Phiduytri> Phiduytris
        {
            get { return GetCollection<Phiduytri>(nameof(Phiduytris)); }
        }

        [DevExpress.Xpo.Aggregated, Association]
        public XPCollection<Phong> Phongs
        {
            get { return GetCollection<Phong>(nameof(Phongs)); }
        }


        [DevExpress.Xpo.Aggregated, Association]
        public XPCollection<HopDong> HopDongs
        {
            get { return GetCollection<HopDong>(nameof(HopDongs)); }
        }


        [DevExpress.Xpo.Aggregated, Association]
        public XPCollection<KhachThue> KhachThues
        {
            get { return GetCollection<KhachThue>(nameof(KhachThues)); }
        }

        [DevExpress.Xpo.Aggregated, Association]
        public XPCollection<TuyChon> TuyChons
        {
            get { return GetCollection<TuyChon>(nameof(TuyChons)); }
        }

        [DevExpress.Xpo.Aggregated, Association]
        public XPCollection<ThietBi> ThietBis
        {
            get { return GetCollection<ThietBi>(nameof(ThietBis)); }
        }

        [DevExpress.Xpo.Aggregated, Association]
        public XPCollection<HoaDon> HoaDons
        {
            get { return GetCollection<HoaDon>(nameof(HoaDons)); }
        }

        [DevExpress.Xpo.Aggregated, Association]
        public XPCollection<KhoanThu> KhoanThus
        {
            get { return GetCollection<KhoanThu>(nameof(KhoanThus)); }
        }

        [DevExpress.Xpo.Aggregated, Association]
        public XPCollection<PhieuThu> PhieuThus
        {
            get { return GetCollection<PhieuThu>(nameof(PhieuThus)); }
        }



        [DevExpress.Xpo.Aggregated, Association]
        public XPCollection<KhoanChi> KhoanChis
        {
            get { return GetCollection<KhoanChi>(nameof(KhoanChis)); }
        }


        [DevExpress.Xpo.Aggregated, Association]
        public XPCollection<PhieuChi> PhieuChis
        {
            get { return GetCollection<PhieuChi>(nameof(PhieuChis)); }
        }


        [DevExpress.Xpo.Aggregated, Association]
        public XPCollection<NhatKy> NhatKies
        {
            get { return GetCollection<NhatKy>(nameof(NhatKies)); }
        }


        private void Captaikhoan()
        {
            //role
            if (this.Roles.Count == 0)
            {
                PermissionPolicyRole defaultRole = Session.FindObject<PermissionPolicyRole>(CriteriaOperator.Parse("Name = ?", "Chutro")); // tìm trong danh sách role có tên là "Chutro"
                if (defaultRole != null)
                {
                    XafApplication xafApp = ApplicationHelper.Instance.Application;
                    using IObjectSpace objectSpace = xafApp.CreateObjectSpace<ApplicationUser>(); // tạo mới 1 object space để làm việc với đối tượng
                    ((ISecurityUserWithLoginInfo)this).CreateUserLoginInfo(SecurityDefaults.PasswordAuthentication,objectSpace.GetKeyValueAsString(this)); // tạo thông tin đăng nhập cho người dùng
                    this.Roles.Add(defaultRole);
                }
               
            }

        }

    }
}