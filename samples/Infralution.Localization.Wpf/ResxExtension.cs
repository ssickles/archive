//
//      FILE:   ResxExtension.cs.
//
// COPYRIGHT:   Copyright 2008 
//              Infralution
//
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Markup;
using System.Resources;
using System.Reflection;
using System.Windows;
using System.ComponentModel;
using System.Globalization;
using System.Threading;
using System.Diagnostics;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Drawing;
using System.Windows.Interop;
using System.IO;
using System.Runtime.InteropServices;

[assembly: XmlnsDefinition("http://schemas.microsoft.com/winfx/2006/xaml/presentation", "Infralution.Localization.Wpf")]
[assembly: XmlnsDefinition("http://schemas.microsoft.com/winfx/2007/xaml/presentation", "Infralution.Localization.Wpf")]
[assembly: XmlnsDefinition("http://schemas.microsoft.com/winfx/2008/xaml/presentation", "Infralution.Localization.Wpf")]

namespace Infralution.Localization.Wpf
{
    /// <summary>
    /// Defines the handling method for the <see cref="ResxExtension.GetResource"/> event
    /// </summary>
    /// <param name="resxName">The name of the resx file</param>
    /// <param name="key">The resource key within the file</param>
    /// <param name="culture">The culture to get the resource for</param>
    /// <returns>The resource</returns>
    public delegate object GetResourceHandler(string resxName, string key, CultureInfo culture);

    /// <summary>
    /// A markup extension to allow resources for WPF Windows and controls to be retrieved
    /// from an embedded resource (resx) file associated with the window or control
    /// </summary>
    /// <remarks>
    /// 
    /// </remarks>
    [MarkupExtensionReturnType(typeof(object))]
    public class ResxExtension : ManagedMarkupExtension
    {
       
        #region Member Variables

        /// <summary>
        /// The type name that the resource is associated with
        /// </summary>
        private string _resxName;

        /// <summary>
        /// The key used to retrieve the resource
        /// </summary>
        private string _key;

        /// <summary>
        /// The default value for the property
        /// </summary>
        private string _defaultValue;

        /// <summary>
        /// The resource manager to use for this extension.  Holding a strong reference to the
        /// Resource Manager keeps it in the cache while ever there are ResxExtensions that
        /// are using it.
        /// </summary>
        private ResourceManager _resourceManager;

        /// <summary>
        /// Cached resource managers
        /// </summary>
        private static Dictionary<string, WeakReference> _resourceManagers = new Dictionary<string, WeakReference>();

        /// <summary>
        /// The manager for resx extensions
        /// </summary>
        private static MarkupExtensionManager _markupManager = new MarkupExtensionManager(40);


        #endregion

        #region Public Interface

        /// <summary>
        /// This event allows a designer or preview application (such as Globalizer.NET) to
        /// intercept calls to get resources and provide the values instead dynamically
        /// </summary>
        public static event GetResourceHandler GetResource;

        /// <summary>
        /// Create a new instance of the markup extension
        /// </summary>
        public ResxExtension()
            : base(_markupManager)
        {
        }

        /// <summary>
        /// Create a new instance of the markup extension
        /// </summary>
        /// <param name="resxName">The fully qualified name of the embedded resx (without .resources)</param>
        /// <param name="key">The key used to get the value from the resources</param>
        /// <param name="defaultValue">
        /// The default value for the property (can be null).  This is useful for non-string
        /// that properties that may otherwise cause page load errors if the resource is not
        /// present for some reason (eg at design time before the assembly has been compiled)
        /// </param>
        public ResxExtension(string resxName, string key, string defaultValue)
            : base(_markupManager)
        {
            this._resxName = resxName;
            this._key = key;
            if (!string.IsNullOrEmpty(defaultValue))
            {
                this._defaultValue = defaultValue;
            }
        }


        /// <summary>
        /// The fully qualified name of the embedded resx (without .resources) to get
        /// the resource from
        /// </summary>
        public string ResxName
        {
            get { return _resxName; }
            set { _resxName = value; }
        }

