using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.XtraRichEdit;
using DevExpress.XtraRichEdit.API.Native;
using MyPhongTro.Module.BusinessObjects.Hopdong_thanhtoan;
using MyPhongTro.Module.BusinessObjects.Quanlykhanhthue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tmLib;

namespace MyPhongTro.Module.Controllers.Hopdong
{
    public class Inhopdong : ObjectViewController<DetailView, HopDong>
    {
        public Inhopdong()
        {
            SimpleAction inHopDongAction = new(this, "Tạo hợp đồng", "View")
            {
                TargetViewId = "HopDong_DetailView",
                ImageName = "print",
                ToolTip = "Tạo hợp đồng",
                ConfirmationMessage= "Bạn có chắc chắn muốn tạo hợp đồng này không?"
            };
            inHopDongAction.Execute += InHopDongAction_Execute;
        }
        private void InHopDongAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            if (View.CurrentObject is HopDong hopDong)
            {
                if (string.IsNullOrEmpty(hopDong.NoidungIn))
                {
                    TCom.CustomError("Hợp đồng không tồn tại");
                    return;
                }

                if (hopDong.TamTrus == null || !hopDong.TamTrus.Any())
                {
                    TCom.CustomError("Hợp đồng này chưa có khách thuê");
                    return;
                }

                // Lấy ListView con của Tạm trú khách thuê trong HopDong_DetailView
                var listPropertyEditor = View.FindItem("TamTrus") as DevExpress.ExpressApp.Editors.ListPropertyEditor; 
                if (listPropertyEditor?.ListView == null)
                {
                    TCom.CustomError("Không tìm thấy danh sách khách thuê");
                    return;
                }


                // Lấy các khách được chọn
                var selectedTamTrus = listPropertyEditor.ListView.SelectedObjects.Cast<TamTru>().ToList(); 

                if (selectedTamTrus.Count == 0)
                {
                    TCom.CustomError("Vui lòng chọn 1 khách thuê đại diện trước khi in hợp đồng");
                    return;
                }

      

                var khachDaiDien = selectedTamTrus.First().Khachthue; 
                var tienThue = hopDong.HopDongCTs?.FirstOrDefault(ct => ct.Khoanthu?.TenKhoanThu == "Tiền nhà") ?.Dongia.ToString();

                using RichEditDocumentServer wordProcessor = new(); // công cụ xử lý văn bản RTF/Docx của DevExpress.
                wordProcessor.RtfText = hopDong.NoidungIn; // load nội dung hợp đồng vào wordProcessor để xử lý.
                var replacements = new Dictionary<string, string>
                {
                 
                    { "{tinhthanh}", hopDong.Chutro?.TinhThanh?.Ten ?? "" },
                    { "{dd}", hopDong.Ngaylap.Day.ToString("00") },
                    { "{mm}", hopDong.Ngaylap.Month.ToString("00") },
                    { "{yyyy}", hopDong.Ngaylap.Year.ToString()},

                    { "{chu}", hopDong.Chutro?.Tenchutro ?? "" },
                    { "{cccdchu}", hopDong.Chutro?.CCCD ?? "" },
                    { "{dienthoaichu}", hopDong.Chutro?.Dienthoai ?? "" },
                    { "{hokhauchu}", hopDong.Chutro?.Diachi + ", " + hopDong.Chutro?.Xa?.Ten + ", " + hopDong.Chutro?.TinhThanh?.Ten },


                    { "{khach}", khachDaiDien?.HoTen ?? "" },
                    { "{cccdkhach}", khachDaiDien?.SoCCCD ?? "" },
                    { "{dienthoaikhach}", khachDaiDien?.Dienthoai ?? "" }, 
                    { "{hokhaukhach}", khachDaiDien?.Diachi ?? "" }, 
                    
                    { "{diachiphong}", hopDong.Chutro?.Diachi + ", " + hopDong.Chutro?.Xa?.Ten + ", " + hopDong.Chutro?.TinhThanh?.Ten },
                    { "{noithat}",hopDong.Phong?.Thietbiphongs != null ? string.Join(", ", hopDong.Phong.Thietbiphongs.Select(tb => tb.Thietbi)) : "" },

                    { "{namthue}", hopDong.Tungay.Year.ToString()},
                    { "{tungay}", hopDong.Tungay.ToString("dd/MM/yyyy") },
                    { "{denngay}", hopDong.Denngay.ToString("dd/MM/yyyy") },


                    { "{tiencoc}", hopDong.Tiencoc.ToString("N0")},
                    { "{chucoc}", ViCom.ChuyenSo(hopDong.Tiencoc.ToString())},
                    { "{tienthue}", hopDong.HopDongCTs?.FirstOrDefault(ct => ct.Khoanthu?.TenKhoanThu == "Tiền nhà") ?.Dongia.ToString("N0") ?? "" },
                    { "{chutienthue}", ViCom.ChuyenSo(tienThue) },

                    { "{hinhthuctt}", hopDong.Hinhthuc.ToString() },
                    { "{chutaikhoan}", hopDong.Chutro?.ChuTK ?? "" },
                    { "{sotaikhoan}", hopDong.Chutro?.SoTK ?? "" },
                    { "{nganhang}", hopDong.Chutro?.Nganhang.ToString() },

                    { "{songay}", ((hopDong.ThanhtoanDenngay.ToDateTime(TimeOnly.MinValue)- hopDong.ThanhtoanTungay.ToDateTime(TimeOnly.MinValue)).Days + 1).ToString() },
                    { "{ngay1}", hopDong.ThanhtoanTungay.Day.ToString("00") },
                    { "{ngay2}", hopDong.ThanhtoanDenngay.Day.ToString("00") },
                    { "{sodien}", hopDong.HopDongCTs?.FirstOrDefault(ct => ct.Khoanthu?.TenKhoanThu == "Tiền điện") ?.Chisodau.ToString() },


                };

                foreach (var kvp in replacements)
                {
                    wordProcessor.Document.ReplaceAll(kvp.Key, kvp.Value, SearchOptions.None);
                }

                hopDong.NoidungIn = wordProcessor.RtfText; //gán lại nội dung RTF đã được cập nhật
                ObjectSpace.CommitChanges(); //lưu lại thay đổi vào database
            }

        }
    }
}
