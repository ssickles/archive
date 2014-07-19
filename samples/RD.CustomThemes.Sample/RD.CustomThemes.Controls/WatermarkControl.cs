#region Using directives
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
#endregion

namespace RD.CustomThemes.Controls
{
  public class WatermarkControl : Control
  {
    #region Ctors
    public WatermarkControl()
    {
    }
    static WatermarkControl()
    {
      DefaultStyleKeyProperty.OverrideMetadata(typeof(WatermarkControl), new FrameworkPropertyMetadata(typeof(WatermarkControl)));
    }
    #endregion
  }
}
