//using DevExpress.Data.Filtering;
//using DevExpress.ExpressApp;
//using DevExpress.ExpressApp.Security;
//using DevExpress.Xpo;
//using MyPhongTro.Module.BusinessObjects.Chutro;
//using System;
//using System.Reflection;

//namespace MyPhongTro.Module.Controllers.FilterControler
//{
//    public class GenericFilterChutroController : ViewController<ListView>
//    {
//        protected override void OnActivated()
//        {
//            base.OnActivated();

//            if (!TCom.IsAdmin(ObjectSpace)) // Nếu không phải Admin
//            {
//                object user = SecuritySystem.CurrentUser;  // Lấy người dùng hiện tại

//                ChuTro currentChuTro = null;

//                // Kiểm tra xem user hiện tại có phải là ChuTro không
//                if (user is ChuTro chuTro)
//                {
//                    currentChuTro = ObjectSpace.GetObjectByKey<ChuTro>(chuTro.Oid);
//                }

//                if (currentChuTro != null)
//                {
//                    var objectType = View.ObjectTypeInfo.Type; // Lấy kiểu của đối tượng trong ListView
//                    PropertyInfo chutroProp = objectType.GetProperty("Chutro"); // Kiểm tra xem kiểu đối tượng có thuộc tính Chutro không

//                    if (chutroProp != null)
//                    {
//                        string criteria = "Chutro.Oid = ?";
//                        View.CollectionSource.Criteria["ChutroFilter"] = CriteriaOperator.Parse(criteria, currentChuTro.Oid);
//                    }

//                }
//            }
//        }
//    }
//}
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Security;
using MyPhongTro.Module.BusinessObjects.Chutro;
using System;
using System.Linq;
using System.Reflection;

namespace MyPhongTro.Module.Controllers.FilterControler
{
    public class GenericFilterChutroController : ViewController<ListView>
    {
        protected override void OnActivated()
        {
            base.OnActivated();

            if (TCom.IsAdmin(ObjectSpace)) return;

            ChuTro currentChuTro = ObjectSpace.GetObjectByKey<ChuTro>(SecuritySystem.CurrentUserId);
            if (currentChuTro == null) return;

            var objectType = View.ObjectTypeInfo.Type;
            string filterPath = GetChutroPath(objectType);

            if (!string.IsNullOrEmpty(filterPath))
            {
                string criteria = $"{filterPath}.Oid = ?";
                View.CollectionSource.Criteria["ChutroFilter"] = CriteriaOperator.Parse(criteria, currentChuTro.Oid);
            }
        }

        private string GetChutroPath(Type type, string prefix = "", int depth = 0)
        {
            if (depth > 3) return null; // tránh lặp vô hạn

            // Trường hợp có trực tiếp Chutro
            PropertyInfo direct = type.GetProperty("Chutro");
            if (direct != null)
                return prefix + "Chutro";

            // Duyệt các thuộc tính kiểu reference
            foreach (PropertyInfo prop in type.GetProperties())
            {
                if (typeof(IXafEntityObject).IsAssignableFrom(prop.PropertyType))
                {
                    string path = GetChutroPath(prop.PropertyType, prefix + prop.Name + ".", depth + 1);
                    if (!string.IsNullOrEmpty(path))
                        return path;
                }
            }

            return null;
        }
    }
}
