using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using MyPhongTro.Module.BusinessObjects.Cauhinhhethong;
using MyPhongTro.Module.BusinessObjects.Hopdong_thanhtoan;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyPhongTro.Module.Controllers.Hopdong
{
    public class HopdongTiencocController : ObjectViewController<DetailView,HopDong> // chỉ áp dụng cho detailview cụ thể (HopDong)
    {
        public HopdongTiencocController()
        {
            SimpleAction hdTiencoc = new(this, "Thanh toán tiền cọc", "View")
            {
                TargetViewId = "HopDong_DetailView",
                ImageName = "tiencoc",
                ToolTip = "Thanh toán tiền cọc", // hiển thị khi rê chuột vào nút
                ConfirmationMessage = "Chắc chắn lập hoá đơn tiền cọc cho khách?"
            };
            hdTiencoc.Execute += HdTiencoc_Execute; // sự kiện khi nhấn nút


        }

        private void HdTiencoc_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            if(View.CurrentObject is HopDong hopdong)  // kiểm tra đối tượng hiện tại có phải là HopDong không
            {
                if(hopdong.Tiencoc == 0)
                {
                    TCom.CustomError("Không có tiền cọc");
                }else
                {
                    string ten = "Thu tiền cọc";
                    HoaDon hoadon = ObjectSpace.FindObject<HoaDon>(CriteriaOperator.Parse("Hopdong.Oid = ? && Noidung=?", hopdong.Oid,ten));
                    if (hoadon != null)
                    {
                        TCom.CustomError("Đã có hoá đơn thu tiền cọc");
                        return;
                    }
                    hoadon = ObjectSpace.CreateObject<HoaDon>(); // tạo mới hoá đơn
                    hoadon.IsHoadonTiencoc = true; // đánh dấu là hoá đơn tiền cọc
                    hoadon.Hopdong =hopdong; // gán hợp đồng cho hoá đơn
                    hoadon.Ngay = hopdong.Ngaylap;
                    hoadon.Noidung = ten;
                    hoadon.Save();

                    HoaDonCT hoaDonCT = ObjectSpace.CreateObject<HoaDonCT>();
                    hoaDonCT.Hoadon = hoadon; // gán hoá đơn cho chi tiết hoá đơn

                    KhoanThu khoanThu = ObjectSpace.FindObject<KhoanThu>(CriteriaOperator.Parse("Chutro.Oid=? && TenKhoanThu=?", hopdong.Chutro.Oid,ten));
                    if(khoanThu == null)
                    {
                        khoanThu = ObjectSpace.CreateObject<KhoanThu>();
                        khoanThu.Chutro = hopdong.Chutro;
                        khoanThu.TenKhoanThu = ten;
                        khoanThu.Save();

                    }
                    // gán các thuộc tính cho chi tiết hoá đơn
                    hoaDonCT.Khoanthu = khoanThu;
                    hoaDonCT.Soluong = 1;
                    hoaDonCT.DonGia = hopdong.Tiencoc;
                    hoaDonCT.Ghichu = ten;
                    hoaDonCT.Save();

                    hoadon.HoaDonCTs.Add(hoaDonCT);
                    hoadon.Save();

                    hopdong.HoaDons.Add(hoadon);
                    hopdong.Save();

                    ObjectSpace.CommitChanges();
                    TCom.CustomInfo("", true);

                }    
            }    
        }
    }
}
