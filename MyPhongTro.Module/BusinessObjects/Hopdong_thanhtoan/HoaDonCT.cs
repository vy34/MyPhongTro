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
    public class HoaDonCT(Session session) : BaseObject(session)
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


        private KhoanThu _Khoanthu;
        [Association]
        [XafDisplayName("Khoản thu")]
        public KhoanThu Khoanthu
        {
            get { return _Khoanthu; }
            set { SetPropertyValue<KhoanThu>(nameof(Khoanthu), ref _Khoanthu, value); }
        }


        private int _Chisodau;
        [XafDisplayName("Chỉ số đầu")]
        public int Chisodau
        {
            get { return _Chisodau; }
            set { SetPropertyValue<int>(nameof(Chisodau), ref _Chisodau, value); }
        }



        private int _Chisocuoi;
        [XafDisplayName("Chỉ số cuối")]
        public int Chisocuoi
        {
            get { return _Chisocuoi; }
            set { SetPropertyValue<int>(nameof(Chisocuoi), ref _Chisocuoi, value); }
        }

        private decimal _DonGia;
        [ModelDefault("DisplayFormat", "{0:### ### ###}")]     //tự động
        [ModelDefault("EditMask", "### ### ###")]
        [XafDisplayName("Đơn giá")]
        public decimal DonGia
        {
            get { return _DonGia; }
            set { SetPropertyValue<decimal>(nameof(DonGia), ref _DonGia, value); }
        }

        private decimal _ThanhTien;
        [XafDisplayName("Thành tiền")]
        [ModelDefault("DisplayFormat", "{0:### ### ###}")]     //tự động
        [ModelDefault("EditMask", "### ### ###")]
        public decimal ThanhTien
        {
            get
            {
                return (int)(Chisocuoi - Chisodau) * DonGia;
            }

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