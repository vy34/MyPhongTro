using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.SystemModule;
using MyPhongTro.Module.BusinessObjects.Hopdong_thanhtoan;
using System;
using System.Linq;
using DevExpress.Data.Filtering;

namespace MyPhongTro.Module.Controllers
{
    // Controller này sẽ được kích hoạt trên ListView của HoaDonCT
    public class HoaDonCTListViewController : ViewController<ListView>
    {
        private ListEditor listEditor;

        public HoaDonCTListViewController()
        {
            TargetObjectType = typeof(HoaDonCT);
        }

        protected override void OnActivated()
        {
            base.OnActivated();
            listEditor = View.Editor;
            if (listEditor != null)
            {
                // Lắng nghe sự kiện khi một đối tượng trong ListView được thêm mới hoặc thay đổi
                listEditor.DataSourceChanged += ListEditor_DataSourceChanged;
            }
        }

        private void ListEditor_DataSourceChanged(object sender, EventArgs e)
        {
            // Duyệt qua từng đối tượng trong danh sách để cập nhật chỉ số
            foreach (HoaDonCT hoaDonCT in View.CollectionSource.List)
            {
                if (hoaDonCT != null && hoaDonCT.Khoanthu != null)
                {
                    string tenKhoanThu = hoaDonCT.Khoanthu.TenKhoanThu;
                    if (tenKhoanThu.Contains("điện", StringComparison.OrdinalIgnoreCase) ||
                        tenKhoanThu.Contains("nước", StringComparison.OrdinalIgnoreCase))
                    {
                        UpdateChiSoDau(hoaDonCT);
                    }
                }
            }
            // Thông báo cho View rằng các đối tượng đã thay đổi để cập nhật UI
            View.ObjectSpace.SetModified(View.CurrentObject);
        }

        private void UpdateChiSoDau(HoaDonCT hoadonct)
        {
            var hoadonCha = hoadonct.Hoadon;
            if (hoadonCha?.Hopdong == null) return;

            // Tìm hóa đơn đã lưu gần nhất (trước ngày của hóa đơn hiện tại)
            var hoadonTruoc = hoadonCha.Hopdong.HoaDons
                                        .Where(hd => hd.Ngay < hoadonCha.Ngay && hd.Oid != hoadonCha.Oid)
                                        .OrderByDescending(hd => hd.Ngay)
                                        .FirstOrDefault();

            if (hoadonTruoc != null)
            {
                var hoaDonCTTruoc = hoadonTruoc.HoaDonCTs
                                     .FirstOrDefault(ct => ct.Khoanthu?.Oid == hoadonct.Khoanthu?.Oid);

                if (hoaDonCTTruoc != null)
                {
                    hoadonct.Chisodau = hoaDonCTTruoc.Chisocuoi;
                }
                else
                {
                    GetChiSoDauFromHopDong(hoadonct);
                }
            }
            else
            {
                GetChiSoDauFromHopDong(hoadonct);
            }
        }

        private void GetChiSoDauFromHopDong(HoaDonCT hoadonct)
        {
            var hopdong = hoadonct.Hoadon?.Hopdong;
            if (hopdong == null) return;

            var hopdongCT = hopdong.HopDongCTs
                            .FirstOrDefault(ct => ct.Khoanthu?.Oid == hoadonct.Khoanthu?.Oid);

            if (hopdongCT != null)
            {
                hoadonct.Chisodau = hopdongCT.Chisodau;
            }
        }

        protected override void OnDeactivated()
        {
            if (listEditor != null)
            {
                listEditor.DataSourceChanged -= ListEditor_DataSourceChanged;
                listEditor = null;
            }
            base.OnDeactivated();
        }
    }
}