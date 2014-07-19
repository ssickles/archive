
Namespace Validation

    <AttributeUsage(AttributeTargets.Property, AllowMultiple:=False, Inherited:=False)> _
    Public NotInheritable Class RegularExpressionValidatorAttribute
        Inherits BaseValidatorAttribute

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

        Public Sub New(ByVal enumRegularExpressionPatternType As RegularExpressionPatternType, ByVal bolIsRequired As Boolean)
            _enumRegularExpressionPatternType = enumRegularExpressionPatternType
            _bolIsRequired = bolIsRequired

        End Sub

        Public Sub New(ByVal strCustomRegularExpressionPattern As String, ByVal bolIsRequired As Boolean)
            _enumRegularExpressionPatternType = Validation.RegularExpressionPatternType.Custom
            _bolIsRequired = bolIsRequired
            _strCustomRegularExpressionPattern = strCustomRegularExpressionPattern

            'will blow up if expression pattern is invalid
            Dim objRegEx As New System.Text.RegularExpressions.Regex(strCustomRegularExpressionPattern)

        End Sub

#End Region

#Region " Methods "

        Public Overrides Function Create(ByVal strPropertyName As String) As Validator
            Return New Validator(AddressOf Validation.StringValidationRules.RegularExpressionRule, New RegulatExpressionRuleEventArgs(Me, strPropertyName))

        End Function

#End Region

    End Class

End Namespace