        /// <summary>
        /// The name of the resource key
        /// </summary>
        public string Key
        {
            get { return _key; }
            set { _key = value; }
        }

        /// <summary>
        /// The default value to use if the resource can't be found
        /// </summary>
        /// <remarks>
        /// This particularly useful for properties which require non-null
        /// values because it allows the page to be displayed even if
        /// the resource can't be loaded
        /// </remarks>
        public string DefaultValue
        {
            get { return _defaultValue; }
            set { _defaultValue = value; }
        }

        /// <summary>
        /// Return the MarkupManager for this extension
        /// </summary>
        public static MarkupExtensionManager MarkupManager
        {
            get { return _markupManager; }
        }

        /// <summary>
        /// Use the Markup Manager to update all targets
        /// </summary>
        public static void UpdateAllTargets()
        {
            _markupManager.UpdateAllTargets();
        }

        /// <summary>
        /// Update the ResxExtension target with the given key
        /// </summary>
        public static void UpdateTarget(string key)
        {
            foreach (ResxExtension target in _markupManager.ActiveExtensions)
            {
                if (target.Key == key)
                {
                    target.UpdateTarget();
                }
            }
        }

        #endregion

        #region Local Methods

        /// <summary>
        /// Check if the assembly contains an embedded resx of the given name
        /// </summary>
        /// <param name="assembly">The assembly to check</param>
        /// <param name="resxName">The name of the resource we are looking for</param>
        /// <returns>True if the assembly contains the resource</returns>
        private bool HasEmbeddedResx(Assembly assembly, string resxName)
        {
            try
            {
                string[] resources = assembly.GetManifestResourceNames();
                string searchName = resxName.ToLower() + ".resources";
                foreach (string resource in resources)
                {
                    if (resource.ToLower() == searchName) return true;
                }
            }
            catch
            {
                // GetManifestResourceNames throws an exception for some
                // dynamic assemblies - just ignore these assemblies.
            }
            return false;
        }

        /// <summary>
        /// Find the assembly that contains the type
        /// </summary>
        /// <returns>The assembly if loaded (otherwise null)</returns>
        private Assembly FindResourceAssembly()
        {
            Assembly assembly = Assembly.GetEntryAssembly();

            // check the entry assembly first - this will short circuit a lot of searching
            //
            if (assembly != null && HasEmbeddedResx(assembly, ResxName)) return assembly;

            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (Assembly searchAssembly in assemblies)
            {
                // skip system assemblies
                //
                string name = searchAssembly.FullName;
                if (!name.StartsWith("Microsoft.") &&
                    !name.StartsWith("System.") &&
                    !name.StartsWith("System,") &&
                    !name.StartsWith("mscorlib,") &&
                    !name.StartsWith("PresentationFramework,") &&
                    !name.StartsWith("WindowsBase,"))
                {
                   if (HasEmbeddedResx(searchAssembly, ResxName)) return searchAssembly;
                }
            }
            return null;
        }

        /// <summary>
        /// Get the resource manager for this type
        /// </summary>
        /// <param name="resxName">The name of the embedded resx</param>
        /// <returns>The resource manager</returns>
        /// <remarks>Caches resource managers to improve performance</remarks>
        private ResourceManager GetResourceManager(string resxName)
        {
            WeakReference reference = null;
            ResourceManager result = null;
            if (_resourceManagers.TryGetValue(resxName, out reference))
            {
                result = reference.Target as ResourceManager;

                // if the resource manager has been garbage collected then remove the cache
                // entry (it will be readded)
                //
                if (result == null)
                {
                    _resourceManagers.Remove(resxName);
                }
            }

            if (result == null)
            {
                Assembly assembly = FindResourceAssembly();
                if (assembly != null)
                {
                    result = new ResourceManager(resxName, assembly);
                }
                _resourceManagers.Add(resxName, new WeakReference(result));
            }
            return result;
        }

        [DllImport("gdi32.dll")]
        private static extern bool DeleteObject(IntPtr hObject);

