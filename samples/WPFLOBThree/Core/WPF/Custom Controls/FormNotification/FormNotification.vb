Imports System.ComponentModel

Namespace WPF

    <TemplatePart(Name:="PART_Expander", Type:=GetType(Expander))> _
    Public NotInheritable Class FormNotification
        Inherits System.Windows.Controls.Control

#Region " Declarations "

        Private WithEvents _objErrorsExpander As Expander
        Private WithEvents _objErrorsExpanderAdornerLayer As AdornerLayer
        Private WithEvents _objExpandedTimer As System.Timers.Timer
        Private WithEvents _objTextBlockAdorner As TextBlockAdorner
        Private Delegate Sub ExpanderDelegate()

#End Region

#Region " Shared Properties "

        Public Shared AutoCollapseTimeoutProperty As DependencyProperty = DependencyProperty.Register("AutoCollapseTimeout", GetType(Double), GetType(FormNotification), New PropertyMetadata(2.0), New ValidateValueCallback(AddressOf IsAutoCollapseTimeoutValid))
        Public Shared ErrorHeaderForegroundProperty As DependencyProperty = DependencyProperty.Register("ErrorHeaderForeground", GetType(Brush), GetType(FormNotification), New PropertyMetadata(New SolidColorBrush(Color.FromArgb(255, 208, 0, 0))))
        Public Shared ErrorHeaderTextProperty As DependencyProperty = DependencyProperty.Register("ErrorHeaderText", GetType(String), GetType(FormNotification), New PropertyMetadata("Edit Errors"))
        Public Shared ErrorMessageProperty As DependencyProperty = DependencyProperty.Register("ErrorMessage", GetType(String), GetType(FormNotification), New PropertyMetadata(String.Empty, New PropertyChangedCallback(AddressOf OnErrorMessageChanged)))
        Public Shared ErrorPopUpBackgroundProperty As DependencyProperty = DependencyProperty.Register("ErrorPopUpBackground", GetType(Brush), GetType(FormNotification), New PropertyMetadata(New SolidColorBrush(Color.FromArgb(255, 253, 240, 151))))
        Public Shared ErrorPopUpForegroundProperty As DependencyProperty = DependencyProperty.Register("ErrorPopUpForeground", GetType(Brush), GetType(FormNotification), New PropertyMetadata(New SolidColorBrush(Colors.Black)))
        Public Shared NotificationMessageBackgroundProperty As DependencyProperty = DependencyProperty.Register("NotificationMessageBackground", GetType(Brush), GetType(FormNotification), New PropertyMetadata(New SolidColorBrush(Colors.LightGray)))
        Public Shared NotificationMessageForegroundProperty As DependencyProperty = DependencyProperty.Register("NotificationMessageForeground", GetType(Brush), GetType(FormNotification), New PropertyMetadata(New SolidColorBrush(Colors.Blue)))
        Public Shared NotificationMessageProperty As DependencyProperty = DependencyProperty.Register("NotificationMessage", GetType(String), GetType(FormNotification), New PropertyMetadata(String.Empty, New PropertyChangedCallback(AddressOf OnNotificationMessageChanged)))
        Public Shared WatermarkMessageBackgroundProperty As DependencyProperty = DependencyProperty.Register("WatermarkMessageBackground", GetType(Brush), GetType(FormNotification))
        Public Shared WatermarkMessageForegroundProperty As DependencyProperty = DependencyProperty.Register("WatermarkMessageForeground", GetType(Brush), GetType(FormNotification), New PropertyMetadata(New SolidColorBrush(Colors.Gray)))
        Public Shared WatermarkMessageProperty As DependencyProperty = DependencyProperty.Register("WatermarkMessage", GetType(String), GetType(FormNotification), New PropertyMetadata(String.Empty))

#End Region

