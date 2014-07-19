Imports System.Text.RegularExpressions

Namespace Validation

    Public Class StringValidationRules

#Region " Methods "

        Public Shared Function RegularExpressionRule(ByVal target As Object, ByVal e As RuleEventArgs) As Boolean

            'great site for patterns
            'http://regexlib.com/Search.aspx
            Dim args As RegulatExpressionRuleEventArgs = TryCast(e, RegulatExpressionRuleEventArgs)
            If args Is Nothing Then
                Throw New ArgumentException(String.Format("Wrong type of event args passed to RegularExpressionRule.  Should have received RegulatExpressionRuleEventArgs but was passed {0}", e.GetType))
            End If

            Dim strTestMe As String = TryCast(CallByName(target, args.PropertyName, CallType.Get), String)

            If args.IsRequired Then

                If strTestMe Is Nothing OrElse IsDBNull(strTestMe) OrElse strTestMe.Trim.Length = 0 Then
                    e.BrokenRuleDescription = String.Format("{0} was null or empty but is a required field.", RuleEventArgs.GetPropertyFriendlyName(e))
                    Return False
                End If

            Else

                If strTestMe Is Nothing OrElse strTestMe.Trim.Length = 0 Then
                    Return True
                End If

            End If

            Dim strPattern As String = String.Empty
            Dim strBrokenRuleDescription As String = String.Empty

            Select Case args.RegularExpressionPatternType

                Case RegularExpressionPatternType.Custom
                    strPattern = args.CustomRegularExpressionPattern

                    If strPattern.Trim.Length = 0 Then
                        Throw New ArgumentException("Programmer did not supply the CustomRegularExpressionPattern")
                    End If

                    strBrokenRuleDescription = String.Format("{0} did not match the required {1} pattern.", RuleEventArgs.GetPropertyFriendlyName(e), strPattern)

                Case RegularExpressionPatternType.Email
                    strPattern = "^\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$"
                    strBrokenRuleDescription = String.Format("{0} did not match the required email pattern.", RuleEventArgs.GetPropertyFriendlyName(e))

                Case RegularExpressionPatternType.IPAddress
                    strPattern = "^((25[0-5]|2[0-4][0-9]|1[0-9][0-9]|[1-9][0-9]|[0-9])\.){3}(25[0-5]|2[0-4][0-9]|1[0-9][0-9]|[1-9][0-9]|[0-9])$"
                    strBrokenRuleDescription = String.Format("{0} did not match the required IP Address pattern.", RuleEventArgs.GetPropertyFriendlyName(e))

                Case RegularExpressionPatternType.SSN
                    strPattern = "^\d{3}-\d{2}-\d{4}$"
                    strBrokenRuleDescription = String.Format("{0} did not match the required SSN pattern.", RuleEventArgs.GetPropertyFriendlyName(e))

                Case RegularExpressionPatternType.URL
                    strPattern = "(?#WebOrIP)((?#protocol)((news|nntp|telnet|http|ftp|https|ftps|sftp):\/\/)?(?#subDomain)(([a-zA-Z0-9]+\.*(?#domain)[a-zA-Z0-9\-]+(?#TLD)(\.[a-zA-Z]+){1,2})|(?#IPAddress)((25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9])\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9]|0)\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9]|0)\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[0-9])))+(?#Port)(:[1-9][0-9]*)?)+(?#Path)((\/((?#dirOrFileName)[a-zA-Z0-9_\-\%\~\+]+)?)*)?(?#extension)(\.([a-zA-Z0-9_]+))?(?#parameters)(\?([a-zA-Z0-9_\-]+\=[a-z-A-Z0-9_\-\%\~\+]+)?(?#additionalParameters)(\&([a-zA-Z0-9_\-]+\=[a-z-A-Z0-9_\-\%\~\+]+)?)*)?"
                    strBrokenRuleDescription = String.Format("{0} did not match the required URL pattern.", RuleEventArgs.GetPropertyFriendlyName(e))

                Case RegularExpressionPatternType.ZipCode
                    strPattern = "^\d{5}(-\d{4})?$"
                    strBrokenRuleDescription = String.Format("{0} did not match the required Zip Code pattern.", RuleEventArgs.GetPropertyFriendlyName(e))

                Case Else
                    Throw New ArgumentOutOfRangeException("RegularExpressionPatternType", args.RegularExpressionPatternType, "Programmer did not program this pattern type")
            End Select

            If Regex.IsMatch(strTestMe, strPattern, RegexOptions.IgnoreCase) Then
                Return True

            Else
                e.BrokenRuleDescription = strBrokenRuleDescription
                Return False
            End If

        End Function

        Public Shared Function StringLengthRule(ByVal target As Object, ByVal e As RuleEventArgs) As Boolean

            Dim args As StringLengthRuleEventArgs = DirectCast(e, StringLengthRuleEventArgs)
            If args Is Nothing Then
                Throw New ArgumentException(String.Format("Wrong type of event args passed to StringLengthRule.  Should have received StringLengthRuleEventArgs but was passed {0}", e.GetType))
            End If

            Dim strTestMe As String = TryCast(CallByName(target, args.PropertyName, CallType.Get), String)

            If args.AllowNullString = False AndAlso (IsDBNull(strTestMe) OrElse strTestMe Is Nothing) Then
                e.BrokenRuleDescription = String.Format("{0} can not be null.", RuleEventArgs.GetPropertyFriendlyName(e))
                Return False

            ElseIf args.AllowNullString = True AndAlso (strTestMe Is Nothing OrElse IsDBNull(strTestMe)) Then
                Return True
            End If

            If IsDBNull(strTestMe) OrElse String.IsNullOrEmpty(strTestMe) Then
                strTestMe = String.Empty
            End If

            If args.MinimumLength > 0 AndAlso strTestMe.Length < args.MinimumLength Then
                e.BrokenRuleDescription = String.Format("{0} can not be less than {1} character(s) long.", RuleEventArgs.GetPropertyFriendlyName(e), args.MinimumLength.ToString)
                Return False

            ElseIf args.MaximumLength > 0 AndAlso strTestMe.Length > args.MaximumLength Then
                e.BrokenRuleDescription = String.Format("{0} can not be greater than {1} character(s) long.", RuleEventArgs.GetPropertyFriendlyName(e), args.MaximumLength.ToString)
                Return False
            End If

            Return True

        End Function

#End Region

    End Class

End Namespace
