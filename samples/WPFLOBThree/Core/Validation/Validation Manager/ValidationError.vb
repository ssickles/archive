
Namespace Validation

    Public Class ValidationError

#Region " Declarations "

        Private _objBrokenRule As IValidationRuleMethod

#End Region

#Region " Properties "

        Public ReadOnly Property BrokenRule() As IValidationRuleMethod
            Get
                Return _objBrokenRule
            End Get
        End Property

        Public ReadOnly Property PropertyName() As String
            Get
                Return Me.BrokenRule.RuleEventArgs.PropertyName
            End Get
        End Property

        Public ReadOnly Property RuleName() As String
            Get
                Return Me.BrokenRule.RuleName
            End Get
        End Property

#End Region

#Region " Constructor "

        Public Sub New(ByVal obj As IValidationRuleMethod)
            _objBrokenRule = obj

        End Sub

#End Region

    End Class

End Namespace
