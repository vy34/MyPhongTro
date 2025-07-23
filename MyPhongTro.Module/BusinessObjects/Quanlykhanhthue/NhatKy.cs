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

namespace MyPhongTro.Module.BusinessObjects.Quanlykhanhthue
{
    [DefaultClassOptions]
    [ImageName("nhatky")]
    [NavigationItem("Quản lý khách thuê")]
    [System.ComponentModel.DisplayName("Nhật ký hoạt động")]
    //[DefaultProperty("DisplayMemberNameForLookupEditorsOfThisType")]
    [DefaultListViewOptions(MasterDetailMode.ListViewOnly, true, NewItemRowPosition.Top)]
    //[Persistent("DatabaseTableName")]
    // Specify more UI options using a declarative approach (https://docs.devexpress.com/eXpressAppFramework/112701/business-model-design-orm/data-annotations-in-data-model).
    public class NhatKy(Session session) : BaseObject(session)
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://docs.devexpress.com/eXpressAppFramework/113146/business-model-design-orm/business-model-design-with-xpo/base-persistent-classes).
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://docs.devexpress.com/eXpressAppFramework/112834/getting-started/in-depth-tutorial-winforms-webforms/business-model-design/initialize-a-property-after-creating-an-object-xpo?v=22.1).
        if(Session.IsNewObject(this))
            {
                Ngay = TCom.GetServerDateOnly(); // Ngày mặc định là ngày hiện tại
            
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

        private TamTru _Tamtru;
        [Association]
        [XafDisplayName("Khách tạm trú")]
        //[VisibleInListView(false), VisibleInLookupListView(false), VisibleInDetailView(false), VisibleInDashboards(false)]
        public TamTru Tamtru
        {
            get { return _Tamtru; }
            set { SetPropertyValue<TamTru>(nameof(Tamtru), ref _Tamtru, value); }
        }

        private DateOnly _Ngay;
        [XafDisplayName("Ngày lập")]
        [ModelDefault("EditMask", "dd/MM/yyyy")]
        [ModelDefault("DisplayFormat", "{0:dd/MM/yyyy}")]
        public DateOnly Ngay
        {
            get { return _Ngay; }
            set { SetPropertyValue<DateOnly>(nameof(Ngay), ref _Ngay, value); }
        }


        private string _Noidung;
        [Size(255)]
        [XafDisplayName("Nội dung")]
        public string Noidung
        {
            get { return _Noidung; }
            set { SetPropertyValue<string>(nameof(Noidung), ref _Noidung, value); }
        }

        [Delayed(true), VisibleInListViewAttribute(false)]
        [ImageEditor(
            ListViewImageEditorMode = ImageEditorMode.PopupPictureEdit,
            DetailViewImageEditorMode = ImageEditorMode.PictureEdit, 
            DetailViewImageEditorFixedHeight = 240, 
            DetailViewImageEditorFixedWidth = 300,
            ListViewImageEditorCustomHeight = 40)]
        [XafDisplayName("Ảnh")]
        public byte[] Anh
        {
            get { return GetDelayedPropertyValue<byte[]>(nameof(Anh)); }
            set { SetDelayedPropertyValue<byte[]>(nameof(Anh), value); }
        }
    }
}