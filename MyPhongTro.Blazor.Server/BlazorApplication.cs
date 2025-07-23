using DevExpress.ExpressApp;
using DevExpress.ExpressApp.ApplicationBuilder;
using DevExpress.ExpressApp.Blazor;
using DevExpress.ExpressApp.Security;
using DevExpress.ExpressApp.Security.ClientServer;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Utils;
using DevExpress.ExpressApp.Xpo;
using Microsoft.JSInterop;
using MyPhongTro.Blazor.Server.Services;

namespace MyPhongTro.Blazor.Server;

public class CustomCookieStorage : SettingsStorage
{
    readonly IServiceProvider serviceProvider; // lưu trữ IServiceProvider để lấy các dịch vụ khác
    readonly IHttpContextAccessor httpCont;   // lưu trữ IHttpContextAccessor để truy cập thông tin HTTP Context , phía server
    public CustomCookieStorage(IServiceProvider _serviceProvider)
    {
        serviceProvider = _serviceProvider;
        jsRuntime = (IJSRuntime)serviceProvider.GetService(typeof(IJSRuntime));
        httpCont = (IHttpContextAccessor)serviceProvider.GetService(typeof(IHttpContextAccessor));
    }
    IJSRuntime jsRuntime; // lưu trữ IJSRuntime để gọi các hàm JavaScript từ Blazor , phía client
    public override string LoadOption(string optionPath, string optionName)  //đọc cookie
    {
        if (httpCont != null)
        {
            var val = httpCont.HttpContext.Request.Cookies[optionName];
            return val;
        }
        return null;
    }
    public override void SaveOption(string optionPath, string optionName, string optionValue) // save cookie
    {
        Task.Run(async () => await jsRuntime.InvokeAsync<object>("blazorExtensions.WriteCookie", new object[] { optionName, optionValue, 30 }));
    }
}
public class MyPhongTroBlazorApplication : BlazorApplication {
    public MyPhongTroBlazorApplication() {
        ApplicationName = "Quản lý phòng trọ";
        CheckCompatibilityType = DevExpress.ExpressApp.CheckCompatibilityType.DatabaseSchema;
        DatabaseVersionMismatch += MyPhongTroBlazorApplication_DatabaseVersionMismatch;

        this.CreateCustomLogonParameterStore += MyPhongTroBlazorApplication_CreateCustomLogonParameterStore; // tùy chỉnh việc lưu trữ thông tin đăng nhập người dùng trong cookie
        this.LastLogonParametersReading += MyPhongTroBlazorApplication_LastLogonParametersReading; // đọc thông tin đăng nhập người dùng từ cookie
        this.LastLogonParametersWriting += MyPhongTroBlazorApplication_LastLogonParametersWriting; // ghi thông tin đăng nhập người dùng vào cookie
    }

    private void MyPhongTroBlazorApplication_LastLogonParametersWriting(object sender, LastLogonParametersWritingEventArgs e)
    {
        var st = e.LogonObject as AuthenticationStandardLogonParameters;
        e.SettingsStorage.SaveOption("", "ptrouser", st.UserName);
    }

    private void MyPhongTroBlazorApplication_LastLogonParametersReading(object sender, LastLogonParametersReadingEventArgs e)
    {
        try
        {
            e.Handled = true;
            string user = e.SettingsStorage.LoadOption("", "ptrouser");
            var st = e.LogonObject as AuthenticationStandardLogonParameters;
            st.UserName = user;
        }
        catch
        {
            //throw new UserFriendlyException(ex.Message);
        }
    }
    

    private void MyPhongTroBlazorApplication_CreateCustomLogonParameterStore(object sender, CreateCustomLogonParameterStoreEventArgs e)
    {
        e.Storage = new CustomCookieStorage(this.ServiceProvider);
        e.Handled = true;
    }

    protected override void OnSetupStarted() {
        base.OnSetupStarted();

        this.SetupComplete += (s, e) =>
        {
            if (this.Model.Options is DevExpress.ExpressApp.Blazor.SystemModule.IModelOptionsBlazor optionBlazor)
            {
                optionBlazor.RefreshViewOnTabFocus = true;  // tự động làm mới view khi chuyển tab
                optionBlazor.RestoreTabbedMdiLayout = false;  // không khôi phục bố cục tabbed MDI
            }
        };
#if DEBUG
        if (System.Diagnostics.Debugger.IsAttached && CheckCompatibilityType == CheckCompatibilityType.DatabaseSchema) {
            DatabaseUpdateMode = DatabaseUpdateMode.UpdateDatabaseAlways;
        }

#endif
    }
    private void MyPhongTroBlazorApplication_DatabaseVersionMismatch(object sender, DatabaseVersionMismatchEventArgs e) {
#if EASYTEST
        e.Updater.Update();
        e.Handled = true;
#else
        if(System.Diagnostics.Debugger.IsAttached) {
            e.Updater.Update();
            e.Handled = true;
        }
        else {
            string message = "The application cannot connect to the specified database, " +
                "because the database doesn't exist, its version is older " +
                "than that of the application or its schema does not match " +
                "the ORM data model structure. To avoid this error, use one " +
                "of the solutions from the https://www.devexpress.com/kb=T367835 KB Article.";

            if(e.CompatibilityError != null && e.CompatibilityError.Exception != null) {
                message += "\r\n\r\nInner exception: " + e.CompatibilityError.Exception.Message;
            }
            throw new InvalidOperationException(message);
        }
#endif
    }
}
