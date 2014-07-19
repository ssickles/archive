Namespace Validation

    Public Delegate Function RuleHandler(ByVal target As Object, ByVal e As RuleEventArgs) As Boolean

    Public Delegate Function RuleHandler(Of T, R As RuleEventArgs)(ByVal target As T, ByVal e As R) As Boolean

End Namespace