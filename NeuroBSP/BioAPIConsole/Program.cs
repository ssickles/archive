using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IdentityStream.BioAPI;

namespace BioAPIConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            List<Template> templates = new List<Template>();
            BiometricContext.CaptureBSPs = new List<Type>() { 
                Type.GetType("IdentityStream.BSPs.Capture.Test.TestCapture,IdentityStream.BSPs.Capture.Test"), 
                Type.GetType("IdentityStream.BSPs.Capture.Test.TestCapture,IdentityStream.BSPs.Capture.Test") 
            };
            CaptureSession session = BiometricContext.Current.CreateCaptureSession();
            session.SourcePlaced += new EventHandler(session_SourcePlaced);
            session.SourceRemoved += new EventHandler(session_SourceRemoved);
            session.ImageCaptured += new EventHandler<ImageCapturedEventArgs>(session_ImageCaptured);
            Console.WriteLine("Initialized");
            templates.Add(session.Capture(1));
            Console.WriteLine("Template Captured: {0}", templates[0].MinutiaCount);
            templates.Add(session.Capture(3));
            Console.WriteLine("Template Captured: {0}", templates[1].MinutiaCount);
            templates.Add(session.Capture(3));
            Console.WriteLine("Template Captured: {0}", templates[2].MinutiaCount);
            templates.Add(session.Capture(3));
            Console.WriteLine("Template Captured: {0}", templates[3].MinutiaCount);
            BiometricContext.Current.EndCaptureSession();
            Console.WriteLine("Session Complete");
        }

        static void session_ImageCaptured(object sender, ImageCapturedEventArgs e)
        {
            Console.WriteLine(string.Format("Image Captured: {0}", e.Status));
        }

        static void session_SourceRemoved(object sender, EventArgs e)
        {
            Console.WriteLine("Source Removed");
        }

        static void session_SourcePlaced(object sender, EventArgs e)
        {
            Console.WriteLine("Source Placed");
        }
    }
}
