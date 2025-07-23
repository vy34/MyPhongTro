using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.SystemModule;
using MyPhongTro.Module.BusinessObjects;
using MyPhongTro.Module.BusinessObjects.Chutro;
using System;

namespace MyPhongTro.Module.Controllers.Chung.Profile
{
    public class ProfileController : WindowController  // cho phép tương tác tới toàn window( detailView,listview, navigation)
    {
        protected override void OnActivated()
        {
            base.OnActivated();

            var navigationController = Frame.GetController<ShowNavigationItemController>();  // Lấy controller ShowNavigationItemController từ Frame

            if (navigationController != null)
            {
                navigationController.CustomShowNavigationItem += NavigationController_CustomShowNavigationItem;  // đăng kí sự kiện customShownavigation để bạn có thể can thiệp khi người dùng click vào navi
            }
        }
        private void NavigationController_CustomShowNavigationItem(object sender, CustomShowNavigationItemEventArgs e)
        {

            if (e.ActionArguments.SelectedChoiceActionItem.Id == "Hosochutro") // khi chọn naavi có id là Hosochutro
            {
                IObjectSpace objectSpace = Application.CreateObjectSpace(typeof(ApplicationUser));
                ApplicationUser currentUser = objectSpace.GetObjectByKey<ApplicationUser>(SecuritySystem.CurrentUserId);

                if (currentUser is ChuTro currentChuTro) // kiểm tra currentUser có là chủ trọ k , phải thì gán cho biến currentChutro
                {
                    DetailView detailView = Application.CreateDetailView(objectSpace, "ChuTroView", true);
                    detailView.CurrentObject = currentChuTro;// gán đối tượng hiện tại (currentChuTro) cho detailView
                    detailView.ViewEditMode = ViewEditMode.View; // gán chế độ xem cho detailView

                    e.ActionArguments.ShowViewParameters.CreatedView = detailView; // tạo view mới từ detailView đã tạo
                    e.ActionArguments.ShowViewParameters.TargetWindow = TargetWindow.Current; // hiển thị trong cửa sổ hiện tại , k mở tab mới
                    e.Handled = true;
                }
            }
            else if(e.ActionArguments.SelectedChoiceActionItem.Id == "Hosokhachthue")
            {
                IObjectSpace objectSpace = Application.CreateObjectSpace(typeof(ApplicationUser));
                ApplicationUser currentUser = objectSpace.GetObjectByKey<ApplicationUser>(SecuritySystem.CurrentUserId);
                if (currentUser != null)
                {
                    DetailView detailView = Application.CreateDetailView(objectSpace, "KhachThueView", true);
                    detailView.CurrentObject = currentUser;
                    detailView.ViewEditMode = ViewEditMode.Edit;

                    e.ActionArguments.ShowViewParameters.CreatedView = detailView;
                    e.ActionArguments.ShowViewParameters.TargetWindow = TargetWindow.Current;
                    e.Handled = true;
                }
            }


        }

        protected override void OnDeactivated()
        {
            base.OnDeactivated();

            var navigationController = Frame.GetController<ShowNavigationItemController>();
            if (navigationController != null)
            {
                navigationController.CustomShowNavigationItem -= NavigationController_CustomShowNavigationItem;
            }
        }
    }
}
