using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using MyPhongTro.Module.BusinessObjects.Chutro;
using MyPhongTro.Module.BusinessObjects.Hotro;


namespace MyPhongTro.Module.Controllers.Hotro
{
    public class ImportController : ViewController
    {
        public ImportController()
        {
            TargetViewId = "DiaPhuong_ListView";
            SimpleAction Diaphuong = new(this, "Diaphuong", "View")
            {
                Caption = "Cập nhật",
                ConfirmationMessage = "Chắc chắn cập nhật ?",
                TargetViewId = "DiaPhuong_ListView"
            };
            Diaphuong.Execute += Diaphuong_Execute;
        }

        private void Diaphuong_Execute(object sender, SimpleActionExecuteEventArgs e) // Xử lý sự kiện khi người dùng nhấn nút "Cập nhật"
        {
            IObjectSpace objectSpace = Application.CreateObjectSpace<ClsImport>(); // Tạo một ObjectSpace mới cho ClsImport
            ClsImport clsImport = objectSpace.CreateObject<ClsImport>(); // Tạo một đối tượng ClsImport mới
            DetailView view = Application.CreateDetailView(objectSpace, clsImport);
            view.Caption = "Cập nhật địa phương";
            e.ShowViewParameters.CreatedView = view;
            e.ShowViewParameters.Context = TemplateContext.View;
            e.ShowViewParameters.TargetWindow = TargetWindow.Default;
        }

    }

    public class ImportAction : ViewController
    {
        private  ClsImport imp;
        private static readonly string[] separator = ["\n"];
        public ImportAction()
        {
            TargetViewId = "ClsImport_DetailView";
            SimpleAction CheckData = new(this, "CheckData", "View")
            {
                Caption = "Kiểm tra",
                TargetViewId = "ClsImport_DetailView",
                ToolTip = "Kiểm tra dữ liệu cần cập nhật"
            };

            CheckData.Execute += CheckData_Execute;

            SimpleAction CapnhatDulieu = new(this, "CapnhatDulieu", "View")
            {
                Caption = "Lưu dữ liệu",
                TargetViewId = "ClsImport_DetailView",
                ConfirmationMessage = "Chắc chắn lưu dữ liệu?"
            };
            CapnhatDulieu.Execute += CapnhatDulieu_Execute;
        }
        private void CapnhatDulieu_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            CapnhatDiaphuong();
        }

        private void CheckData_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            CheckDiaPhuong();
        }

       
        private void CheckDiaPhuong()
        {
            imp = View.CurrentObject as ClsImport;
            string dulieu = imp.Noidung;
            if (string.IsNullOrEmpty(dulieu))
                return;

            imp.ImportDiaphuongs.Clear();
            string[] lines = dulieu.Split(separator, StringSplitOptions.None);
            int linecount = lines.Length;
            if (linecount > 0)
            {
                for (int i = 0; i < linecount; i++)
                {
                    string line = lines[i];
                    if (!string.IsNullOrEmpty(line))
                    {
                        string[] datas = line.Split("\t".ToCharArray(), StringSplitOptions.None);
                        ClsImportDiaphuong diaphuong = new()
                        {
                            Oid = Guid.NewGuid(),
                            Ma = GetStringInArray(datas, 0),
                            Ten = GetStringInArray(datas, 1),
                            Captren = GetStringInArray(datas, 2)
                        };
                        imp.ImportDiaphuongs.Add(diaphuong);
                    }
                }
            }
            imp.Noidung = "";
        }
        

        private void CapnhatDiaphuong()
        {
            try
            {
                IObjectSpace objectSpace = Application.CreateObjectSpace<DiaPhuong>();
                var allData = imp.ImportDiaphuongs.ToList();

                // Bước 1: Tạo hoặc cập nhật tất cả các đối tượng DiaPhuong
                // Vòng lặp này chỉ thiết lập Ma và Ten
                foreach (var item in allData)
                {
                    DiaPhuong diaphuong = objectSpace.FindObject<DiaPhuong>(CriteriaOperator.Parse("Ma = ?", item.Ma)); // tìm kiếm đối tượng DiaPhuong theo Ma
                    diaphuong ??= objectSpace.CreateObject<DiaPhuong>(); //
                    diaphuong.Ma = item.Ma;
                    diaphuong.Ten = item.Ten;
                }

                // Commit tạm thời để các đối tượng DiaPhuong đã có trong ObjectSpace
                objectSpace.CommitChanges();

                // Bước 2: Dùng vòng lặp khác để gán mối quan hệ CapTren
                // Vòng lặp này sẽ tìm cấp trên và gán
                foreach (var item in allData)
                {
                    // Tìm đối tượng cấp dưới đã được tạo ở Bước 1
                    DiaPhuong diaphuongCon = objectSpace.FindObject<DiaPhuong>(CriteriaOperator.Parse("Ma = ?", item.Ma)); // tìm kiếm đối tượng DiaPhuong theo Ma

                    if (!string.IsNullOrEmpty(item.Captren) && diaphuongCon != null) 
                    {
                        // Tìm đối tượng cấp trên dựa vào mã của nó
                        DiaPhuong diaphuongCapTren = objectSpace.FindObject<DiaPhuong>(CriteriaOperator.Parse("Ma = ?", item.Captren));
                        if (diaphuongCapTren != null)
                        {
                            diaphuongCon.CapTren = diaphuongCapTren;
                        }
                    }
                    else if (diaphuongCon != null)
                    {
                        // Nếu không có cấp trên, đảm bảo giá trị là null
                        diaphuongCon.CapTren = null;
                    }
                }

                objectSpace.CommitChanges(); // Lưu lại các thay đổi sau khi gán CapTren
                Application.ShowViewStrategy.ShowMessage("Cập nhật dữ liệu địa phương thành công!", InformationType.Success, 5000, InformationPosition.Bottom);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException("Có lỗi xảy ra trong quá trình lưu dữ liệu: " + ex.Message);
            }
        }

        private static string GetStringInArray(string[] array, int index)
        {
            if (index < array.Length)
                return array[index].Trim();
            else
                return string.Empty;
        }
    }

}
