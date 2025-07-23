using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using MyPhongTro.Module.BusinessObjects.Cauhinhhethong;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace MyPhongTro.Module.BusinessObjects.Hopdong_thanhtoan
{
    [DefaultClassOptions]
    [NavigationItem(false)]
    //[ImageName("BO_Contact")]
    //[DefaultProperty("DisplayMemberNameForLookupEditorsOfThisType")]
    [DefaultListViewOptions(MasterDetailMode.ListViewOnly, true, NewItemRowPosition.Top)]
    //[Persistent("DatabaseTableName")]
    // Specify more UI options using a declarative approach (https://docs.devexpress.com/eXpressAppFramework/112701/business-model-design-orm/data-annotations-in-data-model).
    public class HopDongCT(Session session) : BaseObject(session)
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://docs.devexpress.com/eXpressAppFramework/113146/business-model-design-orm/business-model-design-with-xpo/base-persistent-classes).
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://docs.devexpress.com/eXpressAppFramework/112834/getting-started/in-depth-tutorial-winforms-webforms/business-model-design/initialize-a-property-after-creating-an-object-xpo?v=22.1).
        }

        private HopDong _Hopdong;
        [Association]
        [XafDisplayName("Hợp đồng")]
        public HopDong Hopdong
        {
            get { return _Hopdong; }
            set { SetPropertyValue<HopDong>(nameof(Hopdong), ref _Hopdong, value); }
        }
        private KhoanThu _Khoanthu;
        [Association]
        [XafDisplayName("Khoản thu")]
        public KhoanThu Khoanthu
        {
            get { return _Khoanthu; }
            set { SetPropertyValue<KhoanThu>(nameof(Khoanthu), ref _Khoanthu, value); }
        }

        private decimal _GiaThang;
        [XafDisplayName("Giá tháng")]
        [ModelDefault("DisplayFormat", "{0:### ### ###}")]     //tự động
        [ModelDefault("EditMask", "### ### ###")]
        public decimal GiaThang
        {
            get { return _GiaThang; }
            set { SetPropertyValue<decimal>(nameof(GiaThang), ref _GiaThang, value); }
        }

        private decimal _GiaChiso;
        [XafDisplayName("Giá chỉ số")]
        [ModelDefault("DisplayFormat", "{0:### ### ###}")]     //tự động
        [ModelDefault("EditMask", "### ### ###")]
        public decimal GiaChiso
        {
            get { return _GiaChiso; }
            set { SetPropertyValue<decimal>(nameof(GiaChiso), ref _GiaChiso, value); }
        }

        private string _Ghichu;
        [XafDisplayName("Ghi chú")]
        public string Ghichu
        {
            get { return _Ghichu; }
            set { SetPropertyValue<string>(nameof(Ghichu), ref _Ghichu, value); }
        }



    }
}