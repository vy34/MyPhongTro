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

            var apiRequest = new ApiRequest()
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
                var client = new RestClient("https://api.vietqr.io/v2/generate");
                var request = new RestRequest()
                    .AddHeader("x-client-id", "c7c09ac5-6206-4771-a549-505418a7a084")
                    .AddHeader("x-api-key", "56fdc9f2-c20a-4771-89ca-f5fc12114fd0")
                    .AddHeader("Accept", "application/json")
                    .AddJsonBody(apiRequest);

                var response = client.Post(request);

                if (!response.IsSuccessful)
                {
                    Application.ShowViewStrategy.ShowMessage(
                        $"Lỗi tạo QR: {response.StatusCode} - {response.StatusDescription}");
                    return;
                }

                var apiResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<ApiResponse>(response.Content);

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
