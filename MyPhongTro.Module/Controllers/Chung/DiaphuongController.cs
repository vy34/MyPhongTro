using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Blazor.Editors;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.SystemModule;
using MyPhongTro.Module.BusinessObjects.Chutro;
using System;


namespace MyPhongTro.Module.Controllers.Chung
{
    public class DiaphuongController : ViewController<DetailView>
    {
        public DiaphuongController()
        {
            TargetViewId = "ChuTro_DetailView";
        }

        protected override void OnActivated()
        {
            base.OnActivated();
            if (View.FindItem("TinhThanh") is PropertyEditor tinhThanhEditor)  //tìm kiếm id "TinhThanh" trong DetailView
            {
                if (tinhThanhEditor?.GetType()
                                          .GetProperty("CollectionSource") // lấy CollectionSource từ PropertyEditor để áp dụng bộ lọc
                                          ?.GetValue(tinhThanhEditor) is CollectionSourceBase collectionSource) // kiểm tra xem có CollectionSource không
                {
                    collectionSource.Criteria["TinhThanhFilter"] = CriteriaOperator.Parse("CapTren is null"); // áp dụng bộ lọc để chỉ hiển thị các địa phương cấp tỉnh (không có cấp trên)
                }
            }
        }



        protected override void OnDeactivated()
        {
            if (View.FindItem("TinhThanh") is PropertyEditor tinhThanhEditor)
            {
                if (tinhThanhEditor?.GetType()
                                          .GetProperty("CollectionSource")
                                          ?.GetValue(tinhThanhEditor) is CollectionSourceBase collectionSource)
                {
                    collectionSource.Criteria.Remove("TinhThanhFilter");
                }
            }
            base.OnDeactivated();
        }

        
    }
}