#Region " Properties "

        <Category("Custom"), Description("Number of seconds the error pop remains expanded after the mouse leaves.  Default is 2 seconds.  Example 2.5 is 2 1/2 seconds.  Valid range is 0 - 100.  Zero means Auto Collapse is turned off.")> _
        Public Property AutoCollapseTimeout() As Double
            Get
                Return CType(GetValue(AutoCollapseTimeoutProperty), Double)
            End Get
            Set(ByVal value As Double)
                SetValue(AutoCollapseTimeoutProperty, value)

                If _objExpandedTimer IsNot Nothing Then
                    _objExpandedTimer.Interval = value
                End If

            End Set
        End Property

        <Category("Custom"), Description("Error header text foreground brush.")> _
        Public Property ErrorHeaderForeground() As Brush
            Get
                Return CType(GetValue(ErrorHeaderForegroundProperty), Brush)
            End Get
            Set(ByVal value As Brush)
                SetValue(ErrorHeaderForegroundProperty, value)
            End Set
        End Property

        <Category("Custom"), Description("Error header text that is displayed when there is an error on the form.  Setting this text property causes it to be displayed.  Normally this property is data bound to the Error property on an object data implements the IDataErrorInfo interface.")> _
        Public Property ErrorHeaderText() As String
            Get
                Return CType(GetValue(ErrorHeaderTextProperty), String)
            End Get
            Set(ByVal value As String)
                SetValue(ErrorHeaderTextProperty, value)
            End Set
        End Property

        <Category("Custom"), Description("Error message that is displayed in the expander pop up.")> _
        Public Property ErrorMessage() As String
            Get
                Return CType(GetValue(ErrorMessageProperty), String)
            End Get
            Set(ByVal value As String)
                SetValue(ErrorMessageProperty, value)
            End Set
        End Property

        <Category("Custom"), Description("Error message pop up background brush.")> _
        Public Property ErrorPopUpBackground() As Brush
            Get
                Return CType(GetValue(ErrorPopUpBackgroundProperty), Brush)
            End Get
            Set(ByVal value As Brush)
                SetValue(ErrorPopUpBackgroundProperty, value)
            End Set
        End Property

        <Category("Custom"), Description("Error message pop up forground brush.")> _
        Public Property ErrorPopUpForeground() As Brush
            Get
                Return CType(GetValue(ErrorPopUpForegroundProperty), Brush)
            End Get
            Set(ByVal value As Brush)
                SetValue(ErrorPopUpForegroundProperty, value)
            End Set
        End Property

        <Category("Custom"), Description("Notification message text.  If this property is set and their is no error message text, this text will be displayed.  This is normally set in after successful write or delete operations by the host form.")> _
        Public Property NotificationMessage() As String
            Get
                Return CType(GetValue(NotificationMessageProperty), String)
            End Get
            Set(ByVal value As String)
                SetValue(NotificationMessageProperty, value)
            End Set
        End Property

        <Category("Custom"), Description("Notification message pop up background brush.")> _
        Public Property NotificationMessageBackground() As Brush
            Get
                Return CType(GetValue(NotificationMessageBackgroundProperty), Brush)
            End Get
            Set(ByVal value As Brush)
                SetValue(NotificationMessageBackgroundProperty, value)
            End Set
        End Property

        <Category("Custom"), Description("Notification message pop up foreground brush.")> _
        Public Property NotificationMessageForeground() As Brush
            Get
                Return CType(GetValue(NotificationMessageForegroundProperty), Brush)
            End Get
            Set(ByVal value As Brush)
                SetValue(NotificationMessageForegroundProperty, value)
            End Set
        End Property

        <Category("Custom"), Description("Watermark text message.  This is displayed if there is no error message text or notification message text.")> _
        Public Property WatermarkMessage() As String
            Get
                Return CType(GetValue(WatermarkMessageProperty), String)
            End Get
            Set(ByVal value As String)
                SetValue(WatermarkMessageProperty, value)
            End Set
        End Property

        <Category("Custom"), Description("Watermark message pop up background brush.")> _
        Public Property WatermarkMessageBackground() As Brush
            Get
                Return CType(GetValue(WatermarkMessageBackgroundProperty), Brush)
            End Get
            Set(ByVal value As Brush)
                SetValue(WatermarkMessageBackgroundProperty, value)
            End Set
        End Property

        <Category("Custom"), Description("Watermark message pop up foreground brush.")> _
        Public Property WatermarkMessageForeground() As Brush
            Get
                Return CType(GetValue(WatermarkMessageForegroundProperty), Brush)
            End Get
            Set(ByVal value As Brush)
                SetValue(WatermarkMessageForegroundProperty, value)
            End Set
        End Property

#End Region

