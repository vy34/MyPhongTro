using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Editors;
using MyPhongTro.Module.BusinessObjects.Chutro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyPhongTro.Module.Controllers.Chung
{
    public class XaFilterController : ViewController<DetailView>
    {
        private PropertyEditor tinhThanhEditor; // Tìm kiếm id "TinhThanh" trong DetailView
        private PropertyEditor xaEditor; // Tìm kiếm id "Xa" trong DetailView

        public XaFilterController()
        {
            TargetViewId = "ChuTro_DetailView";
        }

        protected override void OnActivated()
        {
            base.OnActivated();
            tinhThanhEditor = View.FindItem("TinhThanh") as PropertyEditor;
            xaEditor = View.FindItem("Xa") as PropertyEditor;

            if (tinhThanhEditor != null)
            {
                tinhThanhEditor.ControlValueChanged += TinhThanhEditor_ControlValueChanged; // Đăng ký sự kiện khi giá trị của TinhThanh thay đổi
                ApplyXaFilter(tinhThanhEditor?.PropertyValue as DiaPhuong); // Áp dụng bộ lọc ban đầu cho Xa nếu TinhThanh đã có giá trị
            }
        }

        private void TinhThanhEditor_ControlValueChanged(object sender, EventArgs e)
        {
            DiaPhuong selectedTinhThanh = tinhThanhEditor?.PropertyValue as DiaPhuong; // Lấy giá trị đã chọn từ TinhThanh ép kiểu về DiaPhuong
            ApplyXaFilter(selectedTinhThanh);
            if (xaEditor?.MemberInfo.GetValue(View.CurrentObject) != null) // kiếm tra xem Xa đã có giá trọ chưa
            {
                xaEditor.MemberInfo.SetValue(View.CurrentObject, null); // Nếu đã có giá trị thì đặt lại giá trị của Xa về null
                xaEditor.WriteValue(); // Ghi giá trị mới vào Xa
            }
        }

        private void ApplyXaFilter(DiaPhuong tinhThanh)
        {
            // Lấy CollectionSource từ xaEditor và áp dụng bộ lọc dựa trên giá trị của TinhThanh
            if (xaEditor?.GetType() .GetProperty("CollectionSource") ?.GetValue(xaEditor) is CollectionSourceBase collectionSource) 
            {
                if (tinhThanh != null)
                {
                    collectionSource.Criteria["CapTrenFilter"] = CriteriaOperator.Parse("CapTren.Oid = ?", tinhThanh.Oid);
                }
                else
                {
                    collectionSource.Criteria["CapTrenFilter"] = CriteriaOperator.Parse("1=0");
                }
            }
        }

        protected override void OnDeactivated()
        {
            if (tinhThanhEditor != null)
            {
                tinhThanhEditor.ControlValueChanged -= TinhThanhEditor_ControlValueChanged;
            }
            base.OnDeactivated();
        }
    }
}
