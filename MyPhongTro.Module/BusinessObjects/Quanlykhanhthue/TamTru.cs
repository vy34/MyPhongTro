using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using MyPhongTro.Module.BusinessObjects.Hopdong_thanhtoan;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace MyPhongTro.Module.BusinessObjects.Quanlykhanhthue
{
    [DefaultClassOptions]
    [ImageName("tamtru")]
    [NavigationItem("Quản lý khách thuê")]
    [System.ComponentModel.DisplayName("Danh sách tạm trú")]
    [DefaultProperty("Khachthue")]
    [DefaultListViewOptions(MasterDetailMode.ListViewOnly, true, NewItemRowPosition.Top)]
    //[Persistent("DatabaseTableName")]
    // Specify more UI options using a declarative approach (https://docs.devexpress.com/eXpressAppFramework/112701/business-model-design-orm/data-annotations-in-data-model).
    public class TamTru(Session session) : BaseObject(session)
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://docs.devexpress.com/eXpressAppFramework/113146/business-model-design-orm/business-model-design-with-xpo/base-persistent-classes).
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://docs.devexpress.com/eXpressAppFramework/112834/getting-started/in-depth-tutorial-winforms-webforms/business-model-design/initialize-a-property-after-creating-an-object-xpo?v=22.1).
        if(Session.IsNewObject(this))
            {
                //TuNgay = TCom.GetServerDateOnly(); // Ngày mặc định là ngày hiện tại
                //DenNgay = TCom.GetServerDateOnly(); // Ngày mặc định là ngày hiện tại
            }
        }
        private HopDong _Hopdong;
        [Association]
        [XafDisplayName("Hợp đồng")]
        public HopDong Hopdong
        {
            get { return _Hopdong; }
            set
            {
                bool isModified = SetPropertyValue<HopDong>(nameof(Hopdong), ref _Hopdong, value);
                if (isModified && !IsDeleted && !IsLoading && !IsSaving && value != null)
                {
                    // Nếu hợp đồng được gán, tự động lấy ngày bắt đầu và kết thúc từ hợp đồng
                    TuNgay = value.Tungay;
                    DenNgay = value.Denngay;
                }
            }
        }


        private KhachThue _Khachthue;
        [Association]
        [XafDisplayName("Khách thuê")]
        public KhachThue Khachthue
        {
            get { return _Khachthue; }
            set { SetPropertyValue<KhachThue>(nameof(Khachthue), ref _Khachthue, value); }
        }



        private DateOnly _TuNgay;
        [XafDisplayName("Từ ngày")]
        [ModelDefault("EditMask", "dd/MM/yyyy")]
        [ModelDefault("DisplayFormat", "{0:dd/MM/yyyy}")]
        public DateOnly TuNgay
        {
            get { return _TuNgay; }
            set { SetPropertyValue<DateOnly>(nameof(TuNgay), ref _TuNgay, value); }
        }

        private DateOnly _DenNgay;
        [XafDisplayName("Đến ngày")]
        [ModelDefault("EditMask", "dd/MM/yyyy")]
        [ModelDefault("DisplayFormat", "{0:dd/MM/yyyy}")]
        public DateOnly DenNgay
        {
            get { return _DenNgay; }
            set { SetPropertyValue<DateOnly>(nameof(DenNgay), ref _DenNgay, value); }
        }

        private string _GhiChu;
        [Size(255)]
        [XafDisplayName("Ghi chú")]
        public string GhiChu
        {
            get { return _GhiChu; }
            set { SetPropertyValue<string>(nameof(GhiChu), ref _GhiChu, value); }
        }


        [DevExpress.Xpo.Aggregated, Association]
        public XPCollection<PhieuThu> PhieuThus
        {
            get { return GetCollection<PhieuThu>(nameof(PhieuThus)); }
        }


        [DevExpress.Xpo.Aggregated, Association]
        public XPCollection<PhieuChi> PhieuChis
        {
            get { return GetCollection<PhieuChi>(nameof(PhieuChis)); }
        }


        [DevExpress.Xpo.Aggregated, Association]
        public XPCollection<NhatKy> NhatKies
        {
            get { return GetCollection<NhatKy>(nameof(NhatKies)); }
        }


    }
}