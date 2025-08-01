using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using System;
using System.ComponentModel;
using System.Linq;

namespace MyPhongTro.Module.BusinessObjects.Chutro
{
    [DefaultClassOptions]
    [ImageName("nganhang")]
    [DefaultProperty("Ten")]
    [DefaultListViewOptions(MasterDetailMode.ListViewOnly, true, NewItemRowPosition.Top)]
    //[Persistent("DatabaseTableName")]
    // Specify more UI options using a declarative approach (https://docs.devexpress.com/eXpressAppFramework/112701/business-model-design-orm/data-annotations-in-data-model).
    public class NganHang(Session session) : BaseObject(session)
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://docs.devexpress.com/eXpressAppFramework/113146/business-model-design-orm/business-model-design-with-xpo/base-persistent-classes).
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://docs.devexpress.com/eXpressAppFramework/112834/getting-started/in-depth-tutorial-winforms-webforms/business-model-design/initialize-a-property-after-creating-an-object-xpo?v=22.1).
        }



        private string _Ten;
        [XafDisplayName("Tên ngân hàng")]
        public string Ten
        {
            get { return _Ten; }
            set { SetPropertyValue<string>(nameof(Ten), ref _Ten, value); }
        }


        private string _Tentat;
        [XafDisplayName("Tên viết tắt")]
        public string Tentat
        {
            get { return _Tentat; }
            set { SetPropertyValue<string>(nameof(Tentat), ref _Tentat, value); }
        }


        private string _Ma;
        [XafDisplayName("Mã ngân hàng")]
        public string Ma
        {
            get { return _Ma; }
            set { SetPropertyValue<string>(nameof(Ma), ref _Ma, value); }
        }


        private int _Bin;
        [XafDisplayName("Mã BIN")]
        public int Bin
        {
            get { return _Bin; }
            set { SetPropertyValue<int>(nameof(Bin), ref _Bin, value); }
        }


        [DevExpress.Xpo.Aggregated, Association]
        public XPCollection<ChuTro> ChuTros
        {
            get { return GetCollection<ChuTro>(nameof(ChuTros)); }
        }








    }
}