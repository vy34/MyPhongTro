using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.Model.Core;
using DevExpress.ExpressApp.Model.DomainLogics;
using DevExpress.ExpressApp.Model.NodeGenerators;
using DevExpress.ExpressApp.Security.ClientServer;
using DevExpress.ExpressApp.Updating;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.BaseImpl.PermissionPolicy;
using DevExpress.Xpo;
using MyPhongTro.Module.BusinessObjects;
using System.ComponentModel;

namespace MyPhongTro.Module;

// For more typical usage scenarios, be sure to check out https://docs.devexpress.com/eXpressAppFramework/DevExpress.ExpressApp.ModuleBase.
public sealed class MyPhongTroModule : ModuleBase {
    public MyPhongTroModule() {
        //
        // MyPhongTroModule
        //
        AdditionalExportedTypes.Add(typeof(DevExpress.Persistent.BaseImpl.ModelDifference));
        AdditionalExportedTypes.Add(typeof(DevExpress.Persistent.BaseImpl.ModelDifferenceAspect));
        AdditionalExportedTypes.Add(typeof(DevExpress.Persistent.BaseImpl.BaseObject));
        AdditionalExportedTypes.Add(typeof(DevExpress.Persistent.BaseImpl.FileData));
        AdditionalExportedTypes.Add(typeof(DevExpress.Persistent.BaseImpl.FileAttachmentBase));
        AdditionalExportedTypes.Add(typeof(DevExpress.Persistent.BaseImpl.Event));
        AdditionalExportedTypes.Add(typeof(DevExpress.Persistent.BaseImpl.Resource));
        RequiredModuleTypes.Add(typeof(DevExpress.ExpressApp.SystemModule.SystemModule));
        RequiredModuleTypes.Add(typeof(DevExpress.ExpressApp.Security.SecurityModule));
        RequiredModuleTypes.Add(typeof(DevExpress.ExpressApp.Objects.BusinessClassLibraryCustomizationModule));
        RequiredModuleTypes.Add(typeof(DevExpress.ExpressApp.CloneObject.CloneObjectModule));
        RequiredModuleTypes.Add(typeof(DevExpress.ExpressApp.ConditionalAppearance.ConditionalAppearanceModule));
        RequiredModuleTypes.Add(typeof(DevExpress.ExpressApp.Dashboards.DashboardsModule));
        RequiredModuleTypes.Add(typeof(DevExpress.ExpressApp.Notifications.NotificationsModule));
        RequiredModuleTypes.Add(typeof(DevExpress.ExpressApp.Office.OfficeModule));
        RequiredModuleTypes.Add(typeof(DevExpress.ExpressApp.ReportsV2.ReportsModuleV2));
        RequiredModuleTypes.Add(typeof(DevExpress.ExpressApp.Scheduler.SchedulerModuleBase));
        RequiredModuleTypes.Add(typeof(DevExpress.ExpressApp.Validation.ValidationModule));
        RequiredModuleTypes.Add(typeof(DevExpress.ExpressApp.ViewVariantsModule.ViewVariantsModule));
    }
    public override IEnumerable<ModuleUpdater> GetModuleUpdaters(IObjectSpace objectSpace, Version versionFromDB) {
        ModuleUpdater updater = new DatabaseUpdate.Updater(objectSpace, versionFromDB);
        return new ModuleUpdater[] { updater };
    }

    public override void Setup(XafApplication application) {
        base.Setup(application);
        // Manage various aspects of the application UI and behavior at the module level.
        ApplicationHelper.Instance.Initialize(application);
        application.LoggedOn += Application_LoggedOn;
        application.LoggingOn += Application_LoggingOn;
        application.SetupComplete += Application_SetupComplete;
    }

    private void Application_SetupComplete(object sender, EventArgs e)
    {
        Application.ObjectSpaceCreated += Application_ObjectSpaceCreated;
    }