        /// <summary>
        /// Convert a resource object to the type required by the WPF element
        /// </summary>
        /// <param name="value">The resource value to convert</param>
        /// <returns>The WPF element value</returns>
        private object ConvertValue(object value)
        {
            object result = null;
            BitmapSource bitmapSource = null;

            // convert icons and bitmaps to BitmapSource objects that WPF uses
            if (value is Icon)
            {
                Icon icon = value as Icon; 
               
                // For icons we must create a new BitmapFrame from the icon data stream
                // The approach we use for bitmaps (below) doesn't work when setting the
                // Icon property of a window (although it will work for other Icons)
                //
                using (MemoryStream iconStream = new MemoryStream())
                {
                    icon.Save(iconStream);
                    iconStream.Seek(0, SeekOrigin.Begin);
                    bitmapSource = BitmapFrame.Create(iconStream);
                }
            }
            else if (value is Bitmap)
            {
                Bitmap bitmap = value as Bitmap;
                IntPtr bitmapHandle = bitmap.GetHbitmap();
                bitmapSource = Imaging.CreateBitmapSourceFromHBitmap(
                    bitmapHandle, IntPtr.Zero, Int32Rect.Empty,
                    BitmapSizeOptions.FromEmptyOptions());
                bitmapSource.Freeze();
                DeleteObject(bitmapHandle);
            }

            if (bitmapSource != null) 
            {
                // if the target property is expecting the Icon to be content then we
                // create an ImageControl and set its Source property to image
                //
                if (TargetPropertyType == typeof(object))
                {
                    System.Windows.Controls.Image imageControl = new System.Windows.Controls.Image();
                    imageControl.Source = bitmapSource;
                    imageControl.Width = bitmapSource.Width;
                    imageControl.Height = bitmapSource.Height;
                    result = imageControl;
                }
                else
                {
                    result = bitmapSource;
                }
            }
            else
            {
                result = value;
            
                // allow for resources to either contain simple strings or typed data
                //
                Type targetType = TargetPropertyType;
                if (value is String && targetType != typeof(String) && targetType != typeof(object))
                {
                    TypeConverter tc = TypeDescriptor.GetConverter(targetType);
                    result = tc.ConvertFromInvariantString(value as string);
                }
            }

            return result;
        }

        /// <summary>
        /// Return the default value for the property
        /// </summary>
        /// <returns></returns>
        private object GetDefaultValue(string key)
        {
            object result = _defaultValue;
            Type targetType = TargetPropertyType;
            if (_defaultValue == null)
            {
                if (targetType == typeof(String) || targetType == typeof(object))
                {
                    result = "#" + key;
                }
            }
            else if (targetType != null)
            {
                // convert the default value if necessary to the required type
                //
                if (targetType != typeof(String) && targetType != typeof(object))
                {
                    try
                    {
                        TypeConverter tc = TypeDescriptor.GetConverter(targetType);
                        result = tc.ConvertFromInvariantString(_defaultValue);
                    }
                    catch
                    {
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// Return the value associated with the key from the resource manager
        /// </summary>
        /// <returns>The value from the resources if possible otherwise the default value</returns>
        protected override object GetValue()
        {
            if (string.IsNullOrEmpty(ResxName))
                throw new ArgumentException("ResxName cannot be null");
            if (string.IsNullOrEmpty(Key))
                throw new ArgumentException("Key cannot be null");


            if (this.IsInDesignMode)
            {
                CultureManager.ShowCultureNotifyIcon();
            }

            object result = null;

            try
            {
                object resource = null;
                if (GetResource != null)
                {
                    resource = GetResource(ResxName, Key, CultureManager.UICulture);
                }
                if (resource == null)
                {
                    if (_resourceManager == null)
                    {
                        _resourceManager = GetResourceManager(ResxName);
                    }
                    if (_resourceManager != null)
                    {
                        resource = _resourceManager.GetObject(Key, CultureManager.UICulture);
                    }
                }
                result = ConvertValue(resource);
            }
            catch
            {
            }

            if (result == null)
            {
                result = GetDefaultValue(Key);
            }
            return result;
        }

        #endregion

    }

}
