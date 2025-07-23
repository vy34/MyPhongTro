using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Model;
using DevExpress.Xpo;
using System;
using System.Linq;

namespace tmLib
{
    [NonPersistent]
    [DeferredDeletion(false)]
    [Appearance("DisableDelete", Criteria = "Locked", AppearanceItemType = "Action", TargetItems = "Delete", Visibility = ViewItemVisibility.Hide)]

    public class ViXPObject(Session session) : XPObject(session)
    {
        private DateTime _Giocn;
        [XafDisplayName("Giờ cập nhật")]
        [ModelDefault("EditMask", "dd/MM/yyyy HH:mm")]
        [ModelDefault("DisplayFormat", "{0:dd/MM/yyyy HH:mm}")]
        public DateTime Giocn
        {
            get { return _Giocn; }
            set { SetPropertyValue<DateTime>(nameof(Giocn), ref _Giocn, value); }
        }

        private bool _Locked;
        [XafDisplayName("Khóa")]
        public bool Locked
        {
            get { return _Locked; }
            set { SetPropertyValue<bool>(nameof(Locked), ref _Locked, value); }
        }
    }
}
