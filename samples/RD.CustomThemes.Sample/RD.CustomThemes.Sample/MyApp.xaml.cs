#region Using directives
using System;
using System.Collections.Generic;
using System.Windows;
#endregion

namespace RD.CustomThemes.Sample
{
  public partial class MyApp : Application
  {
    #region Fields
    private List<ResourceDictionary> _themeList;
    #endregion

    #region Ctor
    public MyApp()
    {
      _themeList = new List<ResourceDictionary>(4);
    }
    #endregion

    #region Properties
    internal List<ResourceDictionary> AvailableThemes
    {
      get { return _themeList; }
    }
    #endregion

    #region Methods
    private void ApplicationStartUp(object sender, StartupEventArgs e)
    {
      _themeList.Add((ResourceDictionary)Application.LoadComponent(new Uri(@"RD.CustomThemes;;;component\themes/customthemes.luna.baml", UriKind.Relative)));
      _themeList.Add((ResourceDictionary)Application.LoadComponent(new Uri(@"RD.CustomThemes;;;component\themes/customthemes.xbox.baml", UriKind.Relative)));
      _themeList.Add((ResourceDictionary)Application.LoadComponent(new Uri(@"RD.CustomThemes;;;component\themes/customthemes.toon.baml", UriKind.Relative)));
    }
    #endregion

  }
}