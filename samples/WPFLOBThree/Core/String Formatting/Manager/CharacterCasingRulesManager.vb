
Namespace StringFormatting

    ''' <summary>
    ''' Maintains rule methods for a business object or business object type.
    ''' </summary>
    Public Class CharacterCasingRulesManager

#Region " Declarations "

        Private _objCharacterCasingRulesList As Generic.Dictionary(Of String, CharacterCasing)

#End Region

#Region " Properties "

        ''' <summary>
        ''' Returns RulesDictionary that contains all defined rules for this object.
        ''' </summary>
        Public ReadOnly Property RulesDictionary() As Generic.Dictionary(Of String, CharacterCasing)
            Get

                If _objCharacterCasingRulesList Is Nothing Then
                    _objCharacterCasingRulesList = New Generic.Dictionary(Of String, CharacterCasing)
                End If

                Return _objCharacterCasingRulesList
            End Get
        End Property

#End Region

#Region " Methods "

        ''' <summary>
        ''' Adds a CharacterCasing Formatting rule to the list of rules to be executed when the property is changed.
        ''' </summary>
        Public Sub AddRule(ByVal strPropertyName As String, ByVal enumCharacterCasing As CharacterCasing)
            Me.RulesDictionary.Add(strPropertyName, enumCharacterCasing)

        End Sub

        ''' <summary>
        ''' Returns the CharacterCasing rule for a property.
        ''' </summary>
        Public Function GetRulesForProperty(ByVal strPropertyName As String) As CharacterCasing

            If RulesDictionary.ContainsKey(strPropertyName) Then
                Return RulesDictionary.Item(strPropertyName)

            Else
                Return CharacterCasing.None
            End If

        End Function

#End Region

    End Class

End Namespace
