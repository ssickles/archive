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
using System.Windows.Media.Media3D;
using System.Windows.Media.Animation;

namespace DrawingIn3D
{
    /// <summary>
    /// Interaction logic for Materials.xaml
    /// </summary>

    public partial class HitTesting : System.Windows.Window
    {

        public HitTesting()
        {
            InitializeComponent();
        }

        private void viewport_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Viewport3D viewport = (Viewport3D)sender;
            Point location = e.GetPosition(viewport);
            HitTestResult hitResult = VisualTreeHelper.HitTest(viewport, location);
            
            if (hitResult != null && hitResult.VisualHit == ringVisual)
            {
                // Hit the ring.
            }
            

            RayMeshGeometry3DHitTestResult meshHitResult = hitResult as RayMeshGeometry3DHitTestResult;
            if (meshHitResult != null && meshHitResult.ModelHit == ringModel)
            {
                // Hit the ring.
            }
            if (meshHitResult != null && meshHitResult.MeshHit == ringMesh)
            {                
                //meshHitResult.PointHit 
                axisRotation.Axis = new Vector3D(
                    -meshHitResult.PointHit.Y, meshHitResult.PointHit.X, 0);

                DoubleAnimation animation = new DoubleAnimation();                
                animation.To = 40;
                animation.DecelerationRatio = 1;
                animation.Duration = TimeSpan.FromSeconds(0.15);
                animation.AutoReverse = true;
                axisRotation.BeginAnimation(AxisAngleRotation3D.AngleProperty, animation);               
            }
        }
    }
}
