//using DevExpress.Data.Filtering;
//using DevExpress.ExpressApp;
//using MyPhongTro.Module.BusinessObjects;
//using MyPhongTro.Module.BusinessObjects.Chutro;
//using MyPhongTro.Module.BusinessObjects.Quanlykhanhthue;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace MyPhongTro.Module.Controllers.FilterControler
//{
//    public class SetChutroForKhachController : ObjectViewController<DetailView, KhachThue>
//    {
//        protected override void OnActivated()
//        {
//            base.OnActivated();
//            // Kiểm tra xem đối tượng hiện tại có phải là một đối tượng mới hay không
//            if (ObjectSpace.IsNewObject(View.CurrentObject))
//            {
//                var khach = View.CurrentObject as KhachThue; //view.currentObject là đối tượng KhachThue hiện tại
             
//                var currentUser = ObjectSpace.GetObjectByKey<ChuTro>(SecuritySystem.CurrentUserId); // lấy Oid của người dùng hiện tại là chủ trọ

//                if (currentUser != null)
//                {
//                    khach.Chutro = currentUser; // gắn khách thuê với chủ trọ hiện tại
//                }
//            }
//        }
//    }

//}
