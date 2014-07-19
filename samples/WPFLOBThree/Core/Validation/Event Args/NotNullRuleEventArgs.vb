
Namespace Validation

    Public Class NotNullRuleEventArgs
        Inherits RuleEventArgs

#Region " Constructor "

        Public Sub New(ByVal e As NotNullValidatorAttribute, ByVal strPropertyName As String)
            MyBase.New(strPropertyName)
            MyBase.PropertyFriendlyName = e.PropertyFriendlyName
            MyBase.CustomMessage = e.CustomMessage
            MyBase.RuleSet = e.RuleSet

        End Sub

        Public Sub New(ByVal strPropertyName As String)
            MyBase.New(strPropertyName)

        End Sub

#End Region

    End Class

End Namespace