
Namespace Validation

    Public Class ComparisionValidationRules

#Region " Methods "

        Public Shared Function ComparePropertyRule(ByVal target As Object, ByVal e As RuleEventArgs) As Boolean

            Dim args As ComparePropertyRuleEventArgs = TryCast(e, ComparePropertyRuleEventArgs)
            If args Is Nothing Then
                Throw New ArgumentException(String.Format("Wrong type of event args passed to ComparePropertyRule.  Should have received ComparePropertyRuleEventArgs but was passed {0}", e.GetType))
            End If

            Dim objSource As Object = CallByName(target, args.PropertyName, CallType.Get)

            If args.IsRequired Then

                If objSource Is Nothing OrElse IsDBNull(objSource) Then
                    e.BrokenRuleDescription = String.Format("{0} was null or empty but is a required field.", RuleEventArgs.GetPropertyFriendlyName(e))
                    Return False
                End If

            Else

                If objSource Is Nothing OrElse IsDBNull(objSource) Then
                    Return True
                End If

            End If

            Dim objTestAgainst As Object = CallByName(target, args.CompareToPropertyName, CallType.Get)

            If objTestAgainst Is Nothing OrElse IsDBNull(objTestAgainst) Then
                Return True
            End If

            Dim objISource As IComparable = DirectCast(objSource, IComparable)
            Dim objITestAgainst As IComparable = DirectCast(objTestAgainst, IComparable)
            Dim intResult As Integer = objISource.CompareTo(objITestAgainst)

            Select Case args.ComparisionType

                Case ComparisionType.Equal

                    If intResult = 0 Then
                        Return True

                    Else
                        e.BrokenRuleDescription = String.Format("{0} must be equal to {1}.", RuleEventArgs.GetPropertyFriendlyName(e), objTestAgainst.ToString)
                        Return False
                    End If

                Case ComparisionType.GreaterThan

                    If intResult > 0 Then
                        Return True

                    Else
                        e.BrokenRuleDescription = String.Format("{0} must be greater than {1}.", RuleEventArgs.GetPropertyFriendlyName(e), objTestAgainst.ToString)
                        Return False
                    End If

                Case ComparisionType.GreaterThanEqual

                    If intResult >= 0 Then
                        Return True

                    Else
                        e.BrokenRuleDescription = String.Format("{0} must be greater than or equal to {1}.", RuleEventArgs.GetPropertyFriendlyName(e), objTestAgainst.ToString)
                        Return False
                    End If

                Case ComparisionType.LessThan

                    If intResult < 0 Then
                        Return True

                    Else
                        e.BrokenRuleDescription = String.Format("{0} must be less than {1}.", RuleEventArgs.GetPropertyFriendlyName(e), objTestAgainst.ToString)
                        Return False
                    End If

                Case ComparisionType.LessThanEqual

                    If intResult <= 0 Then
                        Return True

                    Else
                        e.BrokenRuleDescription = String.Format("{0} must be less than or equal to {1}.", RuleEventArgs.GetPropertyFriendlyName(e), objTestAgainst.ToString)
                        Return False
                    End If

                Case ComparisionType.NotEqual

                    If intResult <> 0 Then
                        Return True

                    Else
                        e.BrokenRuleDescription = String.Format("{0} must not equal {1}.", RuleEventArgs.GetPropertyFriendlyName(e), objTestAgainst.ToString)
                        Return False
                    End If

                Case Else
                    Throw New ArgumentOutOfRangeException("ComparisionType", args.ComparisionType, "Comparision type not programmed")
            End Select

        End Function

        Public Shared Function CompareValueRule(ByVal target As Object, ByVal e As RuleEventArgs) As Boolean

            Dim args As CompareValueRuleEventArgs = TryCast(e, CompareValueRuleEventArgs)
            If args Is Nothing Then
                Throw New ArgumentException(String.Format("Wrong type of event args passed to CompareValueRule.  Should have received CompareValueRuleEventArgs but was passed {0}", e.GetType))
            End If

            Dim objSource As Object = CallByName(target, args.PropertyName, CallType.Get)

            If args.IsRequired Then

                If objSource Is Nothing OrElse IsDBNull(objSource) Then
                    e.BrokenRuleDescription = String.Format("{0} was null or empty but is a required field.", RuleEventArgs.GetPropertyFriendlyName(e))
                    Return False
                End If

            Else

                If objSource Is Nothing OrElse IsDBNull(objSource) Then
                    Return True
                End If

            End If

            Dim objTestAgainst As Object = args.CompareToValue
            Dim objISource As IComparable = DirectCast(objSource, IComparable)
            Dim intResult As Integer = objISource.CompareTo(args.CompareToValue)

            Select Case args.ComparisionType

                Case ComparisionType.Equal

                    If intResult = 0 Then
                        Return True

                    Else
                        e.BrokenRuleDescription = String.Format("{0} must be equal to {1}.", RuleEventArgs.GetPropertyFriendlyName(e), objTestAgainst.ToString)
                        Return False
                    End If

                Case ComparisionType.GreaterThan

                    If intResult > 0 Then
                        Return True

                    Else
                        e.BrokenRuleDescription = String.Format("{0} must be greater than {1}.", RuleEventArgs.GetPropertyFriendlyName(e), objTestAgainst.ToString)
                        Return False
                    End If

                Case ComparisionType.GreaterThanEqual

                    If intResult >= 0 Then
                        Return True

                    Else
                        e.BrokenRuleDescription = String.Format("{0} must be greater than or equal to {1}.", RuleEventArgs.GetPropertyFriendlyName(e), objTestAgainst.ToString)
                        Return False
                    End If

                Case ComparisionType.LessThan

                    If intResult < 0 Then
                        Return True

                    Else
                        e.BrokenRuleDescription = String.Format("{0} must be less than {1}.", RuleEventArgs.GetPropertyFriendlyName(e), objTestAgainst.ToString)
                        Return False
                    End If

                Case ComparisionType.LessThanEqual

                    If intResult <= 0 Then
                        Return True

                    Else
                        e.BrokenRuleDescription = String.Format("{0} must be less than or equal to {1}.", RuleEventArgs.GetPropertyFriendlyName(e), objTestAgainst.ToString)
                        Return False
                    End If

                Case ComparisionType.NotEqual

                    If intResult <> 0 Then
                        Return True

                    Else
                        e.BrokenRuleDescription = String.Format("{0} must not equal {1}.", RuleEventArgs.GetPropertyFriendlyName(e), objTestAgainst.ToString)
                        Return False
                    End If

                Case Else
                    Throw New ArgumentOutOfRangeException("ComparisionType", args.ComparisionType, "Comparision type not programmed")
            End Select

        End Function

        Public Shared Function InRangeRule(ByVal target As Object, ByVal e As RuleEventArgs) As Boolean

            Dim args As RangeRuleEventArgs = TryCast(e, RangeRuleEventArgs)
            If args Is Nothing Then
                Throw New ArgumentException(String.Format("Wrong type of event args passed to InRangeRule.  Should have received RangeRuleEventArgs but was passed {0}", e.GetType))
            End If

            Dim objSource As Object = CallByName(target, args.PropertyName, CallType.Get)

            If args.IsRequired Then

                If objSource Is Nothing OrElse IsDBNull(objSource) Then
                    e.BrokenRuleDescription = String.Format("{0} was null or empty but is a required field.", RuleEventArgs.GetPropertyFriendlyName(e))
                    Return False
                End If

            Else

                If objSource Is Nothing OrElse IsDBNull(objSource) Then
                    Return True
                End If

            End If

            Dim objISource As IComparable = DirectCast(objSource, IComparable)
            Dim objLower As Object = args.LowerValue
            Dim objUpper As Object = args.UpperValue
            Dim intLowerResult As Integer = objISource.CompareTo(args.LowerValue)

            If args.LowerRangeBoundaryType = RangeBoundaryType.Inclusive Then

                If intLowerResult < 0 Then
                    e.BrokenRuleDescription = String.Format("{0} must be greater than or equal to {1}", RuleEventArgs.GetPropertyFriendlyName(e), objLower.ToString)
                    Return False
                End If

            Else

                If intLowerResult <= 0 Then
                    e.BrokenRuleDescription = String.Format("{0} must be greater than {1}", RuleEventArgs.GetPropertyFriendlyName(e), objLower.ToString)
                    Return False
                End If

            End If

            Dim intUpperResult As Integer = objISource.CompareTo(args.UpperValue)

            If args.UpperRangeBoundaryType = RangeBoundaryType.Inclusive Then

                If intUpperResult > 0 Then
                    e.BrokenRuleDescription = String.Format("{0} must be less than or equal to {1}", RuleEventArgs.GetPropertyFriendlyName(e), objLower.ToString)
                    Return False
                End If

            Else

                If intUpperResult <= 0 Then
                    e.BrokenRuleDescription = String.Format("{0} must be less than {1}", RuleEventArgs.GetPropertyFriendlyName(e), objLower.ToString)
                    Return False
                End If

            End If

            Return True

        End Function

        Public Shared Function NotNullRule(ByVal target As Object, ByVal e As RuleEventArgs) As Boolean

            'Boxing and Unboxing
            'When a nullable type is boxed, the common language runtime automatically boxes the underlying value of the 
            'Nullable object, not the Nullable object itself. That is, if the HasValue property is true, the contents 
            'of the Value property is boxed. If the HasValue property is false, a null reference (Nothing in Visual Basic) is boxed. 
            'When the underlying value of a nullable type is unboxed, the common language runtime creates a new 
            'Nullable structure initialized to the underlying value. 
            Dim args As NotNullRuleEventArgs = TryCast(e, NotNullRuleEventArgs)
            If args Is Nothing Then
                Throw New ArgumentException(String.Format("Wrong type of event args passed to NotNullRule.  Should have received NotNullRuleEventArgs but was passed {0}", e.GetType))
            End If

            Dim objSource As Object = CallByName(target, args.PropertyName, CallType.Get)

            'this handles both Nullable and standard uninitialized values
            If objSource Is Nothing Then
                e.BrokenRuleDescription = String.Format("{0} is null.", RuleEventArgs.GetPropertyFriendlyName(e))
                Return False

            ElseIf IsDBNull(objSource) Then
                e.BrokenRuleDescription = String.Format("{0} is DBNull.", RuleEventArgs.GetPropertyFriendlyName(e))
                Return False
            End If

            Return True

        End Function

#End Region

    End Class

End Namespace
