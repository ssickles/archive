
Namespace Validation

    Public Class RegulatExpressionRuleEventArgs
        Inherits RuleEventArgs

#Region " Declarations "

        Private _bolIsRequired As Boolean = False
        Private _enumRegularExpressionPatternType As RegularExpressionPatternType = RegularExpressionPatternType.Custom
        Private _strCustomRegularExpressionPattern As String = String.Empty

#End Region

#Region " Properties "

        Public Property CustomRegularExpressionPattern() As String
            Get
                Return _strCustomRegularExpressionPattern
            End Get
            Set(ByVal Value As String)

                If Value = String.Empty Then
                    _strCustomRegularExpressionPattern = String.Empty
                    Exit Property
                End If

                If Me.RegularExpressionPatternType <> Validation.RegularExpressionPatternType.Custom Then
                    Throw New ArgumentException("Before setting a custom pattern, the Pattern Type must be custom.")
                End If

                'will blow up if expression pattern is invalid
                Dim objRegEx As New System.Text.RegularExpressions.Regex(Value)
                _strCustomRegularExpressionPattern = Value
            End Set
        End Property

        Public Property IsRequired() As Boolean
            Get
                Return _bolIsRequired
            End Get
            Set(ByVal Value As Boolean)
                _bolIsRequired = Value
            End Set
        End Property

        Public Property RegularExpressionPatternType() As RegularExpressionPatternType
            Get
                Return _enumRegularExpressionPatternType
            End Get
            Set(ByVal Value As RegularExpressionPatternType)
                _enumRegularExpressionPatternType = Value
            End Set
        End Property

#End Region

#Region " Constructor "

        Public Sub New(ByVal e As RegularExpressionValidatorAttribute, ByVal strPropertyName As String)
            MyBase.New(strPropertyName)
            MyBase.PropertyFriendlyName = e.PropertyFriendlyName
            MyBase.CustomMessage = e.CustomMessage
            MyBase.RuleSet = e.RuleSet
            _enumRegularExpressionPatternType = e.RegularExpressionPatternType
            _strCustomRegularExpressionPattern = e.CustomRegularExpressionPattern

        End Sub

        Public Sub New(ByVal strPropertyName As String, ByVal enumRegularExpressionPatternType As RegularExpressionPatternType, ByVal bolIsRequired As Boolean)
            MyBase.New(strPropertyName)
            _enumRegularExpressionPatternType = enumRegularExpressionPatternType
            _bolIsRequired = bolIsRequired

        End Sub

        Public Sub New(ByVal strPropertyName As String, ByVal strCustomRegularExpressionPattern As String, ByVal bolIsRequired As Boolean)
            MyBase.New(strPropertyName)
            _enumRegularExpressionPatternType = Validation.RegularExpressionPatternType.Custom
            _strCustomRegularExpressionPattern = strCustomRegularExpressionPattern
            _bolIsRequired = bolIsRequired

            'will blow up if expression pattern is invalid
            Dim objRegEx As New System.Text.RegularExpressions.Regex(strCustomRegularExpressionPattern)

        End Sub

#End Region

    End Class

End Namespace
