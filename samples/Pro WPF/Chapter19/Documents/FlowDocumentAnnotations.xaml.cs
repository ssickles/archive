using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.IO;
using System.Windows.Annotations;
using System.Windows.Annotations.Storage;
using System.Xml;
using System.Security.Principal;

namespace Documents
{
    /// <summary>
    /// Interaction logic for FlowDocumentAnnotations.xaml
    /// </summary>

    public partial class FlowDocumentAnnotations : System.Windows.Window
    {

        public FlowDocumentAnnotations()
        {
            InitializeComponent();
        }

        private Stream annotationStream;
        private AnnotationService service;

        protected void window_Loaded(object sender, RoutedEventArgs e)
        {
            WindowsIdentity identity = System.Security.Principal.WindowsIdentity.GetCurrent();
            this.Resources["AuthorName"] = identity.Name;

            service = AnnotationService.GetService(docReader);

            if (service == null)
            {
                // Create a stream for the annotations to be stored in.
                //AnnotationStream =
                 // new FileStream("annotations.xml", FileMode.OpenOrCreate);
                annotationStream = new MemoryStream();

                // Create the on the document container. 
                service = new AnnotationService(docReader);
                
                // Create the AnnotationStore using the stream.
                AnnotationStore store = new XmlStreamStore(annotationStream);

                // Enable annotations.
                service.Enable(store);                
            }
        }
                
        protected void window_Unloaded(object sender, RoutedEventArgs e)
        {
            if (service != null && service.IsEnabled)
            {
                // Flush annotations to stream.
                service.Store.Flush();

                // Disable annotations.
                service.Disable();                                
                annotationStream.Close();
            }
        }

        private void cmdShowAllAnotations_Click(object sender, RoutedEventArgs e)
        {
            IList<Annotation> annotations = service.Store.GetAnnotations();
            foreach (Annotation annotation in annotations)
            {
                // Check for text information.
                if (annotation.Cargos.Count > 1)
                {
                    string base64Text = annotation.Cargos[1].Contents[0].InnerText;
                    byte[] decoded = Convert.FromBase64String(base64Text);
                    MemoryStream m = new MemoryStream();
                    m.Write(decoded, 0, decoded.Length);
                    m.Position = 0;
                    StreamReader r = new StreamReader(m);
                    string annotationXaml = r.ReadToEnd();
                    MessageBox.Show(annotationXaml);
                }
            }


            PrintDialog dialog = new PrintDialog();
            bool? result = dialog.ShowDialog();
            if (result != null && result.Value)
            {
                System.Windows.Xps.XpsDocumentWriter writer = System.Printing.PrintQueue.CreateXpsDocumentWriter(dialog.PrintQueue);
                
                AnnotationDocumentPaginator adp = new AnnotationDocumentPaginator(
                                        ((IDocumentPaginatorSource)docReader.Document).DocumentPaginator,
                                        service.Store);
                writer.Write(adp);
            }

        }

    }
}