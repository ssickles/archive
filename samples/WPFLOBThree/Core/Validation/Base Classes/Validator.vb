
Namespace Validation

    Public Class Validator
        Implements IValidationRuleMethod

#Region " Declarations "

        Private _bolIsInstanceRule As Boolean = False
        Private _objRuleEventArgs As RuleEventArgs
        Private _objRuleHandler As RuleHandler
        Private _strRuleName As String = String.Empty

#End Region

#Region " Properties "

        Public ReadOnly Property IsInstanceRule() As Boolean
            Get
                Return _bolIsInstanceRule
            End Get
        End Property

        Public ReadOnly Property RuleEventArgs() As RuleEventArgs Implements IValidationRuleMethod.RuleEventArgs
            Get
                Return _objRuleEventArgs
            End Get
        End Property

        Public ReadOnly Property RuleName() As String Implements IValidationRuleMethod.RuleName
            Get
                Return _strRuleName
            End Get
        End Property

#End Region

#Region " Constructor "

        Public Sub New(ByVal objRuleHandler As RuleHandler, ByVal e As RuleEventArgs)
            _objRuleEventArgs = e
            _objRuleHandler = objRuleHandler
            _strRuleName = String.Format("rule://{0}/{1}", objRuleHandler.Method.Name, e.ToString)

        End Sub

        Public Sub New(ByVal objRuleHandler As RuleHandler, ByVal e As RuleEventArgs, ByVal bolIsInstanceRule As Boolean)
            _objRuleEventArgs = e
            _objRuleHandler = objRuleHandler
            _strRuleName = String.Format("rule://{0}/{1}", objRuleHandler.Method.Name, e.ToString)
            _bolIsInstanceRule = bolIsInstanceRule

            If _bolIsInstanceRule Then
                _strRuleName = "instance" & _strRuleName
            End If

        End Sub

#End Region

#Region " Methods "

        Public Function Invoke(ByVal target As Object) As Boolean Implements IValidationRuleMethod.Invoke
            Return _objRuleHandler.Invoke(target, _objRuleEventArgs)

        End Function

#End Region

    End Class

End Namespace
