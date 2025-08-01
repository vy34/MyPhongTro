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
    [ImageName("khoanchi")]
    [System.ComponentModel.DisplayName("Khoản chi")]
    [NavigationItem("Cấu hình hệ thống")]
    [DefaultProperty("TenKhoanChi")]
    [DefaultListViewOptions(MasterDetailMode.ListViewOnly, true, NewItemRowPosition.Top)]
    //[Persistent("DatabaseTableName")]
    // Specify more UI options using a declarative approach (https://docs.devexpress.com/eXpressAppFramework/112701/business-model-design-orm/data-annotations-in-data-model).
    public class KhoanChi(Session session) : BaseObject(session)
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
                throw new UserFriendlyException("Không thể xóa khoản chi này vì có " + so + " đối tượng liên quan. Vui lòng xóa các đối tượng liên quan trước.");
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

        private string _TenKhoanChi;
        [XafDisplayName("Khoản chi")]
        public string TenKhoanChi
        {
            get { return _TenKhoanChi; }
            set { SetPropertyValue<string>(nameof(TenKhoanChi), ref _TenKhoanChi, value); }
        }


        [DevExpress.Xpo.Aggregated, Association]
        public XPCollection<PhieuChi> PhieuChis
        {
            get { return GetCollection<PhieuChi>(nameof(PhieuChis)); }
        }

    }
}