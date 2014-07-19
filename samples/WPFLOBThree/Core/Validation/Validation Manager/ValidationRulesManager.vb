
Namespace Validation

    ''' <summary>
    ''' Maintains rule methods for a business object or business object type.
    ''' </summary>
    Public Class ValidationRulesManager

#Region " Declarations "

        Private _objValidationRulesList As Generic.Dictionary(Of String, ValidationRulesList)

#End Region

#Region " Properties "

        ''' <summary>
        ''' Returns RulesDictionary that contains all defined rules for this object.
        ''' </summary>
        Public ReadOnly Property RulesDictionary() As Generic.Dictionary(Of String, ValidationRulesList)
            Get

                If _objValidationRulesList Is Nothing Then
                    _objValidationRulesList = New Generic.Dictionary(Of String, ValidationRulesList)
                End If

                Return _objValidationRulesList
            End Get
        End Property

#End Region

#Region " Methods "

        ''' <summary>
        ''' Adds a rule to the list of rules to be enforced.
        ''' </summary>
        Public Sub AddRule(ByVal rule As IValidationRuleMethod, ByVal strPropertyName As String)

            Dim objlist As List(Of IValidationRuleMethod) = GetRulesForProperty(strPropertyName).List
            objlist.Add(rule)

        End Sub

        ''' <summary>
        ''' Adds a rule to the list of rules to be enforced.
        ''' </summary>
        Public Sub AddRule(ByVal handler As RuleHandler, ByVal e As RuleEventArgs)

            Dim objlist As List(Of IValidationRuleMethod) = GetRulesForProperty(e.PropertyName).List
            objlist.Add(New Validator(handler, e))

        End Sub

        ''' <summary>
        ''' Returns the list containing rules for a property. If no list exists one is created and returned.
        ''' </summary>
        Public Function GetRulesForProperty(ByVal strPropertyName As String) As ValidationRulesList

            If RulesDictionary.ContainsKey(strPropertyName) Then
                Return RulesDictionary.Item(strPropertyName)

            Else

                Dim objValidationRulesList As New ValidationRulesList
                RulesDictionary.Add(strPropertyName, objValidationRulesList)
                Return objValidationRulesList
            End If

        End Function

#End Region

    End Class

End Namespace
