using MvvmFoundation.Wpf;
using System.Windows.Input;

namespace WpfDemoApp.ViewModels
{
    public class NumberViewModel : ObservableObject
    {
        #region Observable Properties

        public bool IsEven
        {
            get { return this.Value % 2 == 0; }
        }

        public bool IsNegative
        {
            get { return this.Value < 0; }
        }

        public int Value
        {
            get { return _value; }
            set
            {
                if (value == _value)
                    return;

                bool wasEven = this.IsEven;
                bool wasNegative = this.IsNegative;

                _value = value;

                base.RaisePropertyChanged("Value");

                if (wasEven != this.IsEven)
                    base.RaisePropertyChanged("IsEven");

                if (wasNegative != this.IsNegative)
                    base.RaisePropertyChanged("IsNegative");
            }
        }

        #endregion // Observable Properties

        #region Commands

        public ICommand DecrementCommand
        {
            get { return _decrementCommand ?? (_decrementCommand = new RelayCommand(() => --this.Value)); }
        }

        public ICommand IncrementCommand
        {
            get { return _incrementCommand ?? (_incrementCommand = new RelayCommand(() => ++this.Value)); }
        }

        #endregion // Commands

        #region Fields

        RelayCommand _decrementCommand;
        RelayCommand _incrementCommand;
        int _value;

        #endregion // Fields
    }
}