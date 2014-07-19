Namespace StringFormatting

    Public Class CamelCaseString

#Region " Methods "

        ''' <summary>
        ''' Designed to parse property or database column names and return a friendly name without punctuation characters.  Example:  "ap_c_FirstName" will result in "First Name"
        ''' </summary>
        ''' <returns>String with words parsed from camel case string and space added between words.</returns>
        Public Shared Function GetWords(ByVal strCamel As String) As String

            Dim sb As New System.Text.StringBuilder(256)
            Dim bolFoundUpper As Boolean = False

            For Each c As Char In strCamel

                If bolFoundUpper Then

                    If Char.IsUpper(c) Then
                        sb.Append(" ")
                        sb.Append(c)

                    ElseIf Char.IsLetterOrDigit(c) Then
                        sb.Append(c)
                    End If

                ElseIf Char.IsUpper(c) Then
                    bolFoundUpper = True
                    sb.Append(c)
                End If

            Next

            Return sb.ToString

        End Function

#End Region

    End Class

End Namespace