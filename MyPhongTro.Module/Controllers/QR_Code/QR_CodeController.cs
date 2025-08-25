using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.Persistent.Base;
using MyPhongTro.Module.BusinessObjects.Hopdong_thanhtoan;
using RestSharp;


namespace MyPhongTro.Module.Controllers.QR_Code
{
    public class QR_CodeController : ObjectViewController<DetailView, HoaDon>
    {
        public QR_CodeController()
        {
            var qrCodeAction = new SimpleAction(this, "QR Code", PredefinedCategory.View)
            {
                TargetViewId = "HoaDon_DetailView",
                ImageName = "QRCode",
                ToolTip = "Tạo mã QR cho hóa đơn"
            };
            qrCodeAction.Execute += QrCodeAction_Execute;

        }

        private void QrCodeAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            if (View.CurrentObject is not HoaDon hoaDon) 
                return;
             
            var apiRequest = new ApiRequest() // tạo thông tin yêu cầu API
            {
                AccountNo = hoaDon.Chutro?.SoTK,
                AccountName = hoaDon.Chutro?.ChuTK,
                AcqId =  hoaDon.Chutro.Nganhang?.Bin,
                Amount = hoaDon.TongTien,
                AddInfo = hoaDon.Oid.ToString(),
                Format = "text",
                Template = "compact2"
            };

            try
            {
                var client = new RestClient("https://api.vietqr.io/v2/generate"); //chỉ định nơi gửi yêu cầu API, đây là URL của dịch vụ tạo mã QR.
                var request = new RestRequest()  // là một đối tượng chứa thông tin về yêu cầu API, bao gồm phương thức HTTP, tiêu đề và nội dung yêu cầu.
                    .AddHeader("x-client-id", "c7c09ac5-6206-4771-a549-505418a7a084") // dùng để xác thực ứng dụng với API.
                    .AddHeader("x-api-key", "56fdc9f2-c20a-4771-89ca-f5fc12114fd0")
                    .AddHeader("Accept", "application/json") // yêu cầu API trả về dữ liệu ở định dạng JSON.
                    .AddJsonBody(apiRequest);  // thêm thông tin yêu cầu vào request dưới dạng JSON.

                var response = client.Post(request); // gửi yêu cầu POST đến API với thông tin đã chuẩn bị.(request) và nhận phản hồi (response).

                if (!response.IsSuccessful)
                {
                    Application.ShowViewStrategy.ShowMessage(
                        $"Lỗi tạo QR: {response.StatusCode} - {response.StatusDescription}");
                    return;
                }

                // Nếu phản hồi thành công, giải mã nội dung JSON trả về để lấy dữ liệu mã QR.

                var apiResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<ApiResponse>(response.Content); // reponse.Content chứa dữ liệu JSON ( nội dung ) trả về từ API, được giải mã thành đối tượng ApiResponse.

                // xử lý dữ liệu mã QR trả về từ API.

                if (!string.IsNullOrEmpty(apiResponse?.Data?.QrDataURL))
                {
                    hoaDon.QRCodeImage = Convert.FromBase64String(apiResponse.Data.QrDataURL.Replace("data:image/png;base64,", ""));


                    ObjectSpace.CommitChanges();
                    View.ObjectSpace.Refresh();
                    View.Refresh();

                    Application.ShowViewStrategy.ShowMessage("Tạo mã QR thành công!");
                }
                else
                {
                    Application.ShowViewStrategy.ShowMessage("API không trả về dữ liệu mã QR hợp lệ.");
                }
            }
            catch (Exception ex)
            {
                Application.ShowViewStrategy.ShowMessage("Lỗi tạo QR: " + ex.Message);
            }
        }

        public class ApiRequest
        {
            public string AccountNo { get; set; }
            public string AccountName { get; set; }
            public string AcqId { get; set; }
            public decimal Amount { get; set; }
            public string AddInfo { get; set; }
            public string Format { get; set; }
            public string Template { get; set; }
        }

        public class ApiResponse
        {
            public ApiResponseData Data { get; set; }
        }

        public class ApiResponseData
        {
            public string QrDataURL { get; set; }
        }
    }
}
