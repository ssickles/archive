using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using System.Windows.Media.Animation;
using System.Windows;
using FluidKit.Controls;

namespace WPFDisciples.FluidKitTransitions.FlipTransition
{
    public class FlipTransition : Transition
    {
        private readonly Model3DGroup _flipModelContainer;
        private readonly Model3DGroup _rootModel;
        private readonly Viewport3D _viewport;
        private Direction _direction = Direction.TopToBottom;
        private FrameworkElement elementToMoveToView;
        private Transform elementToMoveToViewTransform;
        private Point elementToMovePoint;

        public FlipTransition()
        {
            TransitionResources.MergedDictionaries.Add((ResourceDictionary)
                Application.LoadComponent(new Uri("/WPFDisciplesSampleApp;;;component/FluidKitTransitions/Flip/Flip.xaml",
                                                  UriKind.Relative)));

            _viewport = TransitionResources["3DFlip"] as Viewport3D;

            _rootModel = (_viewport.Children[0] as ModelVisual3D).Content as Model3DGroup;
            _flipModelContainer = _rootModel.Children[1] as Model3DGroup;

        }

        public Direction Direction
        {
            get { return _direction; }
            set { _direction = value; }
        }

        private void RegisterNameScope()
        {
            // Transforms
            Transform3DGroup groupTrans = _rootModel.Transform as Transform3DGroup;
            AxisAngleRotation3D rotator = (groupTrans.Children[0] as RotateTransform3D).Rotation as
                                          AxisAngleRotation3D;
            TranslateTransform3D translator = groupTrans.Children[1] as TranslateTransform3D;

            // We are setting names in code since x:Name in the ResourceDictionary is not supported
            NameScope scope = GetNameScope();
            scope.RegisterName("XFORM_Rotate", rotator);
            scope.RegisterName("XFORM_Translate", translator);
        }

        public override void Setup(VisualBrush prevBrush, VisualBrush nextBrush)
        {
            //store the visual that will be moved to view, so that if any changes are done to it we can rollback to the original state
            elementToMoveToView = (FrameworkElement)nextBrush.Visual;
            elementToMoveToViewTransform = elementToMoveToView.RenderTransform;
            elementToMovePoint = elementToMoveToView.RenderTransformOrigin;

            Owner.AddTransitionElement(_viewport);

            RegisterNameScope();

            AdjustViewport(Owner, prevBrush, nextBrush);
        }

        private void AdjustViewport(FrameworkElement container, VisualBrush prevBrush, VisualBrush nextBrush)
        {
            // Adjusting the positions according to the Container's Width/Height
            double aspect = container.ActualWidth / container.ActualHeight;

            _flipModelContainer.Children.Add(PrepareFace(prevBrush, nextBrush, aspect));

            // Camera
            AdjustCamera(aspect);

            //adjust the transforms and the visual to move
            AdjustTransformsAndVisual(aspect, nextBrush);
        }

        private GeometryModel3D PrepareFace(VisualBrush prevBrush, VisualBrush nextBrush, double aspect)
        {
            // Create the mesh
            MeshGeometry3D mesh = (_viewport.Resources["FlipFace"] as MeshGeometry3D).Clone();

            mesh.Positions[0] = new Point3D(-aspect / 2, 0.5, 0);
            mesh.Positions[1] = new Point3D(aspect / 2, 0.5, 0);
            mesh.Positions[2] = new Point3D(aspect / 2, -0.5, 0);
            mesh.Positions[3] = new Point3D(-aspect / 2, -0.5, 0);

            // Apply material
            DiffuseMaterial material = new DiffuseMaterial(prevBrush);

            // Create the model
            GeometryModel3D model = new GeometryModel3D(mesh, material);
            model.BackMaterial = new DiffuseMaterial(nextBrush);
            return model;
        }

        private void AdjustCamera(double aspect)
        {
            PerspectiveCamera camera = _viewport.Camera as PerspectiveCamera;
            double angle = camera.FieldOfView / 2;
            double cameraZPos = (aspect / 2) / Math.Tan(angle * Math.PI / 180);
            camera.Position = new Point3D(0, 0, cameraZPos);
        }

