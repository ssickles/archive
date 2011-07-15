using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Collections;
using System.Data;

namespace LoadT24Data
{
    /// <summary>
    /// Interaction logic for UploadPreview.xaml
    /// </summary>
    public partial class UploadPreview : Window
    {
        public UploadPreview(DataTable PreviewData)
        {
            InitializeComponent();

            if (PreviewData.Rows.Count > 0)
            {
                GridView view = new GridView();
                foreach (DataColumn column in PreviewData.Columns)
                {
                    GridViewColumn gvColumn = new GridViewColumn();
                    gvColumn.Header = column.ColumnName;
                    gvColumn.DisplayMemberBinding = new Binding(column.ColumnName);
                    view.Columns.Add(gvColumn);
                }

                Preview.View = view;
                Preview.DataContext = PreviewData;
                Preview.SetBinding(ListView.ItemsSourceProperty, new Binding());
            }
        }
    }
}
