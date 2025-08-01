using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.XtraSpreadsheet.Commands;
using MyPhongTro.Module.BusinessObjects.Quanlyphongtro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyPhongTro.Module.Controllers.Filter
{
    public class FilterForChutroController : ViewController
    {
        public FilterForChutroController() 
        {
            TargetViewId = "ThietBi_ListView;PhieuThu_ListView;Phong_ListView;HoaDon_ListView;KhoanThu_ListView;HopDong_ListView;Phiduytri_ListView;KhachThue_ListView;NhatKy_ListView;KhoanChi_ListView;" +
                "BaoTri_ListView;SuaChua_ListView;Anh_ListView;Thietbiphong_ListView;TamTru_ListView;HopDongCT_ListView;HoaDonCT_ListView;PhieuChi_ListView;";
                            
        }

        protected override void OnActivated()
        {
            base.OnActivated();
            SetFilter();
        }

        protected override void OnViewControlsCreated() // Được gọi khi các điều khiển của View đã được tạo
        {
            base.OnViewControlsCreated();
            this.ObjectSpace.Reloaded += ObjectSpace_Reloaded; // Đăng ký sự kiện Reloaded để cập nhật lại bộ lọc khi ObjectSpace được làm mới
        }

        private void ObjectSpace_Reloaded(object sender, EventArgs e)
        {
            SetFilter();
        }

        protected override void OnDeactivated()
        {
            this.ObjectSpace.Reloaded -= ObjectSpace_Reloaded; // gỡ bỏ sự kiện để trách rò rỉ bộ nhớ
            base.OnDeactivated();
            
        }

        private void SetFilter()
        {
            if (TCom.IsChutro())
            {
                CriteriaOperator criteria = CriteriaOperator.Parse("Chutro.Oid =?", SecuritySystem.CurrentUserId); // Lọc theo Chutro hiện tại cho các listview có chutro
                if (View.Id == "Thietbiphong_ListView")
                    criteria = CriteriaOperator.Parse("Phong.Chutro.Oid=?", SecuritySystem.CurrentUserId);
                if (View.Id == "BaoTri_ListView")
                    criteria = CriteriaOperator.Parse("Thietbiphong.Phong.Chutro.Oid=?", SecuritySystem.CurrentUserId);
                if (View.Id == "SuaChua_ListView")
                    criteria = CriteriaOperator.Parse("Phong.Chutro.Oid=?", SecuritySystem.CurrentUserId);
                if (View.Id == "Anh_ListView")
                    criteria = CriteriaOperator.Parse("Hoadon.Chutro.Oid=?", SecuritySystem.CurrentUserId);
                if (View.Id == "TamTru_ListView")
                    criteria = CriteriaOperator.Parse("Hopdong.Chutro.Oid=?", SecuritySystem.CurrentUserId);
                if (View.Id == "HopDongCT_ListView")
                    criteria = CriteriaOperator.Parse("Hopdong.Chutro.Oid=?", SecuritySystem.CurrentUserId);
                if (View.Id == "HoaDonCT_ListView")
                    criteria = CriteriaOperator.Parse("Hoadon.Chutro.Oid=?", SecuritySystem.CurrentUserId);
                if (View.Id == "PhieuThu_ListView")
                    criteria = CriteriaOperator.Parse("Hoadon.Chutro.Oid=?", SecuritySystem.CurrentUserId);

                ((ListView)View).CollectionSource.Criteria["loc"] = criteria; // Gán bộ lọc cho CollectionSource của ListView
            }
            else if (TCom.IsKhachthue())
            {   
                CriteriaOperator criteria = CriteriaOperator.Parse("Khachthue.Oid=?", SecuritySystem.CurrentUserId); // Lọc theo Khachthue hiện tại cho các listview có Khachthue     
                if (View.Id == "HopDong_ListView")
                    criteria = CriteriaOperator.Parse("Tamtru.Hopdong.Chutro.Oid=?", SecuritySystem.CurrentUserId);

                ((ListView)View).CollectionSource.Criteria["loc"] = criteria; // Gán bộ lọc cho CollectionSource của ListView
            }
            else
            {
                ((ListView)View).CollectionSource.Criteria.Remove("loc");
            }    
        }
    }
}
