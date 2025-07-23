using System.ComponentModel;
using System.Text;
using DevExpress.ExpressApp.Security;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.BaseImpl.PermissionPolicy;
using DevExpress.Xpo;

namespace MyPhongTro.Module.BusinessObjects;

[MapInheritance(MapInheritanceType.ParentTable)]
[DefaultProperty(nameof(UserName))]
[CurrentUserDisplayImage(nameof(Photo))]
public class ApplicationUser : PermissionPolicyUser, ISecurityUserWithLoginInfo, ISecurityUserLockout
{
    private int accessFailedCount;
    private DateTime lockoutEnd;

    public ApplicationUser(Session session) : base(session) { }

    [Browsable(false)]
    public int AccessFailedCount
    {
        get { return accessFailedCount; }
        set { SetPropertyValue(nameof(AccessFailedCount), ref accessFailedCount, value); }
    }

    [Browsable(false)]
    public DateTime LockoutEnd
    {
        get { return lockoutEnd; }
        set { SetPropertyValue(nameof(LockoutEnd), ref lockoutEnd, value); }
    }

    [Browsable(false)]
    [DevExpress.Xpo.Aggregated, Association("User-LoginInfo")]
    public XPCollection<ApplicationUserLoginInfo> LoginInfo
    {
        get { return GetCollection<ApplicationUserLoginInfo>(nameof(LoginInfo)); }
    }

    IEnumerable<ISecurityUserLoginInfo> IOAuthSecurityUser.UserLogins => LoginInfo.OfType<ISecurityUserLoginInfo>();

    ISecurityUserLoginInfo ISecurityUserWithLoginInfo.CreateUserLoginInfo(string loginProviderName, string providerUserKey)
    {
        ApplicationUserLoginInfo result = new ApplicationUserLoginInfo(Session);
        result.LoginProviderName = loginProviderName;
        result.ProviderUserKey = providerUserKey;
        result.User = this;
        return result;
    }


    //modify

    private MediaDataObject photo;
    [ImageEditor(
        ListViewImageEditorMode = ImageEditorMode.PopupPictureEdit,
        DetailViewImageEditorMode = ImageEditorMode.PictureEdit,
        DetailViewImageEditorFixedHeight = 240,
        DetailViewImageEditorFixedWidth = 300,
        ListViewImageEditorCustomHeight = 40)]
    public MediaDataObject Photo
    {
        get { return photo; }
        set { SetPropertyValue(nameof(Photo), ref photo, value); }
    }

    

}
