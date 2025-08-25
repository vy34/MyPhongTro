using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using MyPhongTro.Module.BusinessObjects.Hopdong_thanhtoan;
using System.Net.Http.Headers;
using System.Text.Json;


namespace MyPhongTro.Module.Controllers.Phieuthu
{
    public class TrichxuatanhController : ObjectViewController<DetailView, PhieuThu>
    {
        public TrichxuatanhController()
        {
            SimpleAction trichXuatAnhAction = new(this, "Trích xuất ảnh", "View")
            {
                TargetViewId = "PhieuThu_DetailView",
                ImageName = "trichxuatanh",
                ToolTip = "Trích xuất dữ liệu từ ảnh của phiếu thu"
            };
            trichXuatAnhAction.Execute += TrichXuatAnhAction_Execute;
        }

        private async void TrichXuatAnhAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            if (View.CurrentObject is not PhieuThu obj || obj.Anh == null || obj.Anh.Length == 0)
            {
                Application.ShowViewStrategy.ShowMessage("Không có ảnh để trích xuất!", InformationType.Warning);
                return;
            }

            try
            {
                // Gọi AI OCR API
                var resultText = await CallGeminiOCR(obj.Anh);  

                // Parse dữ liệu từ text
                var transactionCode = ExtractBetween(resultText, "Transaction code", "\n");
                var contentRaw = ExtractBetween(resultText, "Content", "\n");

                // Làm sạch nội dung (loại bỏ tiền tố, xuống dòng, khoảng trắng)
                string cleanContent = RemovePrefix(contentRaw)?
                    .Replace("\n", "")
                    .Replace("\r", "")
                    .Replace(" ", "")
                    .Trim();

                // Ghi vào phiếu thu
                obj.Noidung = cleanContent;
                obj.Ghichu = RemovePrefix(transactionCode);

                // Thử parse Oid từ cleanContent
                if (!string.IsNullOrEmpty(cleanContent))
                {
                    HoaDon hoaDon = null;

                    // Nếu cleanContent có dạng GUID (có hoặc không có dấu '-')
                    if (Guid.TryParse(cleanContent, out Guid parsedGuid))
                    {
                        hoaDon = ObjectSpace.GetObjects<HoaDon>().FirstOrDefault(h => h.Oid == parsedGuid);
                    }
                    else
                    {
                        // Nếu OCR trả về Oid dạng "N" (không có '-')
                        hoaDon = ObjectSpace.GetObjects<HoaDon>().FirstOrDefault(h => h.Oid.ToString("N").Equals(cleanContent, StringComparison.OrdinalIgnoreCase));
                    }

                    if (hoaDon != null)
                    {
                        obj.Hoadon = hoaDon;
                        obj.Noidung = hoaDon.Noidung;
                    }
                }

                ObjectSpace.CommitChanges();
                Application.ShowViewStrategy.ShowMessage("Đã trích xuất và cập nhật thành công!", InformationType.Success);
            }
            catch (Exception ex)
            {
                Application.ShowViewStrategy.ShowMessage("Lỗi khi trích xuất: " + ex.Message, InformationType.Error);
            }
        }

        // Hàm gọi API Gemini OCR
        private static async Task<string> CallGeminiOCR(byte[] imageData)
        {
            string apiKey = "AIzaSyAtdl-ezIOKLq7n2y_LOH6GvMhUEf-zpJ8";
            string url = $"https://generativelanguage.googleapis.com/v1beta/models/gemini-2.0-flash:generateContent?key={apiKey}";

            string base64Image = Convert.ToBase64String(imageData); // Chuyển đổi ảnh sang Base64 vì API yêu cầu dữ liệu hình ảnh ở định dạng Base64.

            var requestData = new
            {
                contents = new[]
                {
                    new 
                    { parts = new object[]
                        {
                            new { text = "Extract Transaction code, Amount, and Content from this bank transfer receipt image." },
                            new { inline_data = new { mime_type = "image/jpeg", data = base64Image } }
                        }
                    }
                }
            };

            using var client = new HttpClient(); // Tạo HttpClient để gửi yêu cầu HTTP 
            var content = new StringContent(JsonSerializer.Serialize(requestData)); // Chuyển đổi dữ liệu yêu cầu thành JSON
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json"); // Thiết lập tiêu đề Content-Type cho yêu cầu

            var response = await client.PostAsync(url, content); // wait nghĩa là chờ đợi phản hồi từ API sau khi gửi yêu cầu POST rồi mới chạy tiếp
            response.EnsureSuccessStatusCode();// Đảm bảo phản hồi thành công, nếu không sẽ ném ra ngoại lệ

            string json = await response.Content.ReadAsStringAsync(); // Đọc nội dung phản hồi dưới dạng chuỗi JSON
            using var doc = JsonDocument.Parse(json); // Phân tích cú pháp JSON để truy cập dữ liệu

            return doc.RootElement
                      .GetProperty("candidates")[0]
                      .GetProperty("content")
                      .GetProperty("parts")[0]
                      .GetProperty("text")
                      .GetString();
        }

        private static string ExtractBetween(string text, string start, string end)
        {
            var startIndex = text.IndexOf(start, StringComparison.OrdinalIgnoreCase); // Tìm vị trí bắt đầu của chuỗi cần trích xuất
            if (startIndex == -1) return string.Empty; // Nếu không tìm thấy chuỗi bắt đầu, trả về chuỗi rỗng
            startIndex += start.Length; // Di chuyển chỉ số bắt đầu đến sau chuỗi bắt đầu
            var endIndex = text.IndexOf(end, startIndex);
            if (endIndex == -1) endIndex = text.Length;// Nếu không tìm thấy chuỗi kết thúc, sử dụng độ dài của chuỗi gốc
            return text[startIndex..endIndex].Trim();// Trích xuất chuỗi giữa hai chỉ số và loại bỏ khoảng trắng ở đầu và cuối
        }


        private static string RemovePrefix(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return text;

            if (text.StartsWith(":**"))
                return text[3..].Trim();

            return text.Trim();
        }
    }
}
