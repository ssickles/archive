
Namespace StringFormatting

    Public NotInheritable Class CharacterCasingChecks
        Inherits System.Collections.Generic.List(Of CharacterCasingCheck)

#Region " Declarations & Enums "

        Private Shared _instance As CharacterCasingChecks

#End Region

#Region " Constructors "

        ' Note: constructor is private
        Private Sub New()
        End Sub

#End Region

#Region " Methods "

        Public Shared Function GetChecks() As CharacterCasingChecks

            If _instance Is Nothing Then
                _instance = New CharacterCasingChecks
                'TODO need to determine how to allow our customers to add proper case text corrects here.
                'TODO Developers load this from a data base, config file, web service, etc.
                '
                'These are values that are specific to your company or line of business
                'remove the ones that don't apply and add your own.
                'ensure that the lengths of the LookFor and the ReplaceWith strings are the same
                _instance.Add(New CharacterCasingCheck("Po Box", "PO Box"))
                _instance.Add(New CharacterCasingCheck("C/o ", "c/o "))
                _instance.Add(New CharacterCasingCheck("C/O ", "c/o "))
                _instance.Add(New CharacterCasingCheck("Vpn ", "VPN "))
                _instance.Add(New CharacterCasingCheck("Xp ", "XP "))
                _instance.Add(New CharacterCasingCheck(" Or ", " or "))
                _instance.Add(New CharacterCasingCheck(" And ", " and "))
                _instance.Add(New CharacterCasingCheck(" Nw ", " NW "))
                _instance.Add(New CharacterCasingCheck(" Ne ", " NE "))
                _instance.Add(New CharacterCasingCheck(" Sw ", " SW "))
                _instance.Add(New CharacterCasingCheck(" Se ", " SE "))
                _instance.Add(New CharacterCasingCheck(" Llc. ", " LLC. "))
                _instance.Add(New CharacterCasingCheck(" Llc ", " LLC "))
                _instance.Add(New CharacterCasingCheck(" Lc ", " LC "))
                _instance.Add(New CharacterCasingCheck(" Lc. ", " LC. "))
                _instance.Add(New CharacterCasingCheck(" Lt ", " LT "))
                _instance.Add(New CharacterCasingCheck(" Lt. ", " LT. "))
            End If

            Return _instance
        End Function

#End Region

    End Class

End Namespace
