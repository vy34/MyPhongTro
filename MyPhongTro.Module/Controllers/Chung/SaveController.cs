using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.SystemModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyPhongTro.Module.Controllers.Chung
{
    public class SaveController :ViewController
    {
        protected override void OnActivated()
        {
            base.OnActivated();
            RecordsNavigationController recordsNavigationController = Frame.GetController<RecordsNavigationController>(); // Lấy controller RecordsNavigationController từ Frame
            if (recordsNavigationController != null)
            {
                bool active=false;
                recordsNavigationController.PreviousObjectAction.Active["an"] = active;   // tắt nút tới và lui trong detail view
                recordsNavigationController.NextObjectAction.Active["an"] = active;

            }

        }

        protected override void OnFrameAssigned()
        {
            base.OnFrameAssigned();
            ModificationsController modificationsController = Frame.GetController<ModificationsController>();
            if (modificationsController != null)
            {
                modificationsController.SaveAndCloseAction.Active["an"] = false; // tắt nút lưu và đóng trong detail view
                modificationsController.SaveAndNewAction.Active["an"] = false; // tắt nút lưu và tạo mới trong detail view
                modificationsController.SaveAction.ExecuteCompleted += SaveAction_ExecuteCompleted; // đăng ký sự kiện khi lưu thành công   
            }
        }

        private void SaveAction_ExecuteCompleted(object sender, ActionBaseEventArgs e)
        {
            Application.ShowViewStrategy.ShowMessage("Đã hoàn tất tác vụ", InformationType.Success, 3000, InformationPosition.Top);
            View.ObjectSpace.Refresh();
        }
    }
}
