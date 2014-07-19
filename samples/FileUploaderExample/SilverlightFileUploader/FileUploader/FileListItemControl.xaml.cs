/*
Copyright 2003-2009 Virtual Chemistry, Inc. (VCI)
http://www.virtualchemistry.com
Using .Net, Silverlight, SharePoint and more to solve your tough problems in web-based data management.

Author: Peter Coley
*/

using System;
using System.Windows;
using System.Windows.Browser;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Vci.Silverlight.FileUploader
{
	public partial class FileListItemControl : UserControl
	{
        private UserFile UserFile { get { return (UserFile)this.DataContext; } }

		public FileListItemControl()
		{
			// Required to initialize variables
			InitializeComponent();

            this.Loaded += new RoutedEventHandler(FileListItemControl_Loaded);
		}

        void FileListItemControl_Loaded(object sender, RoutedEventArgs e)
        {
            VisualStateManager.GoToState(this, UserFile.State.ToString(), true);

            UserFile.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(UserFile_PropertyChanged);
        }

        void UserFile_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "State")
            {
                VisualStateManager.GoToState(this, UserFile.State.ToString(), true);

                if (UserFile.State == Constants.FileStates.Error)
                    ToolTipService.SetToolTip(txtState, UserFile.ErrorMessage);
                else
                    ToolTipService.SetToolTip(txtState, null);
            }
            else if (e.PropertyName == "Percentage")
            {
                // if the percentage is decreasing, don't use an animation
                if (UserFile.Percentage < progressPercent.Value)
                    progressPercent.Value = UserFile.Percentage;
                else
                {
                    sbProgressFrame.Value = UserFile.Percentage;
                    sbProgress.Begin();
                }
            }
        }

        private void btnRemove_Click(object sender, RoutedEventArgs e)
        {
            UserFile.IsDeleted = true;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            UserFile.CancelUpload();
        }
	}
}