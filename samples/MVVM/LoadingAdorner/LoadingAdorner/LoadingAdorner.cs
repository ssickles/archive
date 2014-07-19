﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Documents;
using System.Windows;
using System.Windows.Media;
using System.Threading;

namespace LoadingAdorner
{
    /// <summary>
    /// Adorner that disables all controls that fall under it
    /// </summary>
    public class LoadingAdorner : Adorner
    {
        #region Properties

        /// <summary>
        /// Gets or sets the color to paint
        /// </summary>
        public Brush Color
        {
            get { return (Brush)GetValue(ColorProperty); }
            set { SetValue(ColorProperty, value); }
        }

        /// <summary>
        /// Gets or sets the color to paint
        /// </summary>
        public static readonly DependencyProperty ColorProperty =
            DependencyProperty.Register("Color", typeof(Brush), typeof(LoadingAdorner),
            new PropertyMetadata((Brush)new BrushConverter().ConvertFromString("#7F4047F7")));


        /// <summary>
        /// Gets or sets the border 
        /// </summary>
        public Pen Border
        {
            get { return (Pen)GetValue(BorderProperty); }
            set { SetValue(BorderProperty, value); }
        }

        /// <summary>
        /// Gets or sets the border 
        /// </summary>
        public static readonly DependencyProperty BorderProperty =
            DependencyProperty.Register("Border", typeof(Pen), typeof(LoadingAdorner),
            new UIPropertyMetadata(new Pen(Brushes.Gray, 1)));

        //the start point where to start drawing
        private static readonly Point startPoint =
            new Point(0, 0);

        /// <summary>
        /// Gets or sets the text to display 
        /// </summary>
        public string OverlayedText
        {
            get { return (string)GetValue(OverlayedTextProperty); }
            set { SetValue(OverlayedTextProperty, value); }
        }

        /// <summary>
        /// Gets or sets the text to display 
        /// </summary>
        public static readonly DependencyProperty OverlayedTextProperty =
            DependencyProperty.Register("OverlayedText", typeof(string), typeof(LoadingAdorner), new UIPropertyMetadata(""));

        /// <summary>
        /// Gets or sets the foreground to use for the text
        /// </summary>
        public Brush ForeGround
        {
            get { return (Brush)GetValue(ForeGroundProperty); }
            set { SetValue(ForeGroundProperty, value); }
        }

        /// <summary>
        /// Gets or sets the foreground to use for the text
        /// </summary>
        public static readonly DependencyProperty ForeGroundProperty =
            DependencyProperty.Register("ForeGround", typeof(Brush), typeof(LoadingAdorner),
            new UIPropertyMetadata(Brushes.Black));


        /// <summary>
        /// Gets or sets the font size for the text
        /// </summary>
        public double FontSize
        {
            get { return (double)GetValue(FontSizeProperty); }
            set { SetValue(FontSizeProperty, value); }
        }

        /// <summary>
        /// Gets or sets the font size for the text
        /// </summary>
        public static readonly DependencyProperty FontSizeProperty =
            DependencyProperty.Register("FontSize", typeof(double), typeof(LoadingAdorner), new UIPropertyMetadata(10.0));


        /// <summary>
        /// Gets or sets the Typeface for the text
        /// </summary>
        public Typeface Typeface
        {
            get { return (Typeface)GetValue(TypefaceProperty); }
            set { SetValue(TypefaceProperty, value); }
        }

        /// <summary>
        /// Gets or sets the Typeface for the text
        /// </summary>
        public static readonly DependencyProperty TypefaceProperty =
            DependencyProperty.Register("Typeface", typeof(Typeface), typeof(LoadingAdorner),
            new UIPropertyMetadata(new Typeface("Verdana")));



        #endregion

        /// <summary>
        /// Constructor for the adorner
        /// </summary>
        /// <param name="adornerElement">The element to be adorned</param>
        public LoadingAdorner(UIElement adornerElement)
            : base(adornerElement)
        { }


        /// <summary>
        /// Called to draw on screen
        /// </summary>
        /// <param name="drawingContext">The drawind context in which we can draw</param>
        protected override void OnRender(System.Windows.Media.DrawingContext drawingContext)
        {
            FormattedText text = new FormattedText(OverlayedText, Thread.CurrentThread.CurrentUICulture,
                FlowDirection.LeftToRight, Typeface, FontSize, ForeGround);
            drawingContext.DrawText(text, startPoint);
            drawingContext.DrawRectangle(Color, Border, new Rect(startPoint, DesiredSize));
            base.OnRender(drawingContext);
        }
    }
}
