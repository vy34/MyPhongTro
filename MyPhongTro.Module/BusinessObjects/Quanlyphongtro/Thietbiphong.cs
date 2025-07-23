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

namespace MyPhongTro.Module.BusinessObjects.Quanlyphongtro
{
    [DefaultClassOptions]
    [ImageName("thietbiphong")]
    [NavigationItem("Quản lý phòng trọ")]
    [System.ComponentModel.DisplayName("Thiết bị phòng")]
    //[DefaultProperty("Phong")]
    [DefaultListViewOptions(MasterDetailMode.ListViewOnly, true, NewItemRowPosition.Top)]
    //[Persistent("DatabaseTableName")]
    // Specify more UI options using a declarative approach (https://docs.devexpress.com/eXpressAppFramework/112701/business-model-design-orm/data-annotations-in-data-model).
    public class Thietbiphong(Session session) : BaseObject(session)
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://docs.devexpress.com/eXpressAppFramework/113146/business-model-design-orm/business-model-design-with-xpo/base-persistent-classes).
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://docs.devexpress.com/eXpressAppFramework/112834/getting-started/in-depth-tutorial-winforms-webforms/business-model-design/initialize-a-property-after-creating-an-object-xpo?v=22.1).
        if(Session.IsNewObject(this))
            {
                NgaySD = TCom.GetServerDateOnly(); // Ngày sử dụng mặc định là ngày hiện tại
                HanBH = TCom.GetServerDateOnly().AddYears(1); // Hạn bảo hành mặc định là 1 năm sau ngày sử dụng
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

        private ThietBi _Thietbi;
        [Association]
        [XafDisplayName("Thiết bị")]
        public ThietBi Thietbi
        {
            get { return _Thietbi; }
            set { SetPropertyValue<ThietBi>(nameof(Thietbi), ref _Thietbi, value); }
        }

        private DateOnly _NgaySD;
        [XafDisplayName("Ngày sử dụng")]
        [ModelDefault("EditMask", "dd/MM/yyyy")]
        [ModelDefault("DisplayFormat", "{0:dd/MM/yyyy}")]
        public DateOnly NgaySD
        {
            get { return _NgaySD; }
            set { SetPropertyValue<DateOnly>(nameof(NgaySD), ref _NgaySD, value); }
        }


        private DateOnly _HanBH;
        [XafDisplayName("Hạn bảo hành")]
        [ModelDefault("EditMask", "dd/MM/yyyy")]
        [ModelDefault("DisplayFormat", "{0:dd/MM/yyyy}")]
        public DateOnly HanBH
        {
            get { return _HanBH; }
            set { SetPropertyValue<DateOnly>(nameof(HanBH), ref _HanBH, value); }
        }

        private string _Thongso;
        [XafDisplayName("Thông số")]
        public string Thongso
        {
            get { return _Thongso; }
            set { SetPropertyValue<string>(nameof(Thongso), ref _Thongso, value); }
        }

        private string _NhaCungcap;
        [XafDisplayName("Nhà cung cấp")]
        public string NhaCungcap
        {
            get { return _NhaCungcap; }
            set { SetPropertyValue<string>(nameof(NhaCungcap), ref _NhaCungcap, value); }
        }

        [DevExpress.Xpo.Aggregated, Association]
        public XPCollection<BaoTri> BaoTris
        {
            get { return GetCollection<BaoTri>(nameof(BaoTris)); }
        }





    }
}