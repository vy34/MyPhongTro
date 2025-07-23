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

namespace MyPhongTro.Module.BusinessObjects.Quanlyphongtro
{
    [DefaultClassOptions]
    [ImageName("suachua")]
    [System.ComponentModel.DisplayName("Sửa chữa phòng")]
    [NavigationItem("Quản lý phòng trọ")]
    //[DefaultProperty("DisplayMemberNameForLookupEditorsOfThisType")]
    [DefaultListViewOptions(MasterDetailMode.ListViewOnly, true, NewItemRowPosition.Top)]
    //[Persistent("DatabaseTableName")]
    // Specify more UI options using a declarative approach (https://docs.devexpress.com/eXpressAppFramework/112701/business-model-design-orm/data-annotations-in-data-model).
    public class SuaChua(Session session) : BaseObject(session)
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

        private Phong _Phong;
        [Association]
        [XafDisplayName("Phòng")]
        public Phong Phong
        {
            get { return _Phong; }
            set { SetPropertyValue<Phong>(nameof(Phong), ref _Phong, value); }
        }

        private DateOnly _Ngay;
        [XafDisplayName("Ngày")]
        [ModelDefault("EditMask", "dd/MM/yyyy")]
        [ModelDefault("DisplayFormat", "{0:dd/MM/yyyy}")]
        public DateOnly Ngay
        {
            get { return _Ngay; }
            set { SetPropertyValue<DateOnly>(nameof(Ngay), ref _Ngay, value); }
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








    }
}