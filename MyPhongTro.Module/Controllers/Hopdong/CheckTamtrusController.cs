using DevExpress.ExpressApp;
using DevExpress.Map.Native;
using MyPhongTro.Module.BusinessObjects.Hopdong_thanhtoan;
using MyPhongTro.Module.BusinessObjects.Quanlykhanhthue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyPhongTro.Module.Controllers.Hopdong
{
    public class CheckTamtrusController : ObjectViewController<DetailView, HopDong>
    {
        public CheckTamtrusController()
        {
            TargetObjectType = typeof(HopDong);
        }

        protected override void OnActivated()
        {
            base.OnActivated();
            ObjectSpace.Committing += ObjectSpace_Committing; // sảy ra khi nhấn Save trong DetailView.
        }

        private void ObjectSpace_Committing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (View.CurrentObject is HopDong hopDong)
            {
                if (hopDong == null) return;

                foreach (var tamtru in hopDong.TamTrus) 
                {
                    if (tamtru.Khachthue != null)
                    {
                        var hdKhac = ObjectSpace.GetObjects<HopDong>().FirstOrDefault(h => h.Oid != hopDong.Oid
                                          && h.TamTrus.Any(t => t.Khachthue == tamtru.Khachthue));
                        if (hdKhac != null)
                        {
                            throw new UserFriendlyException($"Khách '{tamtru.Khachthue.HoTen}' đã thuộc hợp đồng số {hdKhac.SoHopdong}");
                        }
                    }
                }

            }
        }

        protected override void OnDeactivated()
        {
            ObjectSpace.Committing -= ObjectSpace_Committing;
            base.OnDeactivated();
        }
    }
}
