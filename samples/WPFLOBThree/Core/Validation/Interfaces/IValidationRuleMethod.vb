
Namespace Validation

    Public Interface IValidationRuleMethod

        ReadOnly Property RuleEventArgs() As RuleEventArgs

        ReadOnly Property RuleName() As String

        Function Invoke(ByVal target As Object) As Boolean

    End Interface

End Namespace
