using DevExpress.Blazor;
using DevExpress.DashboardCommon.Viewer;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Blazor.Editors;
using Microsoft.AspNetCore.Mvc;

namespace MyPhongTro.Blazor.Server.Controllers.Chung
{
    public class ListViewController : ViewController<ListView> // áp dụng cho tất cả các ListView
    {
        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();
            if (View.Editor is DxGridListEditor gridListEditor)
            {
                // For example, you can modify the grid's settings or add custom columns
                IDxGridAdapter dxGridAdapter = gridListEditor.GetGridAdapter();
                dxGridAdapter.GridModel.ColumnResizeMode = DevExpress.Blazor.GridColumnResizeMode.ColumnsContainer; 

                // Disable the link for LookupPropertyEditor and ObjectPropertyEditor
                if (dxGridAdapter != null)
                {
                    foreach (var editor in gridListEditor.PropertyEditors)  // không hiện link 
                    {
                        if (editor is LookupPropertyEditor lookupEditor)
                        {
                            lookupEditor.ShowLink = false;
                        }
                        else if (editor is ObjectPropertyEditor objectEditor)
                        {
                            objectEditor.ShowLink = false;
                        }

                    }


                }
              


            }
            
        }



    }
}
