//using DevExpress.Data.Filtering;
//using DevExpress.ExpressApp;
//using MyPhongTro.Module.BusinessObjects;
//using MyPhongTro.Module.BusinessObjects.Chutro;
//using MyPhongTro.Module.BusinessObjects.Hopdong_thanhtoan;
//using MyPhongTro.Module.BusinessObjects.Quanlykhanhthue;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Reflection;
//using System.Text;
//using System.Threading.Tasks;
//using static Azure.Core.HttpHeader;

//namespace MyPhongTro.Module.Controllers.FilterControler
//{
//    public class FilterKhachThueController : ObjectViewController<ListView, KhachThue>
//    {
//        protected override void OnActivated()
//        {
//            base.OnActivated();
//            ApplicationUser currentUser = ObjectSpace.GetObjectByKey<ApplicationUser>(SecuritySystem.CurrentUserId);// lấy user hiện tại
//                if (currentUser != null)
//                {
//                    View.CollectionSource.Criteria["ChutroFilter"] = CriteriaOperator.Parse("Chutro.Oid = ?", currentUser.Oid);  // lọc khách có Chutro.Oid = với Oid hiện tại
//                }
            
//        }
   
//    }
//}