#Region " Constructor and Initializer "

        Shared Sub New()
            DefaultStyleKeyProperty.OverrideMetadata(GetType(FormNotification), New FrameworkPropertyMetadata(GetType(FormNotification)))

        End Sub

        Private Sub FormNotification_Initialized(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Initialized
            _objExpandedTimer = New System.Timers.Timer(AutoCollapseTimeout * 1000)
            _objExpandedTimer.Enabled = False
            _objExpandedTimer.AutoReset = False

        End Sub

#End Region

#Region " Methods "

        ''' <summary>
        ''' When the expander collapses, need to remove the TextBlock adorner
        ''' </summary>
        Private Sub _objErrorsExpander_Collapsed(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles _objErrorsExpander.Collapsed

            If _objErrorsExpanderAdornerLayer IsNot Nothing Then
                _objErrorsExpanderAdornerLayer.Remove(_objTextBlockAdorner)
                _objTextBlockAdorner = Nothing
                _objErrorsExpanderAdornerLayer = Nothing
            End If

        End Sub


        ''' <summary>
        ''' When the expander expands, need to put the error message in the
        ''' adorner layer because the controls below it, may have their own
        ''' adorner validation error messages, this places the expander
        ''' popup on top of all other adorder layer elements. 
        ''' </summary>
        Private Sub _objErrorsExpander_Expanded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles _objErrorsExpander.Expanded

            'this forces the ErrorMessage to be reread from it's source
            Me.InvalidateProperty(ErrorMessageProperty)

            Dim objExpanderGrid As Grid = FindChilden.FindVisualChild(Of Grid)(_objErrorsExpander)
            '
            _objErrorsExpanderAdornerLayer = AdornerLayer.GetAdornerLayer(objExpanderGrid)

            Dim aryDelimiter() As String = {vbCrLf}
            Dim aryErrorMessages() As String = Me.ErrorMessage.Split(aryDelimiter, StringSplitOptions.RemoveEmptyEntries)
            Array.Sort(aryErrorMessages)

            Dim txt As New TextBlock
            With txt
                .Width = Me.Width
                .TextWrapping = TextWrapping.Wrap
                .Text = String.Join(vbCrLf & vbCrLf, aryErrorMessages)
                .Padding = New Thickness(5)
                .Foreground = Me.ErrorPopUpForeground
                .Background = Me.ErrorPopUpBackground
                .BitmapEffect = New System.Windows.Media.Effects.DropShadowBitmapEffect
                '
                'need to move the TextBlock down below the expander and indent a little
                Dim obj As New TranslateTransform(5, _objErrorsExpander.ActualHeight + 2)
                .RenderTransform = obj

            End With
            '
            _objTextBlockAdorner = New TextBlockAdorner(objExpanderGrid, txt)
            '
            _objErrorsExpanderAdornerLayer.Add(_objTextBlockAdorner)

        End Sub

        Private Sub _objErrorsExpander_MouseEnter(ByVal sender As Object, ByVal e As System.Windows.Input.MouseEventArgs) Handles _objErrorsExpander.MouseEnter
            _objExpandedTimer.Stop()

        End Sub

        Private Sub _objErrorsExpander_MouseLeave(ByVal sender As Object, ByVal e As System.Windows.Input.MouseEventArgs) Handles _objErrorsExpander.MouseLeave

            If _objExpandedTimer.Interval > 0 Then
                _objExpandedTimer.Start()
            End If

        End Sub

        ''' <summary>
        ''' Since WPF uses the STA threading model, another thread like a timer
        ''' can not update the UI.  WPF provides a very simple technique for
        ''' updating the UI from another thread.
        ''' </summary>
        Private Sub _objExpandedTimer_Elapsed(ByVal sender As Object, ByVal e As System.Timers.ElapsedEventArgs) Handles _objExpandedTimer.Elapsed
            Dispatcher.Invoke(Windows.Threading.DispatcherPriority.Normal, New ExpanderDelegate(AddressOf CloseExpander))

        End Sub

        Private Sub _objTextBlockAdorner_MouseEnter(ByVal sender As Object, ByVal e As System.Windows.Input.MouseEventArgs) Handles _objTextBlockAdorner.MouseEnter
            _objExpandedTimer.Stop()

        End Sub

        Private Sub _objTextBlockAdorner_MouseLeave(ByVal sender As Object, ByVal e As System.Windows.Input.MouseEventArgs) Handles _objTextBlockAdorner.MouseLeave

            If _objExpandedTimer.Interval > 0 Then
                _objExpandedTimer.Start()
            End If

        End Sub

        ''' <summary>
        ''' This method is called when the error message or notification message
        ''' property values are set and when the Time.Elapsed event fires.
        ''' This ensures that the expander region is closed and the adorner layer
        ''' is removed.
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub CloseExpander()

            If _objErrorsExpander IsNot Nothing AndAlso _objErrorsExpander.IsExpanded Then
                _objErrorsExpander.IsExpanded = False
            End If

        End Sub

        ''' <summary>
        ''' This is the call back that gets called when the ErrorMessage
        ''' dependency property is changed.
        ''' </summary>
        Private Shared Sub OnErrorMessageChanged(ByVal d As DependencyObject, ByVal e As DependencyPropertyChangedEventArgs)

            Dim obj As FormNotification = DirectCast(d, FormNotification)

            If Not String.IsNullOrEmpty(e.NewValue.ToString) Then
                obj.NotificationMessage = String.Empty
            End If

            obj.CloseExpander()

        End Sub

        ''' <summary>
        ''' This is the call back that gets called when the NotificationMessage
        ''' dependency property is changed.
        ''' </summary>
        Private Shared Sub OnNotificationMessageChanged(ByVal d As DependencyObject, ByVal e As DependencyPropertyChangedEventArgs)
            DirectCast(d, FormNotification).CloseExpander()

        End Sub

        ''' <summary>
        ''' This method is called by the WPF Dependency Property system when the
        ''' AutoCollapseTimeout value is set.
        ''' </summary>
        Public Shared Function IsAutoCollapseTimeoutValid(ByVal value As Object) As Boolean

            Dim dbl As Double = CType(value, Double)

            If dbl < 0 OrElse dbl > 100 Then
                Return False

            Else
                Return True
            End If

        End Function

        ''' <summary>
        ''' This is where you can get a reference to a control 
        ''' inside the control template.  Notice the PART_ naming convention.
        ''' </summary>
        Public Overrides Sub OnApplyTemplate()
            MyBase.OnApplyTemplate()
            '
            'Each object that you are getting a reference to here is also
            'listed in a TemplatePart attribute on the class.
            '
            '<TemplatePart(Name:="PART_Expander", Type:=GetType(Expander))> _
            '
            _objErrorsExpander = CType(GetTemplateChild("PART_Expander"), Expander)

        End Sub

#End Region

    End Class

End Namespace
