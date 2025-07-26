using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using DevExpress.XtraRichEdit.API.Native;
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
    [DefaultProperty("SoHopdong")]
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
                ChuTro chutro = Session.FindObject<ChuTro>(CriteriaOperator.Parse("Oid = ?",SecuritySystem.CurrentUserId));
                if (chutro != null)
                {
                    Chutro = chutro; // Tự động gán chủ trọ là người dùng hiện tại
                }
                
                string sql = "select max(SoHD) as so from HopDong where Chutro = '" +  SecuritySystem.CurrentUserId + "'";
                var ret = Session.ExecuteScalar(sql);
                int so = 1;
                if(ret != null) so = tmLib.ViCom.CInt(ret) + 1; // Lấy số hợp đồng lớn nhất của chủ trọ hiện tại
                SoHD = so; // Số hợp đồng mặc định là 1

                Ngaylap = TCom.GetServerDateOnly(); // Ngày mặc định là ngày hiện tại
                Tungay = TCom.GetServerDateOnly(); // Ngày bắt đầu hợp đồng mặc định là ngày hiện tại
                Denngay = TCom.GetServerDateOnly().AddMonths(3); // Ngày kết thúc hợp đồng mặc định là 1 tháng sau ngày hiện tại
                Trangthai = TrangThaiHD.dukien;
                
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
        [RuleRequiredField("Phòng không được để trống", DefaultContexts.Save)]
        public Phong Phong
        {
            get { return _Phong; }
            set 
            {
                bool isModified = SetPropertyValue<Phong>(nameof(Phong), ref _Phong, value);
                if(isModified && value != null && !IsLoading && !IsDeleted)
                {
                    Tiencoc= value.Tiencoc; // Khi chọn phòng thì tự động lấy tiền cọc của phòng đó
                }   
            }
        }

     
        [VisibleInListView(false), VisibleInDetailView(false)]
        public string SoHopdong
        {
            get
            {
                String so = SoHD.ToString();
                if (Phong != null)
                
                    so += "/" + Phong.Sophong;
                return so;
            }
        }

        private int _SoHD;
        [XafDisplayName("Số hợp đồng")]
        public int SoHD
        {
            get { return _SoHD; }
            set { SetPropertyValue<int>(nameof(SoHD), ref _SoHD, value); }
        }



        private DateOnly _Ngaylap;
        [XafDisplayName("Ngày đăng ký")]
        [ModelDefault("EditMask", "dd/MM/yyyy")]
        [ModelDefault("DisplayFormat", "{0:dd/MM/yyyy}")]
        public DateOnly Ngaylap
        {
            get { return _Ngaylap; }
            set { SetPropertyValue<DateOnly>(nameof(Ngaylap), ref _Ngaylap, value); }
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


        private decimal  _Tiencoc;
        [XafDisplayName("Tiền cọc")]
        [ModelDefault("DisplayFormat", "{0:### ### ###}")]     //tự động
        [ModelDefault("EditMask", "### ### ###")]
        public decimal Tiencoc
        {
            get { return _Tiencoc; }
            set { SetPropertyValue<decimal>(nameof(Tiencoc), ref _Tiencoc, value); }
        }


        
        [XafDisplayName("Doanh thu")]
        [ModelDefault("DisplayFormat", "{0:### ### ###}")]     //tự động
        [ModelDefault("EditMask", "### ### ###")]
        public decimal Doanhthu
        {
            get
            {
                decimal tong = HoaDons.Sum(x => x.TongTien);
                return tong;
            }
        }

       
        [XafDisplayName("Số người")]
        [ModelDefault("DisplayFormat", "{0:### ### ###}")] 
        public int Songuoi
        {
            get
            {
                int tong = TamTrus.Count;
                return tong;
            }
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
        public XPCollection<TamTru> TamTrus
        {
            get { return GetCollection<TamTru>(nameof(TamTrus)); }
        }
        


    }
}