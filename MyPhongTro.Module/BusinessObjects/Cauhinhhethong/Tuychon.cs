using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using MyPhongTro.Module.BusinessObjects.Chutro;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace MyPhongTro.Module.BusinessObjects.Cauhinhhethong
{
    [DefaultClassOptions]
    [ImageName("tuychon")]
    [System.ComponentModel.DisplayName("Tùy chọn sử dụng")]
    [NavigationItem("Cấu hình hệ thống")]
    //[DefaultProperty("DisplayMemberNameForLookupEditorsOfThisType")] //  hiển thị obkect UI tránh hiển thị Oid
    [DefaultListViewOptions(MasterDetailMode.ListViewOnly, true, NewItemRowPosition.Top)]
    [DeferredDeletion(false)]
    //[Persistent("DatabaseTableName")]  // đặt lại tên class trong database nếu không muốn hiển thị tên mặt định là TuyChon
    public class TuyChon(Session session) : BaseObject(session)
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://docs.devexpress.com/eXpressAppFramework/113146/business-model-design-orm/business-model-design-with-xpo/base-persistent-classes).
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://docs.devexpress.com/eXpressAppFramework/112834/getting-started/in-depth-tutorial-winforms-webforms/business-model-design/initialize-a-property-after-creating-an-object-xpo?v=22.1).
        }

        private ChuTro _Chutro;
        [Association]
        [VisibleInListView(false), VisibleInLookupListView(false), VisibleInDetailView(false), VisibleInDashboards(false)] // an het cac view
        public ChuTro ChuTro
        {
            get { return _Chutro; }
            set { SetPropertyValue<ChuTro>(nameof(ChuTro), ref _Chutro, value); }
        }

        private double _ThueVAT;
        [XafDisplayName(" Mức thuế VAT")]
        [ModelDefault("DisplayFormat","{0:0.#}%")] // định dạng hiển thị %
        [ModelDefault("EditMask", "0.#")]// định dạng khi người dùng nhập
        public double ThueVAT
        {
            get { return _ThueVAT; }
            set { SetPropertyValue<double>(nameof(ThueVAT), ref _ThueVAT, value); }
        }


        private double _ThueTNCN;
        [XafDisplayName(" Mức thuế TNCN")]
        [ModelDefault("DisplayFormat", "{0:0.#}%")]
        [ModelDefault("EditMask", "0.#")]
        public double ThueTNCN
        {
            get { return _ThueTNCN; }
            set { SetPropertyValue<double>(nameof(ThueTNCN), ref _ThueTNCN, value); }
        }


        private bool _PrintPreview;
        [XafDisplayName("Xem trước khi in")]
        public bool PrintPreview
        {
            get { return _PrintPreview; }
            set { SetPropertyValue<bool>(nameof(PrintPreview), ref _PrintPreview, value); }
        }







    }
}