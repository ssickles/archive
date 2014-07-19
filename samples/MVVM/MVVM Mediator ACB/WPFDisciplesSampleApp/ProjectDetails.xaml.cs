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
using System.Windows.Navigation;
using System.Windows.Shapes;
using WPFDisciples.ViewModels;

namespace WPFDisciples
{
    /// <summary>
    /// Interaction logic for ProjectDetails.xaml
    /// </summary>
    public partial class ProjectDetails : UserControl
    {
        #region Project

        /// <summary>
        /// Project Dependency Property
        /// </summary>
        public static readonly DependencyProperty ProjectProperty =
            DependencyProperty.Register("Project", typeof(WPFDisciples.Backend.ProjectDetails), typeof(ProjectDetails),
                new FrameworkPropertyMetadata((WPFDisciples.Backend.ProjectDetails)null,
                    new PropertyChangedCallback(OnProjectChanged)));

        /// <summary>
        /// Gets or sets the Project property.  This dependency property 
        /// indicates ....
        /// </summary>
        public WPFDisciples.Backend.ProjectDetails Project
        {
            get { return (WPFDisciples.Backend.ProjectDetails)GetValue(ProjectProperty); }
            set { SetValue(ProjectProperty, value); }
        }

        /// <summary>
        /// Handles changes to the Project property.
        /// </summary>
        private static void OnProjectChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((ProjectDetails)d).OnProjectChanged(e);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes to the Project property.
        /// </summary>
        protected virtual void OnProjectChanged(DependencyPropertyChangedEventArgs e)
        {
            if (viewModel != null)
                viewModel.Project = Project;
        }

        #endregion

        #region IsEditMode

        /// <summary>
        /// IsEditMode Dependency Property
        /// </summary>
        public static readonly DependencyProperty IsEditModeProperty =
            DependencyProperty.Register("IsEditMode", typeof(bool), typeof(ProjectDetails),
                new FrameworkPropertyMetadata((bool)false,
                    new PropertyChangedCallback(OnIsEditModeChanged)));

        /// <summary>
        /// Gets or sets the IsEditMode property.  This dependency property 
        /// indicates ....
        /// </summary>
        public bool IsEditMode
        {
            get { return (bool)GetValue(IsEditModeProperty); }
            set { SetValue(IsEditModeProperty, value); }
        }

        /// <summary>
        /// Handles changes to the IsEditMode property.
        /// </summary>
        private static void OnIsEditModeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((ProjectDetails)d).OnIsEditModeChanged(e);
        }

        /// <summary>
        /// Provides derived classes an opportunity to handle changes to the IsEditMode property.
        /// </summary>
        protected virtual void OnIsEditModeChanged(DependencyPropertyChangedEventArgs e)
        {
            viewModel.IsEditMode = IsEditMode;
        }

        #endregion



        ProjectDetailsViewModel viewModel;
        public ProjectDetails()
        {
            InitializeComponent();

            viewModel = new ProjectDetailsViewModel();
            DataContext = viewModel;
        }
    }
}
