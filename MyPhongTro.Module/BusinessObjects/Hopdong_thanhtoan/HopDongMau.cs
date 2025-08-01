using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using MyPhongTro.Module.BusinessObjects.Chutro;
using System.ComponentModel;


namespace MyPhongTro.Module.BusinessObjects.Hopdong_thanhtoan
{
    [DefaultClassOptions]
    [ImageName("mauhopdong")]
    [DefaultProperty("Tenmau")]
    [DefaultListViewOptions(MasterDetailMode.ListViewOnly, true, NewItemRowPosition.Top)]
    //[Persistent("DatabaseTableName")]
    // Specify more UI options using a declarative approach (https://docs.devexpress.com/eXpressAppFramework/112701/business-model-design-orm/data-annotations-in-data-model).
    public class HopDongMau(Session session) : BaseObject(session)
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





        private ChuTro _Chutro;
        [Association]
        [VisibleInListView(false), VisibleInLookupListView(false), VisibleInDetailView(false), VisibleInDashboards(false)]

        public ChuTro Chutro
        {
            get { return _Chutro; }
            set { SetPropertyValue<ChuTro>(nameof(Chutro), ref _Chutro, value); }
        }



        private string _Tenmau;
        [XafDisplayName("Tên mẫu hợp đồng")]
        public string Tenmau
        {
            get { return _Tenmau; }
            set { SetPropertyValue<string>(nameof(Tenmau), ref _Tenmau, value); }
        }

        [Size(SizeAttribute.Unlimited)]
        private string _Noidung;
        [XafDisplayName("Nội dung hợp đồng")]
        public string Noidung
        {
            get { return _Noidung; }
            set { SetPropertyValue<string>(nameof(Noidung), ref _Noidung, value); }
        }






    }
}