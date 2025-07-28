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
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace MyPhongTro.Module.BusinessObjects.Hopdong_thanhtoan
{
    [DefaultClassOptions]
    [ImageName("phieuthu")]
    [System.ComponentModel.DisplayName("Phiếu thu")]
    [NavigationItem("Hợp đồng và hoá đơn")]
    //[DefaultProperty("DisplayMemberNameForLookupEditorsOfThisType")]
    [DefaultListViewOptions(MasterDetailMode.ListViewOnly, true, NewItemRowPosition.Top)]
    //[Persistent("DatabaseTableName")]
    // Specify more UI options using a declarative approach (https://docs.devexpress.com/eXpressAppFramework/112701/business-model-design-orm/data-annotations-in-data-model).
    public class PhieuThu(Session session) : BaseObject(session)
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://docs.devexpress.com/eXpressAppFramework/113146/business-model-design-orm/business-model-design-with-xpo/base-persistent-classes).
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://docs.devexpress.com/eXpressAppFramework/112834/getting-started/in-depth-tutorial-winforms-webforms/business-model-design/initialize-a-property-after-creating-an-object-xpo?v=22.1).
        if(Session.IsNewObject(this))
            {
                ChuTro chutro = Session.FindObject<ChuTro>(CriteriaOperator.Parse("Oid = ?", SecuritySystem.CurrentUserId));
                if (chutro != null)
                {
                    Chutro = chutro; // Tự động gán chủ trọ là người dùng hiện tại
                }
                
                string sql = "select max(So) as so from PhieuThu where Chutro = '" + SecuritySystem.CurrentUserId + "'";
                var ret = Session.ExecuteScalar(sql);
                int so = 1;
                if (ret != null) so = tmLib.ViCom.CInt(ret) + 1; // Lấy số phiếu thu lớn nhất của chủ trọ hiện tại
                So = so; // Số phiếu thu mặc định là 1

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

        private HoaDon _Hoadon;
        [Association]
        [XafDisplayName("Hóa đơn")]
        public HoaDon Hoadon
        {
            get { return _Hoadon; }
            set { SetPropertyValue<HoaDon>(nameof(Hoadon), ref _Hoadon, value); }
        }

        private TamTru _Tamtru;
        [Association]
        [XafDisplayName("Khách tạm trú")]
        public TamTru Tamtru
        {
            get { return _Tamtru; }
            set { SetPropertyValue<TamTru>(nameof(Tamtru), ref _Tamtru, value); }
        }


        private int _So;
        [XafDisplayName("Số phiếu")]
        public int So
        {
            get { return _So; }
            set { SetPropertyValue<int>(nameof(So), ref _So, value); }
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
            get
            {
                if (_Sotien > 0) 
                    return _Sotien;  // nếu số tiền dương (nhập tay) ưu tiên
                else
                {
                    decimal tien = Hoadon != null ? Hoadon.TongTien : 0;
                    return tien;
                }
                
            }       
            set { SetPropertyValue<decimal>(nameof(Sotien), ref _Sotien, value); }
        }

        private string _Noidung;
        [XafDisplayName("Nội dung")]
        [Size(255)]
        public string Noidung
        {
            get { return _Noidung; }
            set { SetPropertyValue<string>(nameof(Noidung), ref _Noidung, value); }
        }

        private string _Ghichu;
        [Size(255)]
        [XafDisplayName("Ghi chú")]
        public string Ghichu
        {
            get { return _Ghichu; }
            set { SetPropertyValue<string>(nameof(Ghichu), ref _Ghichu, value); }
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