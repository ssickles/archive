
Namespace Validation

    <AttributeUsage(AttributeTargets.Property, AllowMultiple:=False, Inherited:=False)> _
    Public NotInheritable Class NotNullValidatorAttribute
        Inherits BaseValidatorAttribute

#Region " Constructor "

        Public Sub New()

        End Sub

#End Region

#Region " Methods "

        Public Overrides Function Create(ByVal strPropertyName As String) As Validator
            Return New Validator(AddressOf Validation.ComparisionValidationRules.NotNullRule, New NotNullRuleEventArgs(Me, strPropertyName))

        End Function

#End Region

    End Class

End Namespace