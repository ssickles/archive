using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.ComponentModel;
using System.Collections.Specialized;
using System.Collections.ObjectModel;
using System.Globalization;

namespace Silverlight3DataGrid
{
public abstract class GroupDescription : INotifyPropertyChanged
{
    // Fields
    private ObservableCollection<object> _explicitGroupNames = new ObservableCollection<object>();

    // Events
    protected event PropertyChangedEventHandler PropertyChanged;

    // Methods
    protected GroupDescription()
    {
        this._explicitGroupNames.CollectionChanged += new NotifyCollectionChangedEventHandler(this.OnGroupNamesChanged);
    }

    public abstract object GroupNameFromItem(object item, int level, CultureInfo culture);
    public virtual bool NamesMatch(object groupName, object itemName)
    {
        return object.Equals(groupName, itemName);
    }

    private void OnGroupNamesChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
        this.OnPropertyChanged(new PropertyChangedEventArgs("GroupNames"));
    }

    protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
    {
        if (this.PropertyChanged != null)
        {
            this.PropertyChanged(this, e);
        }
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    public bool ShouldSerializeGroupNames()
    {
        return (this._explicitGroupNames.Count > 0);
    }

    // Properties
    public ObservableCollection<object> GroupNames
    {
        get
        {
            return this._explicitGroupNames;
        }
    }
}
}
