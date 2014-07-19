#region Using directives
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
#endregion

namespace RD.CustomThemes.Sample
{
  public partial class Window1 : Window
  {
    #region Fields
    private int _themeIndex;
    #endregion

    #region Ctors
    public Window1()
    {
      InitializeComponent();
      _themeIndex = -1;
    }
    #endregion

    #region Methods
    protected override void OnKeyDown(KeyEventArgs e)
    {
      base.OnKeyDown(e);

      if (e.Key == Key.F5)
      {
        ++_themeIndex;
        if (_themeIndex > this.MyApplication.AvailableThemes.Count - 1)
        {
          _themeIndex = -1;
          Application.Current.Resources = null;
        }
        else
        Application.Current.Resources = this.MyApplication.AvailableThemes[_themeIndex];
      }
    }
    #endregion
  }
}