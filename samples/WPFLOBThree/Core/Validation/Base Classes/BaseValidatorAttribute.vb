
Namespace Validation

    Public MustInherit Class BaseValidatorAttribute
        Inherits Attribute

#Region " Declarations "

        Protected _strCustomMessage As String = String.Empty
        Protected _strPropertyFriendlyName As String = String.Empty
        Protected _strRuleSet As String = String.Empty

#End Region

#Region " Properties "

        Public Property CustomMessage() As String
            Get
                Return _strCustomMessage
            End Get
            Set(ByVal Value As String)
                _strCustomMessage = Value
            End Set
        End Property

        Public Property PropertyFriendlyName() As String
            Get
                Return _strPropertyFriendlyName
            End Get
            Set(ByVal Value As String)
                _strPropertyFriendlyName = Value
            End Set
        End Property

        Public Property RuleSet() As String
            Get
                Return _strRuleSet
            End Get
            Set(ByVal Value As String)
                _strRuleSet = Value
            End Set
        End Property

#End Region

#Region " Abstract Methods "

        Public MustOverride Function Create(ByVal strPropertyName As String) As Validator

#End Region

    End Class

End Namespace
