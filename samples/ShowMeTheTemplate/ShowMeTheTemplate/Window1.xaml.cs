using System;
using System.Reflection;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.IO;
using System.Windows.Forms.Integration;
using System.Xml;
using System.Windows.Markup;
using System.Windows.Navigation;

namespace ShowMeTheTemplate {
  public partial class Window1 : System.Windows.Window {
    Assembly presentationFrameworkAssembly = Assembly.LoadWithPartialName("PresentationFramework");
    Dictionary<Type, object> typeElementMap = new Dictionary<Type, object>();
    List<string> filesToDeleteOnExit = new List<string>();

    public Window1() {
      InitializeComponent();
      bookLink.Click += bookLink_Click;
      Closed += Window1_Closed;
      themes.SelectionChanged += themes_SelectionChanged;
      DataContext = new List<TemplatedElementInfo>(TemplatedElementInfo.GetTemplatedElements(presentationFrameworkAssembly));
      themes.SelectedIndex = 0;
    }

    void bookLink_Click(object sender, RoutedEventArgs e) {
      System.Diagnostics.Process.Start("http://sellsbrothers.com/writing/wpfbook/");
    }

    void themes_SelectionChanged(object sender, SelectionChangedEventArgs e) {
      ComboBox cb = (ComboBox)sender;
      Uri themeUri = new Uri((string)((ComboBoxItem)cb.SelectedItem).Tag, UriKind.Relative);
      ResourceDictionary themeResources = (ResourceDictionary)Application.LoadComponent(themeUri);
      templateItems.Resources = themeResources;
    }

    void Window1_Closed(object sender, EventArgs e) {
      foreach( string file in filesToDeleteOnExit ) {
        File.Delete(file);
      }
    }

    void ElementHolder_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e) {
      ContentControl elementHolder = (ContentControl)sender;
      TemplatedElementInfo elementInfo = (TemplatedElementInfo)elementHolder.DataContext;

      // Get element (cached)
      object element = GetElement(elementInfo.ElementType);

      // Create and show the element (some have to be shown to give up their templates...)
      ShowElement(elementHolder, element);

      // Fill the element (don't seem to need to do this, but makes it easier to see on the screen...)
      FillElement(element);
    }

    // Get the element from a cache based on the type
    // Used to avoid recreating a type twice and used so that when the WebBrowser needs to get the templates for each property, it knows where to look
    object GetElement(Type elementType) {
      if( !typeElementMap.ContainsKey(elementType) ) {
        typeElementMap[elementType] = presentationFrameworkAssembly.CreateInstance(elementType.FullName);
      }

      return typeElementMap[elementType];
    }

    // When the WF host has loaded (for each property on the currently selected control),
    // tell the WebBrowser to navigate to the property's template
    void WindowsFormsHost_Loaded(object sender, RoutedEventArgs e) {
      WindowsFormsHost host = (WindowsFormsHost)sender;
      PropertyInfo prop = (PropertyInfo)host.DataContext;
      System.Windows.Forms.WebBrowser browser = (System.Windows.Forms.WebBrowser)host.Child;
      Type elementType = prop.ReflectedType;
      object element = GetElement(elementType);
      FrameworkTemplate template = (FrameworkTemplate)prop.GetValue(element, null);
      ShowTemplate(browser, template);
    }

    void ShowTemplate(System.Windows.Forms.WebBrowser browser, FrameworkTemplate template) {
      if( template == null ) {
        browser.DocumentText = "(no template)";
        return;
      }

      // Write the template to a file so that the browser knows to show it as XML
      string filename = System.IO.Path.GetTempFileName();
      File.Delete(filename);
      filename = System.IO.Path.ChangeExtension(filename, "xml");

      // pretty print the XAML (for View Source)
      using( XmlTextWriter writer = new XmlTextWriter(filename, System.Text.Encoding.UTF8) ) {
        writer.Formatting = Formatting.Indented;
        XamlWriter.Save(template, writer);
      }

      // Show the template
      browser.Navigate(new Uri(@"file:///" + filename));
    }

    void WebBrowser_Navigated(object sender, System.Windows.Forms.WebBrowserNavigatedEventArgs e) {
      // Queue the files to be deleted at shutdown (otherwise, View Source doesn't work)
      if( e.Url.IsFile ) { filesToDeleteOnExit.Add(e.Url.LocalPath); }
    }

    void ShowElement(ContentControl elementHolder, object element) {
      elementHolder.Content = null;

      Type elementType = element.GetType();
      if( (elementType == typeof(ToolTip)) ||
          (elementType == typeof(Window)) ) {
        // can't be set as a child, but don't need to be shown, so do nothing
      }
      else if( elementType == typeof(NavigationWindow) ) {
        NavigationWindow wnd = (NavigationWindow)element;
        wnd.WindowState = WindowState.Minimized;
        wnd.ShowInTaskbar = false;
        wnd.Show(); // needs to be shown once to "hydrate" the template
        wnd.Hide();
      }
      else if( typeof(ContextMenu).IsAssignableFrom(elementType) ) {
        elementHolder.ContextMenu = (ContextMenu)element;
      }
      else if( typeof(Page).IsAssignableFrom(elementType) ) {
        Frame frame = new Frame();
        frame.Content = element;
        elementHolder.Content = frame;
      }
      else {
        elementHolder.Content = element;
      }
    }

    void FillElement(object element) {
      if( element is ContentControl ) {
        ((ContentControl)element).Content = "(some content)";

        if( element is HeaderedContentControl ) {
          ((HeaderedContentControl)element).Header = "(a header)";
        }
      }
      else if( element is ItemsControl ) {
        ((ItemsControl)element).Items.Add("(an item)");
      }
      else if( element is PasswordBox ) {
        ((PasswordBox)element).Password = "(a password)";
      }
      else if( element is System.Windows.Controls.Primitives.TextBoxBase ) {
        ((System.Windows.Controls.Primitives.TextBoxBase)element).AppendText("(some text)");
      }
      else if( element is Page ) {
        ((Page)element).Content = "(some content)";
      }
    }

  }

  class TemplatedElementInfo {
    Type elementType;
    public Type ElementType {
      get { return elementType; }
    }

    IEnumerable<PropertyInfo> templatedProperties;
    public IEnumerable<PropertyInfo> TemplateProperties {
      get { return templatedProperties; }
    }

    public TemplatedElementInfo(Type elementType, IEnumerable<PropertyInfo> templatedProperties) {
      this.elementType = elementType;
      this.templatedProperties = templatedProperties;
    }

    public static IEnumerable<TemplatedElementInfo> GetTemplatedElements(Assembly assem) {
      Type frameworkTemplateType = typeof(FrameworkTemplate);

      foreach( Type type in assem.GetTypes() ) {
        if( type.IsAbstract ) { continue; }
        if( type.ContainsGenericParameters ) { continue; }
        if( type.GetConstructor(new Type[] { }) == null ) { continue; }

        List<PropertyInfo> templatedProperties = new List<PropertyInfo>();
        foreach( PropertyInfo prop in type.GetProperties(BindingFlags.Instance | BindingFlags.Public) ) {
          if( frameworkTemplateType.IsAssignableFrom(prop.PropertyType) ) {
            templatedProperties.Add(prop);
          }
        }

        if( templatedProperties.Count == 0 ) { continue; }

        yield return new TemplatedElementInfo(type, templatedProperties);
      }
    }
  }

}
