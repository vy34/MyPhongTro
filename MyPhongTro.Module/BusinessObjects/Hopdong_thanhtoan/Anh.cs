using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace MyPhongTro.Module.BusinessObjects.Hopdong_thanhtoan
{
    [DefaultClassOptions]
    [NavigationItem(false)]
    [ImageName("image")]
    //[DefaultProperty("Photo")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Persistent("DatabaseTableName")]
    // Specify more UI options using a declarative approach (https://docs.devexpress.com/eXpressAppFramework/112701/business-model-design-orm/data-annotations-in-data-model).
    public class Anh(Session session) : BaseObject(session)
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://docs.devexpress.com/eXpressAppFramework/113146/business-model-design-orm/business-model-design-with-xpo/base-persistent-classes).
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://docs.devexpress.com/eXpressAppFramework/112834/getting-started/in-depth-tutorial-winforms-webforms/business-model-design/initialize-a-property-after-creating-an-object-xpo?v=22.1).
        }


        private HoaDon _Hoadon;
        [Association]
        [XafDisplayName("Hóa đơn")]
        public HoaDon Hoadon
        {
            get { return _Hoadon; }
            set { SetPropertyValue<HoaDon>(nameof(Hoadon), ref _Hoadon, value); }
        }


        [Delayed(true), VisibleInListViewAttribute(true)]
        [ImageEditor(
            ListViewImageEditorMode = ImageEditorMode.PopupPictureEdit,
            DetailViewImageEditorMode = ImageEditorMode.PictureEdit,
            DetailViewImageEditorFixedHeight = 240,
            DetailViewImageEditorFixedWidth = 300,
            ListViewImageEditorCustomHeight = 40)]
        [XafDisplayName("Thêm minh chứng")]
        public byte[] Photo
        {
            get { return GetDelayedPropertyValue<byte[]>(nameof(Photo)); }
            set { SetDelayedPropertyValue<byte[]>(nameof(Photo), value); }
        }
    }
}