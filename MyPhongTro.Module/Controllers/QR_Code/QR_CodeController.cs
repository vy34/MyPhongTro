using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using MyPhongTro.Module.BusinessObjects.Hopdong_thanhtoan;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyPhongTro.Module.Controllers.QR_Code
{
    public class QR_CodeController : ObjectViewController<DetailView,HoaDon> // chỉ áp dụng cho detailview cụ thể
    {
        public QR_CodeController()
        {
            // Tạo nút QR Code
            SimpleAction qrCodeAction = new(this, "QR Code", "View")
            {
                TargetViewId = "HoaDon_DetailView", // ID của view chi tiết hóa đơn
                ImageName = "QRCode",
                ToolTip = "Tạo mã QR cho hóa đơn"
            };
            qrCodeAction.Execute += QrCodeAction_Execute; // đăng ký sự kiện khi nhấn nút
        }

        private void QrCodeAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            
        }
    }
}
