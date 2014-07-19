#region Using directives
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
#endregion

namespace RD.CustomThemes.Controls
{
  public class LogonGrid : Grid
  {
    #region Ctors
    public LogonGrid()
    {
    }
    static LogonGrid()
    {
      DefaultStyleKeyProperty.OverrideMetadata(typeof(LogonGrid), new FrameworkPropertyMetadata(typeof(LogonGrid)));
    }
    #endregion
  }
}
