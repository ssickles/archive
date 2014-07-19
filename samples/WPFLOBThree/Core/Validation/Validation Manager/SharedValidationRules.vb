
Namespace Validation

    ''' <summary>
    ''' Maintains a list of all the per-type <see cref="ValidationRulesManager"/> objects loaded in memory.
    ''' </summary>
    Public Module SharedValidationRules

#Region " Declarations "

        Private _objValidationRuleManagers As New Dictionary(Of Type, ValidationRulesManager)

#End Region

#Region " Methods "

        ''' <summary>
        ''' Gets the <see cref="ValidationRulesManager"/> for the specified object type, optionally creating a new instance of the object if necessary.
        ''' </summary>
        ''' <param name="objectType">
        ''' Type of business object for which the rules apply.
        ''' </param>
        Public Function GetManager(ByVal objectType As Type) As ValidationRulesManager

            Dim objManager As ValidationRulesManager = Nothing

            If Not _objValidationRuleManagers.TryGetValue(objectType, objManager) Then
                SyncLock _objValidationRuleManagers
                    objManager = New ValidationRulesManager
                    _objValidationRuleManagers.Add(objectType, objManager)
                End SyncLock
            End If

            Return objManager

        End Function

        ''' <summary>
        ''' Gets a value indicating whether a set of rules have been created for a given <see cref="Type" />.
        ''' </summary>
        ''' <param name="objectType">
        ''' Type of business object for which the rules apply.
        ''' </param>
        ''' <returns><see langword="true" /> if rules exist for the type.</returns>
        Public Function RulesExistFor(ByVal objectType As Type) As Boolean
            Return _objValidationRuleManagers.ContainsKey(objectType)

        End Function

#End Region

    End Module

End Namespace
