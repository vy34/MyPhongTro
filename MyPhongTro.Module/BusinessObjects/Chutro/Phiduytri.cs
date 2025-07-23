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

namespace MyPhongTro.Module.BusinessObjects.Chutro
{
    [DefaultClassOptions]
    [ImageName("phiduytri")]
    [System.ComponentModel.DisplayName("Phí duy trì")]
    [NavigationItem("Chủ trọ")]
    //[DefaultProperty("DisplayMemberNameForLookupEditorsOfThisType")]
    [DefaultListViewOptions(MasterDetailMode.ListViewOnly, true, NewItemRowPosition.Top)]
    //[Persistent("DatabaseTableName")]
    // Specify more UI options using a declarative approach (https://docs.devexpress.com/eXpressAppFramework/112701/business-model-design-orm/data-annotations-in-data-model).
    public class Phiduytri(Session session) : BaseObject(session)
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://docs.devexpress.com/eXpressAppFramework/113146/business-model-design-orm/business-model-design-with-xpo/base-persistent-classes).
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://docs.devexpress.com/eXpressAppFramework/112834/getting-started/in-depth-tutorial-winforms-webforms/business-model-design/initialize-a-property-after-creating-an-object-xpo?v=22.1).
        if(Session.IsNewObject(this))
            {
                Ngaynop = TCom.GetServerDateOnly(); // Ngày nộp mặc định là ngày hiện tại
               
            }
        }



        private ChuTro _Chutro;
        [Association]
        [XafDisplayName("Chủ trọ")]
        public ChuTro Chutro
        {
            get { return _Chutro; }
            set { SetPropertyValue<ChuTro>(nameof(Chutro), ref _Chutro, value); }
        }

        private DateOnly _Ngaynop;
        [XafDisplayName("Ngày nộp")]
        [ModelDefault("EditMask", "dd/MM/yyyy")]
        [ModelDefault("DisplayFormat", "{0:dd/MM/yyyy}")]
        public DateOnly Ngaynop
        {
            get { return _Ngaynop; }
            set { SetPropertyValue<DateOnly>(nameof(Ngaynop), ref _Ngaynop, value); }
        }

        private decimal _Sotien;
        [XafDisplayName("Số tiền")]
        [ModelDefault("DisplayFormat", "{0:### ### ###}")]     //tự động
        [ModelDefault("EditMask", "### ### ###")]
        public decimal Sotien
        {
            get { return _Sotien; }
            set { SetPropertyValue<decimal>(nameof(Sotien), ref _Sotien, value); }
        }


        private string _Noidung;
        [XafDisplayName("Nội dung")]
        public string Noidung
        {
            get { return _Noidung; }
            set { SetPropertyValue<string>(nameof(Noidung), ref _Noidung, value); }
        }


        private DateOnly _Hangsudung;
        [XafDisplayName("Hạn sử dụng")]
        public DateOnly Hangsudung
        {
            get { return _Hangsudung; }
            set { SetPropertyValue<DateOnly>(nameof(Hangsudung), ref _Hangsudung, value); }
        }

        [Delayed(true), VisibleInListViewAttribute(false)] //đặt false để ẩn ảnh nếu k cần trong listView
        [ImageEditor(
            ListViewImageEditorMode = ImageEditorMode.PopupPictureEdit, // ảnh sẽ hiển thị dạng poppup trong listView
            DetailViewImageEditorMode = ImageEditorMode.PictureEdit,    //trong detailView dùng control để nhập or hiện thị ảnh
            DetailViewImageEditorFixedHeight = 240,
            DetailViewImageEditorFixedWidth = 300,
            ListViewImageEditorCustomHeight = 40)
         ]

        [XafDisplayName("Ảnh chuyển khoản")]
        public byte[] Anhthanhtoan
        {
            get { return GetDelayedPropertyValue<byte[]>(nameof(Anhthanhtoan)); }
            set { SetDelayedPropertyValue<byte[]>(nameof(Anhthanhtoan), value); }
        }

        private bool _Locked;
        [XafDisplayName("Khóa tài khoản")]
        public bool Locked
        {
            get { return _Locked; }
            set { SetPropertyValue<bool>(nameof(Locked), ref _Locked, value); }
        }






    }



    }