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
using MyPhongTro.Module.BusinessObjects.Chutro;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace MyPhongTro.Module.BusinessObjects.Quanlykhanhthue
{
    [DefaultClassOptions]
    [ImageName("khanhthue")]
    [NavigationItem("Quản lý khách thuê")]
    [System.ComponentModel.DisplayName("Khách thuê")]
    [DefaultProperty("HoTen")]
    [DefaultListViewOptions(MasterDetailMode.ListViewOnly, true, NewItemRowPosition.Top)]
    //[Persistent("DatabaseTableName")]
    // Specify more UI options using a declarative approach (https://docs.devexpress.com/eXpressAppFramework/112701/business-model-design-orm/data-annotations-in-data-model).
    public class KhachThue(Session session) : ApplicationUser(session)
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://docs.devexpress.com/eXpressAppFramework/113146/business-model-design-orm/business-model-design-with-xpo/base-persistent-classes).
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://docs.devexpress.com/eXpressAppFramework/112834/getting-started/in-depth-tutorial-winforms-webforms/business-model-design/initialize-a-property-after-creating-an-object-xpo?v=22.1).
            if (Session.IsNewObject(this))
            {
                NgayDangky = TCom.GetServerDateOnly(); // Ngày đăng ký mặc định là ngày hiện tại
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

        private ChuTro _Chutro;
        [Association]
        [VisibleInListView(false), VisibleInLookupListView(false), VisibleInDetailView(false), VisibleInDashboards(false)]
        public ChuTro Chutro
        {
            get { return _Chutro; }
            set { SetPropertyValue<ChuTro>(nameof(Chutro), ref _Chutro, value); }
        }

        private string _HoTen;
        [XafDisplayName("Họ và tên"), Size(255)]
        [RuleRequiredField("HoTenKhach", DefaultContexts.Save, "Phải có HỌ TÊN người thuê")]
        public string HoTen
        {
            get { return _HoTen; }
            set { SetPropertyValue<string>(nameof(HoTen), ref _HoTen, value); }
        }

        private string _Dienthoai;
        [Size(50), XafDisplayName("Điện thoại")]
        [RuleRequiredField("PhoneKhach", DefaultContexts.Save, "Phải có điện thoại người thuê làm tài khoản")]
        public string Dienthoai
        {
            get { return _Dienthoai; }
            set
            {

                bool isModified = SetPropertyValue<string>(nameof(Dienthoai), ref _Dienthoai, value);
                if (Session.IsNewObject(this) && isModified)
                {
                    // Tự động tạo tài khoản người dùng nếu là chủ trọ mới
                    if (string.IsNullOrEmpty(UserName))
                    {
                        UserName = value; // Sử dụng số điện thoại làm tên đăng nhập

                    }
                }
            }
        }

        private string _Email;
        [XafDisplayName("Email"), Size(128), RuleUniqueValue]
        public string Email
        {
            get { return _Email; }
            set { SetPropertyValue<string>(nameof(Email), ref _Email, value); }
        }

        private string _SoCCCD;
        [XafDisplayName("Số CCCD")]
        public string SoCCCD
        {
            get { return _SoCCCD; }
            set { SetPropertyValue<string>(nameof(SoCCCD), ref _SoCCCD, value); }
        }


        private string _Diachi;
        [Size(255), XafDisplayName("Địa chỉ")]
        public string Diachi
        {
            get { return _Diachi; }
            set { SetPropertyValue<string>(nameof(Diachi), ref _Diachi, value); }
        }

        private string _MsThue;
        [Size(50), XafDisplayName("MS Thuế")]
        public string MsThue
        {
            get { return _MsThue; }
            set { SetPropertyValue<string>(nameof(MsThue), ref _MsThue, value); }
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

        [Delayed(true), VisibleInListViewAttribute(false)]
        [ImageEditor(
            ListViewImageEditorMode = ImageEditorMode.PopupPictureEdit,
            DetailViewImageEditorMode = ImageEditorMode.PictureEdit,
            DetailViewImageEditorFixedHeight = 240,
            DetailViewImageEditorFixedWidth = 300,
            ListViewImageEditorCustomHeight = 40)]
        [XafDisplayName("Mặt trước CCCD")]
        public byte[] AnhTruoc
        {
            get { return GetDelayedPropertyValue<byte[]>(nameof(AnhTruoc)); }
            set { SetDelayedPropertyValue<byte[]>(nameof(AnhTruoc), value); }
        }


        [Delayed(true), VisibleInListViewAttribute(false)]
        [ImageEditor(
            ListViewImageEditorMode = ImageEditorMode.PopupPictureEdit,
            DetailViewImageEditorMode = ImageEditorMode.PictureEdit,
            DetailViewImageEditorFixedHeight = 240,
            DetailViewImageEditorFixedWidth = 300,
            ListViewImageEditorCustomHeight = 40)]
        [XafDisplayName("Mặt sau CCCD")]
        public byte[] AnhSau
        {
            get { return GetDelayedPropertyValue<byte[]>(nameof(AnhSau)); }
            set { SetDelayedPropertyValue<byte[]>(nameof(AnhSau), value); }
        }

        //[Delayed(true), VisibleInListViewAttribute(false)]
        //[ImageEditor(
        //    ListViewImageEditorMode = ImageEditorMode.PopupPictureEdit,
        //    DetailViewImageEditorMode = ImageEditorMode.PictureEdit,
        //    DetailViewImageEditorFixedHeight = 240,
        //    DetailViewImageEditorFixedWidth = 300,
        //    ListViewImageEditorCustomHeight = 40)]
        //[XafDisplayName("Ảnh đại diện")]
        //public byte[] Anh
        //{
        //    get { return GetDelayedPropertyValue<byte[]>(nameof(Anh)); }
        //    set { SetDelayedPropertyValue<byte[]>(nameof(Anh), value); }
        //}


        [DevExpress.Xpo.Aggregated, Association]
        public XPCollection<TamTru> TamTrus
        {
            get { return GetCollection<TamTru>(nameof(TamTrus)); }
        }

        private void Captaikhoan()
        {
            string baseUserName = string.IsNullOrEmpty(this.UserName) ? this.Dienthoai : this.UserName;
            string tenDangNhap = baseUserName;

            int sott = 0;
            ApplicationUser user = Session.FindObject<ApplicationUser>(CriteriaOperator.Parse("UserName = ?", tenDangNhap));  // duyệt tring application có tồn tại usernam = tenDangNhap này chua
            while (user != null)
            {
                sott++;
                tenDangNhap = baseUserName + sott; // reset tên đăng nhập cũ rồi cộng thêm số thứ tự
                user = Session.FindObject<ApplicationUser>(CriteriaOperator.Parse("UserName = ?", tenDangNhap));
            }
            this.UserName = tenDangNhap; // Cập nhật tên đăng nhập duy nhất
            this.SetPassword(UserName + "@#");


            //role
            if (this.Roles.Count == 0)
            {
                PermissionPolicyRole role = Session.FindObject<PermissionPolicyRole>(CriteriaOperator.Parse("Name = ?", "Default"));
                if (role != null)
                {
                    XafApplication app = ApplicationHelper.Instance.Application;
                    using IObjectSpace objectSpace = app.CreateObjectSpace<ApplicationUser>();
                    ((ISecurityUserWithLoginInfo)this).CreateUserLoginInfo(SecurityDefaults.PasswordAuthentication, objectSpace.GetKeyValueAsString(this));
                    this.Roles.Add(role);
                }
            }

        }

    }
}