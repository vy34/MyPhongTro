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
    [DefaultProperty("Ten")]
    [DefaultListViewOptions(MasterDetailMode.ListViewOnly, true, NewItemRowPosition.Top)]
    //[Persistent("DatabaseTableName")]
    // Specify more UI options using a declarative approach (https://docs.devexpress.com/eXpressAppFramework/112701/business-model-design-orm/data-annotations-in-data-model).
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



        private DiaPhuong _CapTren;
        [XafDisplayName("Cấp trên")]
        [Association("Diaphuong-CapTren")]
        public DiaPhuong CapTren
        {
            get { return _CapTren; }
            set { SetPropertyValue<DiaPhuong>(nameof(CapTren), ref _CapTren, value); }
        }
        
        [Association("Diaphuong-CapTren")] 
        public XPCollection<DiaPhuong> DiaPhuongCons
        {
            get { return GetCollection<DiaPhuong>(nameof(DiaPhuongCons)); }
        }

        [Association("Chutro-Diaphuong-TinhThanh")]
        public XPCollection<ChuTro> ChutroTinhs
        {
            get { return GetCollection<ChuTro>(nameof(ChutroTinhs)); }
        }

        [Association("Chutro-Diaphuong-Xa")]
        public XPCollection<ChuTro> ChutroXas
        {
            get { return GetCollection<ChuTro>(nameof(ChutroXas)); }
        }



    }
}