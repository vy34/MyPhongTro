using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Xpo;
using System;
using System.Linq;
using tmLib;
using MyPhongTro.Module.BusinessObjects;

namespace MyPhongTro.Module
{
    public static class TCom
    {
        private const string EmptyString = "";

        public static List<UserObject> UserObjects = [];
        public static bool IsAdmin(IObjectSpace os)
        {
            bool bAdmin = false;
            //XafApplication app = ApplicationHelper.Instance.Application;
            //IObjectSpace os = app.CreateObjectSpace<ApplicationUser>();
            ApplicationUser CurentUser = os.GetObjectByKey<ApplicationUser>(SecuritySystem.CurrentUserId);
            if (CurentUser != null)
            {
                if (!string.IsNullOrEmpty(CurentUser.UserName))
                {
                    bAdmin = CurentUser.Roles.Any(r => r.IsAdministrative);
                }
            }
            //os.Dispose();
            return bAdmin;
        }

        public static void CustomInfo(string msg, bool bDone = false) // để hiển thị thông báo thành công kiểu như popup nhỏ
        {
            XafApplication app = ApplicationHelper.Instance.Application;
            if (bDone)
                app.ShowViewStrategy.ShowMessage("Đã hoàn thành tác vụ", InformationType.Success);
            else
                app.ShowViewStrategy.ShowMessage(msg, InformationType.Success);
        }
        public static void CustomError(string msg)      // hiển thị thông báo lỗi 
        {
            XafApplication app = ApplicationHelper.Instance.Application;
            app.ShowViewStrategy.ShowMessage(msg, InformationType.Error);
        }
        public static DateTime GetServerDateTime()  // lấy thời gian hiện tại của máy chủ
        {
            XafApplication app = ApplicationHelper.Instance.Application;
            using IObjectSpace objectSpace = app.CreateObjectSpace<ApplicationUser>();
            using Session session = ((XPObjectSpace)objectSpace).Session;
            string sql = "SELECT GETDATE();";
            DateTime timeCurrent = (DateTime)session.ExecuteScalar(sql);
            return timeCurrent;
        }

        public static DateOnly GetServerDateOnly()  // lấy ngày hiện tại của máy chủ
        {
            XafApplication app = ApplicationHelper.Instance.Application;
            using IObjectSpace objectSpace = app.CreateObjectSpace<ApplicationUser>();
            using Session session = ((XPObjectSpace)objectSpace).Session;
            string sql = "SELECT GETDATE();";
            DateTime timeCurrent = (DateTime)session.ExecuteScalar(sql);
            DateOnly ngay = DateOnly.FromDateTime(timeCurrent);
            return ngay;
        }

        public static string GetUserStringValue(string userKey) // trả về giá trị chuỗi của người dùng theo userKey (ptrouser)
        {
            string value = EmptyString;
            if (SecuritySystem.CurrentUserId == null) return value;

            foreach (UserObject item in UserObjects)
            {
                //if (item.UserId == SecuritySystem.CurrentUserId.ToString() && item.Userkey == userKey)
                if (string.Compare(item.UserId, SecuritySystem.CurrentUserId.ToString(), StringComparison.Ordinal) == 0 &&
                    string.Compare(item.Userkey, userKey, StringComparison.Ordinal) == 0)
                {
                    value = (string)item.Value;
                    break;
                }
            }
            return value;
        }

        public static bool GetUserBoolValue(string userKey)  // trả về bool của key cho currentUserId
        {
            bool value = false;
            if (SecuritySystem.CurrentUserId == null) return value;

            foreach (UserObject item in UserObjects)
            {
                //if (item.UserId == SecuritySystem.CurrentUserId.ToString() && item.Userkey == userKey)
                if (string.Compare(item.UserId, SecuritySystem.CurrentUserId.ToString(), StringComparison.Ordinal) == 0 &&
                    string.Compare(item.Userkey, userKey, StringComparison.Ordinal) == 0)
                {
                    value = (bool)item.Value;
                    break;
                }
            }
            return value;
        }
        public static DateTime GetUserDateTimeValue(string userKey)  // Trả về giá trị DateTime của key cho user hiện tại (
        {
            DateTime value = DateTime.MinValue;
            if (SecuritySystem.CurrentUserId == null) return value;

            foreach (UserObject item in UserObjects)
            {
                //if (item.UserId == SecuritySystem.CurrentUserId.ToString() && item.Userkey == userKey)
                if (string.Compare(item.UserId, SecuritySystem.CurrentUserId.ToString(), StringComparison.Ordinal) == 0 &&
                    string.Compare(item.Userkey, userKey, StringComparison.Ordinal) == 0)
                {
                    _ = DateTime.TryParse(item.Value.ToString(), out value);
                    break;
                }
            }
            return value;
        }

        public static int GetUserIntValue(string userKey)
        {
            int value = 0;
            if (SecuritySystem.CurrentUserId == null) return value;

            foreach (UserObject item in UserObjects)
            {
                //if (item.UserId == SecuritySystem.CurrentUserId.ToString() && item.Userkey == userKey)
                if (string.Compare(item.UserId, SecuritySystem.CurrentUserId.ToString(), StringComparison.Ordinal) == 0 &&
                    string.Compare(item.Userkey, userKey, StringComparison.Ordinal) == 0)
                {
                    value = tmLib.ViCom.CInt(item.Value);
                    break;
                }
            }
            return value;
        }
        public static Guid GetUserGuidValue(string userKey)
        {
            Guid value = Guid.Empty;
            if (SecuritySystem.CurrentUserId == null) return value;

            foreach (UserObject item in UserObjects)
            {
                //if (item.UserId == SecuritySystem.CurrentUserId.ToString() && item.Userkey == userKey)
                if (string.Compare(item.UserId, SecuritySystem.CurrentUserId.ToString(), StringComparison.Ordinal) == 0 &&
                    string.Compare(item.Userkey, userKey, StringComparison.Ordinal) == 0)
                {
                    string strvalue = item.Value.ToString();
                    _ = Guid.TryParse(strvalue, out value);
                    break;
                }
            }
            return value;
        }
        public static void AddUserObject(string userKey, object value) // lưu giá trị tạm thời vào danh sách UserObject với userkey để khi nào trong app cũng có thể lấy lại Oid của người dùng đó
        {
            bool Co = false;
            foreach (UserObject item in UserObjects)
            {
                //if (item.UserId == SecuritySystem.CurrentUserId.ToString() && item.Userkey == userKey)
                if (string.Compare(item.UserId, SecuritySystem.CurrentUserId.ToString(), StringComparison.Ordinal) == 0 &&
                    string.Compare(item.Userkey, userKey, StringComparison.Ordinal) == 0)
                {
                    Co = true;
                    item.Value = value;
                    return;
                }
            }
            if (!Co)
            {
                UserObject item = new()
                {
                    UserId = SecuritySystem.CurrentUserId.ToString(),
                    Userkey = userKey,
                    Value = value
                };
                UserObjects.Add(item);
            }
        }

        public static object GetUserObject(string userKey)
        {
            object value = null;
            foreach (UserObject item in UserObjects)
            {
                //if (item.UserId == SecuritySystem.CurrentUserId.ToString() && item.Userkey == userKey)
                if (string.Compare(item.UserId, SecuritySystem.CurrentUserId.ToString(), StringComparison.Ordinal) == 0 &&
                    string.Compare(item.Userkey, userKey, StringComparison.Ordinal) == 0)
                {
                    value = item.Value;
                    break;
                }
            }
            return value;
        }
    }
}
