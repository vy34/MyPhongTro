using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using MyPhongTro.Module.BusinessObjects.Chutro;
using MyPhongTro.Module.BusinessObjects.Quanlykhanhthue;
using MyPhongTro.Module.BusinessObjects.Quanlyphongtro;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace MyPhongTro.Module.BusinessObjects.Hopdong_thanhtoan
{
    [DefaultClassOptions]
    [ImageName("hopdong")]
    [System.ComponentModel.DisplayName("Hợp đồng")]
    [NavigationItem("Hợp đồng thanh toán")]
    [DefaultProperty("SoHD")]
    [DefaultListViewOptions(MasterDetailMode.ListViewOnly, true, NewItemRowPosition.Top)]
    //[Persistent("DatabaseTableName")]
    // Specify more UI options using a declarative approach (https://docs.devexpress.com/eXpressAppFramework/112701/business-model-design-orm/data-annotations-in-data-model).
    public class HopDong(Session session) : BaseObject(session)
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://docs.devexpress.com/eXpressAppFramework/113146/business-model-design-orm/business-model-design-with-xpo/base-persistent-classes).
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://docs.devexpress.com/eXpressAppFramework/112834/getting-started/in-depth-tutorial-winforms-webforms/business-model-design/initialize-a-property-after-creating-an-object-xpo?v=22.1).
        if(Session.IsNewObject(this))
            {
                Ngayky = TCom.GetServerDateOnly(); // Ngày mặc định là ngày hiện tại
                Tungay = TCom.GetServerDateOnly(); // Ngày bắt đầu hợp đồng mặc định là ngày hiện tại
                Denngay = TCom.GetServerDateOnly().AddMonths(3); // Ngày kết thúc hợp đồng mặc định là 1 tháng sau ngày hiện tại
                Trangthai = TrangThaiHD.dukien;
                int maxSo = Session.Query<HopDong>().Max(x => (int?)x.SoHD) ?? 0;
                SoHD = maxSo + 1; // Tăng số đăng ký lên 
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


        private Phong _Phong;
        [Association]
        [XafDisplayName("Phòng")]
        public Phong Phong
        {
            get { return _Phong; }
            set { SetPropertyValue<Phong>(nameof(Phong), ref _Phong, value); }
        }

        private int _SoHD;
        [XafDisplayName("Số hợp đồng")]
        public int SoHD
        {
            get { return _SoHD; }
            set { SetPropertyValue<int>(nameof(SoHD), ref _SoHD, value); }
        }

        private DateOnly _Ngayky;
        [XafDisplayName("Ngày đăng ký")]
        [ModelDefault("EditMask", "dd/MM/yyyy")]
        [ModelDefault("DisplayFormat", "{0:dd/MM/yyyy}")]
        public DateOnly Ngayky
        {
            get { return _Ngayky; }
            set { SetPropertyValue<DateOnly>(nameof(Ngayky), ref _Ngayky, value); }
        }


        private DateOnly _Tungay;
        [XafDisplayName("Từ ngày")]
        [ModelDefault("EditMask", "dd/MM/yyyy")]
        [ModelDefault("DisplayFormat", "{0:dd/MM/yyyy}")]
        public DateOnly Tungay
        {
            get { return _Tungay; }
            set { SetPropertyValue<DateOnly>(nameof(Tungay), ref _Tungay, value); }
        }


        private DateOnly _Denngay;
        [XafDisplayName("Đến ngày")]
        [ModelDefault("EditMask", "dd/MM/yyyy")]
        [ModelDefault("DisplayFormat", "{0:dd/MM/yyyy}")]
        public DateOnly Denngay
        {
            get { return _Denngay; }
            set { SetPropertyValue<DateOnly>(nameof(Denngay), ref _Denngay, value); }
        }


        private string _Ghichu;
        [XafDisplayName("Ghi chú")]
        public string Ghichu
        {
            get { return _Ghichu; }
            set { SetPropertyValue<string>(nameof(Ghichu), ref _Ghichu, value); }
        }
       

        public enum TrangThaiHD
        {
            [XafDisplayName("Dự kiến")] dukien = 0,
            [XafDisplayName("Đang thuê")] dangthue = 1,
            [XafDisplayName("Kết thúc")] ketthuc = 2
        }

        private TrangThaiHD _Trangthai;
        [XafDisplayName("Trạng thái")]
        public TrangThaiHD Trangthai
        {
            get { return _Trangthai; }
            set { SetPropertyValue<TrangThaiHD>(nameof(Trangthai), ref _Trangthai, value); }
        }

        [DevExpress.Xpo.Aggregated, Association]
        public XPCollection<HopDongCT> HopDongCTs
        {
            get { return GetCollection<HopDongCT>(nameof(HopDongCTs)); }
        }

        [DevExpress.Xpo.Aggregated, Association]
        public XPCollection<HoaDon> HoaDons
        {
            get { return GetCollection<HoaDon>(nameof(HoaDons)); }
        }

        [DevExpress.Xpo.Aggregated, Association]
        public XPCollection<Chiso> Chisos
        {
            get { return GetCollection<Chiso>(nameof(Chisos)); }
        }


        [DevExpress.Xpo.Aggregated, Association]
        public XPCollection<PhieuChi> PhieuChis
        {
            get { return GetCollection<PhieuChi>(nameof(PhieuChis)); }
        }

        [DevExpress.Xpo.Aggregated, Association]
        public XPCollection<TamTru> TamTrus
        {
            get { return GetCollection<TamTru>(nameof(TamTrus)); }
        }
        






















    }
}