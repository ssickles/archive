Imports System
Imports System.Collections
Imports System.Windows
Imports System.Windows.Controls
Imports System.Windows.Documents
Imports System.Windows.Media

''' <summary> 
''' Renders a TextBlock in an element's adorner layer. 
''' </summary> 
''' <remarks>
''' Thanks for Josh Smith for this great code:
''' http://joshsmithonwpf.wordpress.com/2007/08/25/rendering-text-in-the-adorner-layer/
''' </remarks>
Public Class TextBlockAdorner
    Inherits Adorner

#Region " Declarations "

    Private _logicalChildren As ArrayList
    Private _textBlock As TextBlock

#End Region

#Region "Constructor"

    Public Sub New(ByVal adornedElement As UIElement, ByVal textBlock As TextBlock)
        MyBase.New(adornedElement)
        _textBlock = textBlock
        ' Register the TextBlock with the element tree so that 
        ' it will be rendered, and can inherit DP values. 
        MyBase.AddLogicalChild(_textBlock)
        MyBase.AddVisualChild(_textBlock)

    End Sub

#End Region

#Region "Measure/Arrange"

    ''' <summary> 
    ''' Positions and sizes the TextBlock. 
    ''' </summary> 
    ''' <param name="finalSize">The actual size of the TextBlock.</param> 
    Protected Overloads Overrides Function ArrangeOverride(ByVal finalSize As Size) As Size

        Dim location As New Point(0, 0)
        Dim rect As New Rect(location, finalSize)
        _textBlock.Arrange(rect)
        Return finalSize

    End Function

    ''' <summary> 
    ''' Allows the TextBlock to determine how big it wants to be. 
    ''' </summary> 
    ''' <param name="constraint">A limiting size for the TextBlock.</param> 
    Protected Overloads Overrides Function MeasureOverride(ByVal constraint As Size) As Size
        _textBlock.Measure(constraint)
        Return _textBlock.DesiredSize

    End Function

#End Region

#Region "Visual Children"

    ''' <summary> 
    ''' Required for the TextBlock to be rendered. 
    ''' </summary> 
    Protected Overloads Overrides ReadOnly Property VisualChildrenCount() As Integer
        Get
            Return 1
        End Get
    End Property

    ''' <summary> 
    ''' Required for the TextBlock to be rendered. 
    ''' </summary> 
    Protected Overloads Overrides Function GetVisualChild(ByVal index As Integer) As Visual

        If index <> 0 Then
            Throw New ArgumentOutOfRangeException("index")
        End If

        Return _textBlock

    End Function

#End Region

#Region "Logical Children"

    ''' <summary> 
    ''' Required for the TextBlock to inherit property values 
    ''' from the logical tree, such as FontSize. 
    ''' </summary> 
    Protected Overloads Overrides ReadOnly Property LogicalChildren() As IEnumerator
        Get

            If _logicalChildren Is Nothing Then
                _logicalChildren = New ArrayList()
                _logicalChildren.Add(_textBlock)
            End If

            Return _logicalChildren.GetEnumerator()
        End Get
    End Property

#End Region

End Class
