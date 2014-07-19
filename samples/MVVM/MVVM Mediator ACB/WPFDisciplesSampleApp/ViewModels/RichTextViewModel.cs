using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Documents;
using WPFDisciples.Common;
using AttachedCommandBehavior;

namespace WPFDisciples.ViewModels
{
    /// <summary>
    /// ViewModel for the RichText View
    /// </summary>
    public class RichTextViewModel : BaseViewModel
    {
        /// <summary>
        /// Command to close the view
        /// </summary>
        public SimpleCommand Close { get; private set; }

        string text;
        /// <summary>
        /// Gets or sets the Text entered by the user
        /// </summary>
        public string Text
        {
            get { return text; }
            set
            {
                text = value;
                OnPropertyChanged("Text");
            }
        }

        public RichTextViewModel()
        {
            Close = new SimpleCommand
            {
                //Publish a message with the new Text
                ExecuteDelegate = x => Mediator.NotifyColleagues(ViewModelMessages.HideRichText, RichTextBoxHelper.GetText((FlowDocument)x))
            };

            //Receive the Text from the previous form
            Mediator.Register(x => Text = x == null ? "" : x.ToString(), ViewModelMessages.ShowRichText);
        }
    }
}
