using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using System.ComponentModel;


namespace MyPhongTro.Module.BusinessObjects.Hotro
{
    [DomainComponent] // nó là DomainComponent, không phải là một Persistent Object // nó không được lưu trữ trong cơ sở dữ liệu
    [System.ComponentModel.DisplayName("Cập nhật địa phương")]
    [NavigationItem(false)]
    public class ClsImportDiaphuong
    {
        [DevExpress.ExpressApp.Data.Key] // Đánh dấu thuộc tính này là khóa chính của đối tượng
        public Guid Oid { get; set; }

        [XafDisplayName("Mã")]
        public string Ma { get; set; }

        [XafDisplayName("Tên")]
        public string Ten { get; set; }

        [XafDisplayName("Cấp trên")]
        public string Captren { get; set; } // Cấp trên của địa phương này (ví dụ: Tỉnh của một Huyện, hoặc Huyện của một Xã)



    }
    [DefaultClassOptions] // tạo một lớp Persistent Object với các tùy chọn mặc định có list view, detail view, v.v.
    [NavigationItem(false)]
    [System.ComponentModel.DisplayName("Cập nhật dữ liệu")]
    public class ClsImport(Session session) : BaseObject(session)
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).

        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }


        [XafDisplayName("Dữ liệu từ Clipboard")]
        [VisibleInListView(false)]
        [Size(SizeAttribute.Unlimited)] // Thêm dòng này để loại bỏ giới hạn kích thước
        public string Noidung { get; set; }


        private BindingList<ClsImportDiaphuong> _ImportDiaphuongs; // binding list để lưu trữ dữ liệu địa phương đã nhập
        [XafDisplayName("Dữ liệu đã kiểm tra")]
        public BindingList<ClsImportDiaphuong> ImportDiaphuongs
        {
            get
            {
                _ImportDiaphuongs ??= []; // nếu _ImportDiaphuongs chưa được khởi tạo thì khởi tạo nó là một BindingList rỗng
                return _ImportDiaphuongs;
            }
        }
            

    }


}

