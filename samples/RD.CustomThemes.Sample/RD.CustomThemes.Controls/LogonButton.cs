#region Using directives
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
#endregion

namespace RD.CustomThemes.Controls
{
  public class LogonButton : Button
  {
    #region Ctors
    public LogonButton()
    {
    }
    static LogonButton()
    {
      DefaultStyleKeyProperty.OverrideMetadata(typeof(LogonButton), new FrameworkPropertyMetadata(typeof(LogonButton)));
    }
    #endregion
  }
}