    private void Application_ObjectSpaceCreated(object sender, ObjectSpaceCreatedEventArgs e)
    {
        if (e.ObjectSpace is NonPersistentObjectSpace space)
        {
            IObjectSpace additionalObjectSpace = Application.CreateObjectSpace<ApplicationUser>(); // tạo một ObjectSpace thật có thể kết nối với DB
            space.AdditionalObjectSpaces.Add(additionalObjectSpace);                                //        

            space.ObjectGetting += ObjectSpace_ObjectGetting;                                       // để  gắn lại  ObjectSpace cho đối tượng non-persistent khi mở detailView
            e.ObjectSpace.Disposed += (s, args) =>
            {
                ((NonPersistentObjectSpace)s).ObjectGetting -= ObjectSpace_ObjectGetting;
                additionalObjectSpace.Dispose();
            };
        }
        if (e.ObjectSpace is NonPersistentObjectSpace nonPersistentObjectSpace)
        {
            nonPersistentObjectSpace.ObjectsGetting += NonPersistentObjectSpace_ObjectsGetting;  // gắn sự kiện ObjectsGetting để lấy dữ liệu cho các đối tượng non-persistent
        }
    }

    private void NonPersistentObjectSpace_ObjectsGetting(object sender, ObjectsGettingEventArgs e)
    {
        //NonPersistentObjectSpace obs = (NonPersistentObjectSpace)sender;
        //XPObjectSpace objectSpace = (XPObjectSpace)obs.AdditionalObjectSpaces[0];
        //if (e.ObjectType == typeof(RptDoanhthu))
        //{
        //    e.Objects = GetBaocao.GetDoanhthu(objectSpace);
        //}
        //else if (e.ObjectType == typeof(RptDoanhthuBanle))
        //{
        //    e.Objects = GetBaocao.GetDoanhthuBanle(objectSpace);
        //}
        //else if (e.ObjectType == typeof(ClsAudit))
        //{
        //    e.Objects = GetBaocao.GetAudit(objectSpace);
        //}
    }

    private void ObjectSpace_ObjectGetting(object sender, ObjectGettingEventArgs e)
    {
        if (e.SourceObject is IObjectSpaceLink)
        {
            e.TargetObject = e.SourceObject;
            ((IObjectSpaceLink)e.TargetObject).ObjectSpace = (IObjectSpace)sender;
        }
    }

    void Application_LoggingOn(object sender, LogonEventArgs e)
    {
        XafApplication app = (XafApplication)sender;

        IObjectSpaceProvider objectSpaceProvider = app.ObjectSpaceProviders[0];
        ((SecuredObjectSpaceProvider)objectSpaceProvider).AllowICommandChannelDoWithSecurityContext = true;
    }

    private void Application_LoggedOn(object sender, LogonEventArgs e)
    {
        XafApplication app = (XafApplication)sender;
        IObjectSpaceProvider objectSpaceProvider = app.ObjectSpaceProviders[0];
        ((SecuredObjectSpaceProvider)objectSpaceProvider).AllowICommandChannelDoWithSecurityContext = true;

        IObjectSpace os = app.CreateObjectSpace<ApplicationUser>();
        ApplicationUser CurentUser = os.GetObjectByKey<ApplicationUser>(SecuritySystem.CurrentUserId);
        if (CurentUser != null)
        {
            TCom.AddUserObject("userid", CurentUser.Oid);
        }
        if (TCom.IsAdmin(os))
        {
            //CheckSystemOptions(app);
        }

        os.Dispose();

        DateTime homnay = TCom.GetServerDateTime();
        DateTime ngay = new(homnay.Year, homnay.Month, 1);
        _ = DateTime.TryParse(ngay.ToShortDateString() + " 00:00:01", out DateTime tungay);
        _ = DateTime.TryParse($"{homnay.ToShortDateString()} 23:59:59", out DateTime denngay);
        TCom.AddUserObject("tungay", tungay);
        TCom.AddUserObject("denngay", denngay);
    }

    public override void CustomizeTypesInfo(ITypesInfo typesInfo) {
        base.CustomizeTypesInfo(typesInfo);
        CalculatedPersistentAliasHelper.CustomizeTypesInfo(typesInfo);
    }
}
