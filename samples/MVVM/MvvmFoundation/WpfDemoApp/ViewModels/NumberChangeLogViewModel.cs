using System.Collections.ObjectModel;
using MvvmFoundation.Wpf;

namespace WpfDemoApp.ViewModels
{
    public class NumberChangeLogViewModel
    {
        public NumberChangeLogViewModel()
        {
            this.Number = new NumberViewModel();
            this.ChangeLog = new ObservableCollection<string>();

            _observer =
                new PropertyObserver<NumberViewModel>(this.Number)
                   .RegisterHandler(n => n.Value, n => Log("Value: " + n.Value))
                   .RegisterHandler(n => n.IsNegative, this.AppendIsNegative)
                   .RegisterHandler(n => n.IsEven, this.AppendIsEven);
        }

        void AppendIsNegative(NumberViewModel number)
        {
            if (number.IsNegative)
                this.Log("\tNumber is now negative");
            else
                this.Log("\tNumber is now positive");
        }

        void AppendIsEven(NumberViewModel number)
        {
            if (number.IsEven)
                this.Log("\tNumber is now even");
            else
                this.Log("\tNumber is now odd");
        }

        void Log(string item)
        {
            this.ChangeLog.Add(item);
            App.Messenger.NotifyColleagues(App.MSG_LOG_APPENDED);
        }

        public ObservableCollection<string> ChangeLog { get; private set; }
        public NumberViewModel Number { get; private set; }

        readonly PropertyObserver<NumberViewModel> _observer;
    }
}