using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Editors;
using DevExpress.Persistent.Base;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using MyPhongTro.Module.BusinessObjects.Chutro;
using MyPhongTro.Module.BusinessObjects.Hopdong_thanhtoan;

namespace MyPhongTro.Module.BusinessObjects.Quanlyphongtro
{
    [DefaultClassOptions]
    [ImageName("phong")]
    [NavigationItem("Quản lý phòng trọ")]
    [System.ComponentModel.DisplayName("Danh sách phòng")]
    [DefaultProperty("Sophong")]
    [DefaultListViewOptions(MasterDetailMode.ListViewOnly, true, NewItemRowPosition.Top)]
    //[Persistent("DatabaseTableName")]
    // Specify more UI options using a declarative approach (https://docs.devexpress.com/eXpressAppFramework/112701/business-model-design-orm/data-annotations-in-data-model).
    public class Phong(Session session) : BaseObject(session)
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://docs.devexpress.com/eXpressAppFramework/113146/business-model-design-orm/business-model-design-with-xpo/base-persistent-classes).
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://docs.devexpress.com/eXpressAppFramework/112834/getting-started/in-depth-tutorial-winforms-webforms/business-model-design/initialize-a-property-after-creating-an-object-xpo?v=22.1).
            if(Session.IsNewObject(this))
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
                throw new UserFriendlyException("Không thể xóa phòng này vì có " + so + " đối tượng liên quan. Vui lòng xóa các đối tượng liên quan trước.");
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

        private string _Sophong;
        [XafDisplayName("Số phòng")]
        public string Sophong
        {
            get { return _Sophong; }
            set { SetPropertyValue<string>(nameof(Sophong), ref _Sophong, value); }
        }


        private decimal _Tiencoc;
        [XafDisplayName("Tiền cọc")]
        [ModelDefault("DisplayFormat", "{0:### ### ###}")]     //tự động
        [ModelDefault("EditMask", "### ### ###")]
        public decimal Tiencoc
        {
            get { return _Tiencoc; }
            set { SetPropertyValue<decimal>(nameof(Tiencoc), ref _Tiencoc, value); }
        }



        //private string _Mota;
        //[XafDisplayName("Mô tả")]
        //[Size(SizeAttribute.Unlimited)]
        //[EditorAlias(EditorAliases.RichTextPropertyEditor)] // cho phép sử dụng trình soạn thảo
        //public string Mota
        //{
        //    get { return _Mota; }
        //    set { SetPropertyValue<string>(nameof(Mota), ref _Mota, value); }
        //}

        [DevExpress.Xpo.Aggregated, Association]
        public XPCollection<Thietbiphong> Thietbiphongs
        {
            get { return GetCollection<Thietbiphong>(nameof(Thietbiphongs)); }
        }

        [DevExpress.Xpo.Aggregated, Association]
        public XPCollection<HopDong> HopDongs
        {
            get { return GetCollection<HopDong>(nameof(HopDongs)); }
        }

        [DevExpress.Xpo.Aggregated, Association]
        public XPCollection<SuaChua> SuaChuas
        {
            get { return GetCollection<SuaChua>(nameof(SuaChuas)); }
        }

    }
}