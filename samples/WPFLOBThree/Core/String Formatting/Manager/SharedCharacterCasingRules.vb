
Namespace StringFormatting

    Module SharedCharacterCasingRules

#Region " Declarations "

        Private _objCharacterCasingRulesManagers As New Dictionary(Of Type, CharacterCasingRulesManager)

#End Region

#Region " Methods "

        ''' <summary>
        ''' Gets the <see cref="CharacterCasingRulesManager"/> for the specified object type, optionally creating a new instance of the object if necessary.
        ''' </summary>
        ''' <param name="objectType">
        ''' Type of business object for which the rules apply.
        ''' </param>
        Public Function GetManager(ByVal objectType As Type) As CharacterCasingRulesManager

            Dim objManager As CharacterCasingRulesManager = Nothing

            If Not _objCharacterCasingRulesManagers.TryGetValue(objectType, objManager) Then
                SyncLock _objCharacterCasingRulesManagers
                    objManager = New CharacterCasingRulesManager
                    _objCharacterCasingRulesManagers.Add(objectType, objManager)
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
            Return _objCharacterCasingRulesManagers.ContainsKey(objectType)

        End Function

#End Region

    End Module

End Namespace
