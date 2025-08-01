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
    [ImageName("diaphuong")]
    [DefaultProperty("Ma")]
    [DefaultListViewOptions(MasterDetailMode.ListViewOnly, true, NewItemRowPosition.Top)]
    //[Persistent("DatabaseTableName")]
    // Specify more UI options using a d    eclarative approach (https://docs.devexpress.com/eXpressAppFramework/112701/business-model-design-orm/data-annotations-in-data-model).
    public class DiaPhuong(Session session) : BaseObject(session)
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://docs.devexpress.com/eXpressAppFramework/113146/business-model-design-orm/business-model-design-with-xpo/base-persistent-classes).
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://docs.devexpress.com/eXpressAppFramework/112834/getting-started/in-depth-tutorial-winforms-webforms/business-model-design/initialize-a-property-after-creating-an-object-xpo?v=22.1).
        }


        private string _Ma;
        [XafDisplayName("Mã")]
        public string Ma
        {
            get { return _Ma; }
            set { SetPropertyValue<string>(nameof(Ma), ref _Ma, value); }
        }


        private string _Ten;
        [XafDisplayName("Tên địa phương")]
        public string Ten
        {
            get { return _Ten; }
            set { SetPropertyValue<string>(nameof(Ten), ref _Ten, value); }
        }



        [DevExpress.Xpo.Aggregated, Association]
        public XPCollection<ChuTro> ChuTros
        {
            get { return GetCollection<ChuTro>(nameof(ChuTros)); }
        }






    }
}