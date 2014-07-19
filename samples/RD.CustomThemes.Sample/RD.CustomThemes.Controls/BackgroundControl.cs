#region Using directives
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
#endregion

namespace RD.CustomThemes.Controls
{
  public class BackgroundControl : Control
  {
    #region Ctors
    public BackgroundControl()
    {
    }
    static BackgroundControl()
    {
      DefaultStyleKeyProperty.OverrideMetadata(typeof(BackgroundControl), new FrameworkPropertyMetadata(typeof(BackgroundControl)));
    }
    #endregion
  }
}
