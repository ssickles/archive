
Namespace StringFormatting

    Public Class FormatText

#Region " Methods "

        Public Shared Function ApplyCharacterCasing(ByVal strIn As String, ByVal enumCharacterCase As CharacterCasing) As String
            strIn = strIn.Trim

            If strIn.Length = 0 Then
                Return strIn.Trim
                Exit Function
            End If

            Dim intX As Integer

            Select Case enumCharacterCase

                Case CharacterCasing.None
                    Return strIn

                Case CharacterCasing.LowerCase
                    Return strIn.ToLower

                Case CharacterCasing.UpperCase
                    Return strIn.ToUpper

                Case CharacterCasing.OutlookPhoneNoProperName
                    Return FormatOutLookPhone(strIn)

                Case CharacterCasing.OutlookPhoneUpper
                    Return FormatOutLookPhone(strIn).ToUpper
            End Select

            strIn = strIn.ToLower

            Dim strPrevious As String = " "
            Dim strPreviousTwo As String = "  "
            Dim strPreviousThree As String = "   "
            Dim strChar As String

            For intX = 0 To strIn.Length - 1
                strChar = strIn.Substring(intX, 1)

                If Char.IsLetter(CType(strChar, Char)) AndAlso strChar <> strChar.ToUpper Then

                    If strPrevious = " " _
                        OrElse strPrevious = "." _
                        OrElse strPrevious = "-" _
                        OrElse strPrevious = "/" _
                        OrElse strPreviousThree = " O'" _
                        OrElse strPreviousTwo = "Mc" Then
                        Mid(strIn, intX + 1, 1) = strChar.ToUpper
                        strPrevious = strChar.ToUpper

                    Else
                        strPrevious = strChar
                    End If

                Else
                    strPrevious = strChar
                End If

                strPreviousTwo = strPreviousTwo.Substring(1, 1) & strPrevious
                strPreviousThree = strPreviousThree.Substring(1, 1) & strPreviousThree.Substring(2, 1) & strPrevious
            Next

            intX = strIn.IndexOf("'")

            If intX = 1 Then
                Mid(strIn, intX + 2, 1) = strIn.Substring(intX + 1, 1).ToUpper
            End If

            Try
                intX = strIn.IndexOf("'", 3)

                If intX > 3 AndAlso strIn.Substring(intX - 2, 1) = " " Then
                    Mid(strIn, intX + 2, 1) = strIn.Substring(intX + 1, 1).ToUpper
                End If

            Catch
            End Try

            'never remove this code
            strIn += " "

            For Each objCheck As CharacterCasingCheck In StringFormatting.CharacterCasingChecks.GetChecks

                If strIn.Contains(objCheck.LookFor) Then

                    Try
                        Mid(strIn, strIn.IndexOf(objCheck.LookFor) + 1, objCheck.LookFor.Length) = objCheck.ReplaceWith

                    Catch
                    End Try

                End If

            Next

            If enumCharacterCase = CharacterCasing.OutlookPhoneProperName Then
                strIn = FormatOutLookPhone(strIn)
            End If

            Return strIn.Trim

        End Function

        Private Shared Function FormatOutLookPhone(ByVal strIn As String) As String

            If strIn.Trim.Length = 0 Then
                Return strIn
            End If

            Dim strTempCasted As String = strIn & " "

            Try

                Dim strTemp As String = strTempCasted
                Dim intX As Integer = strTemp.IndexOf(" ", 8)

                If intX > 0 Then
                    strTemp = strIn.Substring(0, intX)
                    strTemp = strTemp.Replace("(", "")
                    strTemp = strTemp.Replace(")", "")
                    strTemp = strTemp.Replace(" ", "")
                    strTemp = strTemp.Replace("-", "")

                    If IsNumeric(strTemp) AndAlso strTemp.Length = 10 Then
                        strTempCasted = CType(strTemp, Long).ToString("(###) ###-####") & "  " & strTempCasted.Substring(intX).Trim
                    End If

                Else
                End If

            Catch
            End Try

            Return strTempCasted

        End Function

#End Region

    End Class

End Namespace