        private void AdjustTransformsAndVisual(double aspect, VisualBrush nextBrush)
        {
            // Axis of rotation
            RotateTransform3D rotateXForm = (_rootModel.Transform as Transform3DGroup).Children[0] as RotateTransform3D;
            TranslateTransform3D translateXForm = (_rootModel.Transform as Transform3DGroup).Children[1] as TranslateTransform3D;

            Storyboard animator = _viewport.Resources["STORYBOARD_FlipAnimator"] as Storyboard;
            DoubleAnimationUsingKeyFrames rotateAnim = animator.Children[0] as DoubleAnimationUsingKeyFrames;
            DoubleAnimationUsingKeyFrames translateAnim = animator.Children[1] as DoubleAnimationUsingKeyFrames;
            rotateAnim.Duration = this.Duration;
            translateAnim.Duration = this.Duration;

            switch (Direction)
            {
                case Direction.LeftToRight:
                    rotateAnim.KeyFrames[1].Value = 180;
                    rotateXForm.CenterX = -aspect / 2;
                    (rotateXForm.Rotation as AxisAngleRotation3D).Axis = new Vector3D(0, 1, 0);

                    translateXForm.OffsetY = 0;
                    Storyboard.SetTargetProperty(translateAnim, new PropertyPath(TranslateTransform3D.OffsetXProperty));
                    translateAnim.KeyFrames[1].Value = aspect;

                    //apply a transform on the visual to move so that the rotation does not affect the look of it
                    elementToMoveToView.RenderTransformOrigin = new Point(0.5, 0.5);
                    TransformGroup group = new TransformGroup();
                    group.Children.Add(new ScaleTransform(1, -1));
                    group.Children.Add(new RotateTransform(-180));
                    elementToMoveToView.RenderTransform = group;
                    break;

                case Direction.RightToLeft:
                    rotateAnim.KeyFrames[1].Value = -180;
                    rotateXForm.CenterX = aspect / 2;
                    (rotateXForm.Rotation as AxisAngleRotation3D).Axis = new Vector3D(0, 1, 0);

                    Storyboard.SetTargetProperty(translateAnim, new PropertyPath(TranslateTransform3D.OffsetXProperty));
                    translateAnim.KeyFrames[1].Value = -aspect;

                    elementToMoveToView.RenderTransformOrigin = new Point(0.5, 0.5);
                    group = new TransformGroup();
                    group.Children.Add(new ScaleTransform(1, -1));
                    group.Children.Add(new RotateTransform(-180));
                    elementToMoveToView.RenderTransform = group;
                    break;

                case Direction.TopToBottom:
                    rotateAnim.KeyFrames[1].Value = 180;
                    rotateXForm.CenterY = 0.5;
                    (rotateXForm.Rotation as AxisAngleRotation3D).Axis = new Vector3D(1, 0, 0);

                    Storyboard.SetTargetProperty(translateAnim, new PropertyPath(TranslateTransform3D.OffsetYProperty));
                    translateAnim.KeyFrames[1].Value = -1;

                    elementToMoveToView.RenderTransformOrigin = new Point(0.5, 0.5);
                    group = new TransformGroup();
                    group.Children.Add(new ScaleTransform(-1, 1));
                    group.Children.Add(new RotateTransform(-180));
                    elementToMoveToView.RenderTransform = group;
                    break;

                case Direction.BottomToTop:
                    rotateAnim.KeyFrames[1].Value = -180;
                    rotateXForm.CenterY = -0.5;
                    (rotateXForm.Rotation as AxisAngleRotation3D).Axis = new Vector3D(1, 0, 0);

                    Storyboard.SetTargetProperty(translateAnim, new PropertyPath(TranslateTransform3D.OffsetYProperty));
                    translateAnim.KeyFrames[1].Value = 1;

                    elementToMoveToView.RenderTransformOrigin = new Point(0.5, 0.5);
                    group = new TransformGroup();
                    group.Children.Add(new ScaleTransform(-1, 1));
                    group.Children.Add(new RotateTransform(180));
                    elementToMoveToView.RenderTransform = group;
                    break;
            }
        }

        public override Storyboard PrepareStoryboard()
        {
            return (Storyboard)_viewport.Resources["STORYBOARD_FlipAnimator"];
        }


        public override void Cleanup()
        {
            elementToMoveToView.RenderTransformOrigin = elementToMovePoint;
            elementToMoveToView.RenderTransform = elementToMoveToViewTransform;
            _flipModelContainer.Children.Clear();
        }
    }
}
