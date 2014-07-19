Imports System.ComponentModel

Namespace WPF

    Public Class CustomToolBarButton
        Inherits System.Windows.Controls.Button

#Region " Shared Declarations "

        Public Shared ButtonLayoutProperty As DependencyProperty = DependencyProperty.Register("ButtonLayout", GetType(Orientation), GetType(CustomToolBarButton), New PropertyMetadata(Orientation.Horizontal))
        Public Shared ButtonPressedBackgroundProperty As DependencyProperty = DependencyProperty.Register("ButtonPressedBackground", GetType(Brush), GetType(CustomToolBarButton))
        Public Shared ButtonPressedBorderProperty As DependencyProperty = DependencyProperty.Register("ButtonPressedBorder", GetType(Brush), GetType(CustomToolBarButton))
        Public Shared ButtonTextProperty As DependencyProperty = DependencyProperty.Register("ButtonText", GetType(String), GetType(CustomToolBarButton))
        Public Shared DisabledButtonImageProperty As DependencyProperty = DependencyProperty.Register("DisabledButtonImage", GetType(String), GetType(CustomToolBarButton))
        Public Shared EnabledButtonImageProperty As DependencyProperty = DependencyProperty.Register("EnabledButtonImage", GetType(String), GetType(CustomToolBarButton))
        Public Shared MouseOverBackgroundProperty As DependencyProperty = DependencyProperty.Register("MouseOverBackground", GetType(Brush), GetType(CustomToolBarButton))
        Public Shared MouseOverBorderProperty As DependencyProperty = DependencyProperty.Register("MouseOverBorder", GetType(Brush), GetType(CustomToolBarButton), New PropertyMetadata(New SolidColorBrush(Colors.Black)))
        Public Shared MouseOverForegroundProperty As DependencyProperty = DependencyProperty.Register("MouseOverForeground", GetType(Brush), GetType(CustomToolBarButton))
        Public Shared ShowButtonImageProperty As DependencyProperty = DependencyProperty.Register("ShowButtonImage", GetType(Boolean), GetType(CustomToolBarButton), New PropertyMetadata(True))
        Public Shared ShowButtonTextProperty As DependencyProperty = DependencyProperty.Register("ShowButtonText", GetType(Boolean), GetType(CustomToolBarButton), New PropertyMetadata(False))

#End Region

#Region " Properties "

        <Category("Custom"), Description("This sets the position of the text in relation to the button image.")> Public Property ButtonLayout() As Orientation
            Get
                Return CType(GetValue(ButtonLayoutProperty), Orientation)
            End Get
            Set(ByVal value As Orientation)
                SetValue(ButtonLayoutProperty, value)
            End Set
        End Property

        <Category("Custom"), Description("Button pressed background brush.")> Public Property ButtonPressedBackground() As Brush
            Get
                Return CType(GetValue(ButtonPressedBackgroundProperty), Brush)
            End Get
            Set(ByVal value As Brush)
                SetValue(ButtonPressedBackgroundProperty, value)
            End Set
        End Property

        <Category("Custom"), Description("Button pressed border brush.")> Public Property ButtonPressedBorder() As Brush
            Get
                Return CType(GetValue(ButtonPressedBorderProperty), Brush)
            End Get
            Set(ByVal value As Brush)
                SetValue(ButtonPressedBorderProperty, value)
            End Set
        End Property

        <Category("Custom"), Description("Text for the button.")> Public Property ButtonText() As String
            Get
                Return CType(GetValue(ButtonTextProperty), String)
            End Get
            Set(ByVal value As String)
                SetValue(ButtonTextProperty, value)
            End Set
        End Property

        ''' <summary>
        ''' I did this since we have derived from button, but don't use the
        ''' content property like other buttons.  The control template for
        ''' this control is the content of this button.  Doing this, just
        ''' prevents this property from showing up in the GUI designers.
        ''' </summary>
        <System.ComponentModel.Browsable(False)> Public Shadows Property Content() As Object
            Get
                Return MyBase.Content
            End Get
            Set(ByVal value As Object)
                MyBase.Content = value
            End Set
        End Property

        <Category("Custom"), Description("Image to display when the button is disabled.")> Public Property DisabledButtonImage() As String
            Get
                Return CType(GetValue(DisabledButtonImageProperty), String)
            End Get
            Set(ByVal value As String)
                SetValue(DisabledButtonImageProperty, value)
            End Set
        End Property

        <Category("Custom"), Description("Image to display when the button is enabled.")> Public Property EnabledButtonImage() As String
            Get
                Return CType(GetValue(EnabledButtonImageProperty), String)
            End Get
            Set(ByVal value As String)
                SetValue(EnabledButtonImageProperty, value)
            End Set
        End Property

        <Category("Custom"), Description("Mouse over background brush.")> Public Property MouseOverBackground() As Brush
            Get
                Return CType(GetValue(MouseOverBackgroundProperty), Brush)
            End Get
            Set(ByVal value As Brush)
                SetValue(MouseOverBackgroundProperty, value)
            End Set
        End Property

        <Category("Custom"), Description("Mouse over border brush.")> Public Property MouseOverBorder() As Brush
            Get
                Return CType(GetValue(MouseOverBorderProperty), Brush)
            End Get
            Set(ByVal value As Brush)
                SetValue(MouseOverBorderProperty, value)
            End Set
        End Property

        <Category("Custom"), Description("Mouse over foreground text brush.")> Public Property MouseOverForeground() As Brush
            Get
                Return CType(GetValue(MouseOverForegroundProperty), Brush)
            End Get
            Set(ByVal value As Brush)
                SetValue(MouseOverForegroundProperty, value)
            End Set
        End Property

        <Category("Custom"), Description("Display the image on the button.")> Public Property ShowButtonImage() As Boolean
            Get
                Return CType(GetValue(ShowButtonImageProperty), Boolean)
            End Get
            Set(ByVal value As Boolean)
                SetValue(ShowButtonImageProperty, value)
            End Set
        End Property

        <Category("Custom"), Description("Display the text on the button.")> Public Property ShowButtonText() As Boolean
            Get
                Return CType(GetValue(ShowButtonTextProperty), Boolean)
            End Get
            Set(ByVal value As Boolean)
                SetValue(ShowButtonTextProperty, value)
            End Set
        End Property

#End Region

#Region " Constructor "

        Shared Sub New()
            DefaultStyleKeyProperty.OverrideMetadata(GetType(CustomToolBarButton), New FrameworkPropertyMetadata(GetType(CustomToolBarButton)))

        End Sub

#End Region

    End Class

End Namespace
