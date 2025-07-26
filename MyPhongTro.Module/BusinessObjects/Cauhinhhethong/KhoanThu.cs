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

namespace MyPhongTro.Module.BusinessObjects.Cauhinhhethong
{
    [DefaultClassOptions]
    [ImageName("khoanthu")]
    [System.ComponentModel.DisplayName("Các khoản thu")]
    [NavigationItem("Cấu hình hệ thống")]
    [DefaultProperty("TenKhoanThu")]
    [DefaultListViewOptions(MasterDetailMode.ListViewOnly, true, NewItemRowPosition.Top)]
    //[Persistent("DatabaseTableName")]
    // Specify more UI options using a declarative approach (https://docs.devexpress.com/eXpressAppFramework/112701/business-model-design-orm/data-annotations-in-data-model).
    public class KhoanThu(Session session) : BaseObject(session)
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://docs.devexpress.com/eXpressAppFramework/113146/business-model-design-orm/business-model-design-with-xpo/base-persistent-classes).
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://docs.devexpress.com/eXpressAppFramework/112834/getting-started/in-depth-tutorial-winforms-webforms/business-model-design/initialize-a-property-after-creating-an-object-xpo?v=22.1).
        }

        private ChuTro _Chutro;
        [Association]
        [VisibleInListView(false), VisibleInLookupListView(false), VisibleInDetailView(false), VisibleInDashboards(false)]
        public ChuTro Chutro
        {
            get { return _Chutro; }
            set { SetPropertyValue<ChuTro>(nameof(Chutro), ref _Chutro, value); }
        }

        private string _TenKhoanThu;
        [XafDisplayName("Tên khoản thu")]
        public string TenKhoanThu
        {
            get { return _TenKhoanThu; }
            set { SetPropertyValue<string>(nameof(TenKhoanThu), ref _TenKhoanThu, value); }
        }

        private decimal _GiaThang;
        [XafDisplayName("Giá theo tháng")]
        [ModelDefault("DisplayFormat", "{0:### ### ###}")]     //tự động
        [ModelDefault("EditMask", "### ### ###")]
        public decimal GiaThang
        {
            get { return _GiaThang; }
            set { SetPropertyValue<decimal>(nameof(GiaThang), ref _GiaThang, value); }
        }

        private bool _GiaChiso;
        [XafDisplayName("Đơn giá chỉ số")]
        public bool GiaChiso
        {
            get { return _GiaChiso; }
            set { SetPropertyValue<bool>(nameof(GiaChiso), ref _GiaChiso, value); }
        }

        private bool _Def;
        [XafDisplayName("Mặc định")]
        public bool Def
        {
            get { return _Def; }
            set { SetPropertyValue<bool>(nameof(Def), ref _Def, value); }
        }

        [DevExpress.Xpo.Aggregated, Association]
        public XPCollection<HopDongCT> HopDongCTs
        {
            get { return GetCollection<HopDongCT>(nameof(HopDongCTs)); }
        }



        [DevExpress.Xpo.Aggregated, Association]
        public XPCollection<HoaDonCT> HoaDonCTs
        {
            get { return GetCollection<HoaDonCT>(nameof(HoaDonCTs)); }
        }

     








    }
}