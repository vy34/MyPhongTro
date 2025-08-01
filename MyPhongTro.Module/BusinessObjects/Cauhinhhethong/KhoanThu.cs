using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using MyPhongTro.Module.BusinessObjects.Chutro;
using MyPhongTro.Module.BusinessObjects.Hopdong_thanhtoan;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace MyPhongTro.Module.BusinessObjects.Cauhinhhethong
{
    [DefaultClassOptions]
    [ImageName("khoanthu")]
    [System.ComponentModel.DisplayName("Các khoản thu")]
    [NavigationItem("Cấu hình hệ thống")]
    [DefaultProperty("TenKhoanThu")]
    [DefaultListViewOptions(MasterDetailMode.ListViewOnly, true, NewItemRowPosition.Top)]
    //[Persistent("DatabaseTableName")]
    // Specify more UI options using a declarative approach (https://docs.devexpress.com/eXpressAppFramework/112701/business-model-design-orm/data-annotations-in-data-model).
    public class KhoanThu(Session session) : BaseObject(session)
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
            }
        }
        protected override void OnDeleting()
        {
            base.OnDeleting();
            int so = Session.CollectReferencingObjects(this).Count;
            if (so > 0)
            {
                throw new UserFriendlyException("Không thể xóa khoản thu này vì có " + so + " đối tượng liên quan. Vui lòng xóa các đối tượng liên quan trước.");
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

        private string _TenKhoanThu;
        [XafDisplayName("Tên khoản thu")]
        public string TenKhoanThu
        {
            get { return _TenKhoanThu; }
            set { SetPropertyValue<string>(nameof(TenKhoanThu), ref _TenKhoanThu, value); }
        }

        private decimal _Dongia;
        [XafDisplayName("Đơn giá")]
        [ModelDefault("DisplayFormat", "{0:### ### ###}")]     //tự động
        [ModelDefault("EditMask", "### ### ###")]
        public decimal Dongia
        {
            get { return _Dongia; }
            set { SetPropertyValue<decimal>(nameof(Dongia), ref _Dongia, value); }
        }

        private bool _Theochiso;
        [XafDisplayName("Theo chỉ số")]
        public bool Theochiso
        {
            get { return _Theochiso; }
            set { SetPropertyValue<bool>(nameof(Theochiso), ref _Theochiso, value); }
        }

        private bool _Def;
        [XafDisplayName("Mặc định")]
        public bool Def
        {
            get { return _Def; }
            set { SetPropertyValue<bool>(nameof(Def), ref _Def, value); }
        }

        [DevExpress.Xpo.Aggregated, Association]
        public XPCollection<HopDongCT> HopDongCTs
        {
            get { return GetCollection<HopDongCT>(nameof(HopDongCTs)); }
        }



        [DevExpress.Xpo.Aggregated, Association]
        public XPCollection<HoaDonCT> HoaDonCTs
        {
            get { return GetCollection<HoaDonCT>(nameof(HoaDonCTs)); }
        }

     








    }
}