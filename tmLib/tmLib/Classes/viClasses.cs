using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using System;
using System.Linq;

namespace tmLib
{
    public class UserEnvironment
    {
        public string UserId { get; set; }
        public string Userkey { get; set; }
        public string Uservalue { get; set; }
    }
    public class UserObject
    {
        public string UserId { get; set; }
        public string Userkey { get; set; }
        public object Value { get; set; }
    }

    [DomainComponent]
    public class ConfirmationWindowParameters
    {
        public ConfirmationWindowParameters() { }
        [ModelDefault("AllowEdit", "False")]
        [XafDisplayName("Xác nhận tác vụ")]
        public string ConfirmationMessage { get; set; }
    }

}
