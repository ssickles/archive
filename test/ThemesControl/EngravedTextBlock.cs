using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ThemesControl
{
    public class EngravedTextBlock : Control
    {
        public static readonly DependencyProperty BackColorProperty;
        public static readonly DependencyProperty EmbossDepthProperty;
        public static readonly DependencyProperty TextProperty;
        public static readonly DependencyProperty TextWrappingProperty;

        static EngravedTextBlock()
        {
            BackColorProperty = DependencyProperty.Register("BackColor", typeof(Brush), typeof(EngravedTextBlock), new FrameworkPropertyMetadata(SystemColors.ControlTextBrush));
            EmbossDepthProperty = DependencyProperty.Register("EmbossDepth", typeof(Thickness), typeof(EngravedTextBlock), new FrameworkPropertyMetadata(new Thickness(1, 1, 0, 0), FrameworkPropertyMetadataOptions.Inherits));
            TextProperty = TextBlock.TextProperty.AddOwner(typeof(EngravedTextBlock), new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.Inherits));
            TextWrappingProperty = TextBlock.TextWrappingProperty.AddOwner(typeof(EngravedTextBlock), new FrameworkPropertyMetadata(TextWrapping.Wrap, FrameworkPropertyMetadataOptions.Inherits));

            DefaultStyleKeyProperty.OverrideMetadata(typeof(EngravedTextBlock), new FrameworkPropertyMetadata(typeof(EngravedTextBlock)));
        }

        public Brush BackColor
        {
            get { return (Brush)GetValue(BackColorProperty); }
            set { SetValue(BackColorProperty, value); }
        }

        public Thickness EmbossDepth
        {
            get { return (Thickness)GetValue(EmbossDepthProperty); }
            set { SetValue(EmbossDepthProperty, value); }
        }

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public TextWrapping TextWrapping
        {
            get { return (TextWrapping)GetValue(TextWrappingProperty); }
            set { SetValue(TextWrappingProperty, value); }
        }
    }
}
