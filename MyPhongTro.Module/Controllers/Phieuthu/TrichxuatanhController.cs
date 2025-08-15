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
                        hoaDon = ObjectSpace.GetObjects<HoaDon>()
                            .FirstOrDefault(h => h.Oid == parsedGuid);
                    }
                    else
                    {
                        // Nếu OCR trả về Oid dạng "N" (không có '-')
                        hoaDon = ObjectSpace.GetObjects<HoaDon>()
                            .FirstOrDefault(h =>
                                h.Oid.ToString("N")
                                  .Equals(cleanContent, StringComparison.OrdinalIgnoreCase));
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

            string base64Image = Convert.ToBase64String(imageData);

            var requestData = new
            {
                contents = new[]
                {
                    new {
                        parts = new object[]
                        {
                            new { text = "Extract Transaction code, Amount, and Content from this bank transfer receipt image." },
                            new {
                                inline_data = new {
                                    mime_type = "image/jpeg",
                                    data = base64Image
                                }
                            }
                        }
                    }
                }
            };

            using var client = new HttpClient();
            var content = new StringContent(JsonSerializer.Serialize(requestData));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var response = await client.PostAsync(url, content);
            response.EnsureSuccessStatusCode();

            string json = await response.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(json);

            return doc.RootElement
                      .GetProperty("candidates")[0]
                      .GetProperty("content")
                      .GetProperty("parts")[0]
                      .GetProperty("text")
                      .GetString();
        }

        private static string ExtractBetween(string text, string start, string end)
        {
            var startIndex = text.IndexOf(start, StringComparison.OrdinalIgnoreCase);
            if (startIndex == -1) return string.Empty;
            startIndex += start.Length;
            var endIndex = text.IndexOf(end, startIndex);
            if (endIndex == -1) endIndex = text.Length;
            return text[startIndex..endIndex].Trim();
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
