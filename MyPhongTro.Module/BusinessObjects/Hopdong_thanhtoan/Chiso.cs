using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using MyPhongTro.Module.BusinessObjects.Cauhinhhethong;
using MyPhongTro.Module.BusinessObjects.Quanlyphongtro;
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
    public class Chiso(Session session) : BaseObject(session)
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://docs.devexpress.com/eXpressAppFramework/113146/business-model-design-orm/business-model-design-with-xpo/base-persistent-classes).
      // Use CodeRush to create XPO classes and properties with a few keystrokes.
      // https://docs.devexpress.com/CodeRushForRoslyn/118557

        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://docs.devexpress.com/eXpressAppFramework/112834/getting-started/in-depth-tutorial-winforms-webforms/business-model-design/initialize-a-property-after-creating-an-object-xpo?v=22.1).
            if (Session.IsNewObject(this))
            {
                Ngayghi = TCom.GetServerDateOnly(); // Ngày mặc định là ngày hiện tại

            }
        }

        private HopDong _Hopdong;
        [Association]
        [XafDisplayName("Hợp đồng")]
        public HopDong Hopdong
        {
            get { return _Hopdong; }
            set { SetPropertyValue<HopDong>(nameof(Hopdong), ref _Hopdong, value); }
        }

        private KhoanThu _KhoanThu;
        [Association]
        [XafDisplayName("Phòng")]
        public KhoanThu KhoanThu
        {
            get { return _KhoanThu; }
            set { SetPropertyValue<KhoanThu>(nameof(KhoanThu), ref _KhoanThu, value); }
        }


        private DateOnly _Ngayghi;
        [XafDisplayName("Ngày ghi")]
        [ModelDefault("EditMask", "dd/MM/yyyy")]
        [ModelDefault("DisplayFormat", "{0:dd/MM/yyyy}")]
        public DateOnly Ngayghi
        {
            get { return _Ngayghi; }
            set { SetPropertyValue<DateOnly>(nameof(Ngayghi), ref _Ngayghi, value); }
        }


        private int _chiso;
        [XafDisplayName("Chỉ số")]
        public int chiso
        {
            get { return _chiso; }
            set { SetPropertyValue<int>(nameof(chiso), ref _chiso, value); }
        }


        private string _Ghichu;
        [Size(255)]
        [XafDisplayName("Ghi chú")]
        public string Ghichu
        {
            get { return _Ghichu; }
            set { SetPropertyValue<string>(nameof(Ghichu), ref _Ghichu, value); }
        }




    }
}