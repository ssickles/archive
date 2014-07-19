#region Using directives
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
#endregion

namespace RD.CustomThemes.Controls
{
  public class LogoControl : Control
  {
    #region Ctors
    public LogoControl()
    {
    }
    static LogoControl()
    {
      DefaultStyleKeyProperty.OverrideMetadata(typeof(LogoControl), new FrameworkPropertyMetadata(typeof(LogoControl)));
    }
    #endregion
  }
}
