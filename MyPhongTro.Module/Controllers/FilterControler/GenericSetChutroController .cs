//using DevExpress.ExpressApp;
//using MyPhongTro.Module.BusinessObjects.Chutro;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace MyPhongTro.Module.Controllers.FilterControler
//{
//    public class GenericSetChutroController : ViewController<DetailView>
//    {
//        protected override void OnActivated()
//        {
//            base.OnActivated();

//            if (ObjectSpace.IsNewObject(View.CurrentObject) && !TCom.IsAdmin(ObjectSpace)) // kiểm tra nếu đối tượng mới và người dùng không phải là quản trị viên
//            {
//                var currentUser = ObjectSpace.GetObjectByKey<ChuTro>(SecuritySystem.CurrentUserId); // lấy người dùng hiện tại dưới dạng ChuTro
//                if (currentUser != null)
//                {
//                    var currentObject = View.CurrentObject; // lấy đối tượng hiện tại từ View vd: Hóa đơn , hợp đồng, phòng trọ, v.v.
//                    var chutroProp = currentObject.GetType().GetProperty("Chutro"); // kiểm tra xem đối tượng hiện tại có thuộc tính Chutro hay không

//                    if (chutroProp != null && chutroProp.CanWrite) // kiểm tra xem thuộc tính Chutro có tồn tại và có thể ghi được không
//                    {
//                        chutroProp.SetValue(currentObject, currentUser); // gán giá trị Chutro cho đối tượng hiện tại là người dùng hiện tại
//                    }
//                }
//            }
//        }
//    }
//}
using DevExpress.ExpressApp;
using MyPhongTro.Module.BusinessObjects.Chutro;
using System;
using System.Linq;
using System.Reflection;

namespace MyPhongTro.Module.Controllers.FilterControler
{
    public class GenericSetChutroController : ViewController<DetailView>
    {
        protected override void OnActivated()
        {
            base.OnActivated();

            if (!ObjectSpace.IsNewObject(View.CurrentObject) || TCom.IsAdmin(ObjectSpace)) return;

            var currentUser = ObjectSpace.GetObjectByKey<ChuTro>(SecuritySystem.CurrentUserId);
            if (currentUser == null) return;

            var currentObject = View.CurrentObject;
            var objectType = currentObject.GetType();

            // 1. Gán trực tiếp nếu có thuộc tính Chutro
            PropertyInfo directChutroProp = objectType.GetProperty("Chutro");
            if (directChutroProp != null && directChutroProp.CanWrite)
            {
                directChutroProp.SetValue(currentObject, currentUser);
                return;
            }

            // 2. Gán bắc cầu nếu có navigation property có Chutro
            foreach (PropertyInfo prop in objectType.GetProperties())
            {
                if (typeof(IXafEntityObject).IsAssignableFrom(prop.PropertyType))
                {
                    var relatedObj = prop.GetValue(currentObject);
                    if (relatedObj != null)
                    {
                        PropertyInfo relatedChutroProp = prop.PropertyType.GetProperty("Chutro");
                        if (relatedChutroProp != null && relatedChutroProp.CanWrite)
                        {
                            relatedChutroProp.SetValue(relatedObj, currentUser);
                            return;
                        }
                    }
                }
            }
        }
    }
}
