Imports System.ComponentModel

Namespace WPF

    ''' <summary>
    ''' This is used as the base class for all WPF forms (UI that is contained in a UserControl)
    ''' This class will be have more features added on as this series continues
    ''' </summary>
    ''' <remarks></remarks>
    Public Class UserControlBase
        Inherits System.Windows.Controls.UserControl
        Implements INotifyPropertyChanged

#Region " Declarations "

        Private _objControlsWithValiationExceptions As New Dictionary(Of String, System.Windows.Controls.Control)

#End Region

#Region " Events "

        Public Event PropertyChanged As PropertyChangedEventHandler Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged

#End Region

#Region " Properties "

        Public ReadOnly Property ValidationExceptionErrors() As String
            Get
                Return GetExceptionValidationErrors()
            End Get
        End Property

        Public ReadOnly Property HasValidationExceptionErrors() As Boolean
            Get
                Return _objControlsWithValiationExceptions.Keys.Count > 0
            End Get
        End Property

#End Region

#Region " Methods "

        Private Sub ExceptionValidationErrorHandler(ByVal sender As Object, ByVal e As RoutedEventArgs)

            Dim args As System.Windows.Controls.ValidationErrorEventArgs = DirectCast(e, System.Windows.Controls.ValidationErrorEventArgs)

            If TypeOf args.Error.RuleInError Is System.Windows.Controls.ExceptionValidationRule Then
                'only work with validation errors that are Exceptions because the business object has already recorded the business rule violations.

                Dim strDataItemName As String = DirectCast(DirectCast(DirectCast(args.Error, System.Windows.Controls.ValidationError).BindingInError, System.Object), System.Windows.Data.BindingExpression).DataItem.ToString
                Dim strPropertyName As String = DirectCast(DirectCast(DirectCast(DirectCast(DirectCast(args.Error, System.Windows.Controls.ValidationError).BindingInError, System.Object), System.Windows.Data.BindingExpression).ParentBinding, System.Windows.Data.Binding).Path, System.Windows.PropertyPath).Path

                Dim strKey As String = String.Concat(strDataItemName, ":", strPropertyName)

                If args.Action = ValidationErrorEventAction.Added Then

                    _objControlsWithValiationExceptions.Item(strKey) = DirectCast(e.OriginalSource, System.Windows.Controls.Control)
                    OnRaisePropertyChanged("ValidationExceptionErrors")

                ElseIf args.Action = ValidationErrorEventAction.Removed Then

                    _objControlsWithValiationExceptions.Remove(strKey)
                    OnRaisePropertyChanged("ValidationExceptionErrors")

                End If
            End If
        End Sub

        Private Sub UserControlBase_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
            'this adds a form level handler and will listen for each control that has the NotifyOnValidationError=True and a ValidationError occurs.
            Me.AddHandler(System.Windows.Controls.Validation.ErrorEvent, New RoutedEventHandler(AddressOf ExceptionValidationErrorHandler), True)

        End Sub

        Private Sub UserControlBase_Unloaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Unloaded
            Me.RemoveHandler(System.Windows.Controls.Validation.ErrorEvent, New RoutedEventHandler(AddressOf ExceptionValidationErrorHandler))

        End Sub

        Protected Sub ClearValidationExceptionErrors()
            _objControlsWithValiationExceptions.Clear()
            OnRaisePropertyChanged("ValidationExceptionErrors")

        End Sub

        ''' <summary>
        ''' Checks each of the controls that have raised the Validation.ErrorEvent to see if the control still has an exception validation rule in the control's Validation.Errors collection
        ''' </summary>
        ''' <remarks>This is the SUPER fast way to check the Validation Exception errors.  This works only if the controls on the form all have the property, NotifyOnValidationError=True</remarks>
        Private Function GetExceptionValidationErrors() As String

            Dim sb As New System.Text.StringBuilder(1024)

            For Each obj As KeyValuePair(Of String, System.Windows.Controls.Control) In _objControlsWithValiationExceptions

                If System.Windows.Controls.Validation.GetHasError(obj.Value) Then

                    For Each objVE As ValidationError In System.Windows.Controls.Validation.GetErrors(DirectCast(obj.Value, DependencyObject))

                        If TypeOf objVE.RuleInError Is System.Windows.Controls.ExceptionValidationRule Then
                            sb.Append(StringFormatting.CamelCaseString.GetWords(DirectCast(DirectCast(DirectCast(DirectCast(objVE.BindingInError, System.Object), System.Windows.Data.BindingExpression).ParentBinding, System.Windows.Data.Binding).Path, System.Windows.PropertyPath).Path))
                            sb.Append(" has error ")

                            If objVE.Exception Is Nothing OrElse objVE.Exception.InnerException Is Nothing Then
                                sb.AppendLine(objVE.ErrorContent.ToString)
                            Else
                                sb.AppendLine(objVE.Exception.InnerException.Message)
                            End If

                        End If

                    Next

                End If

            Next

            Return sb.ToString

        End Function

        Protected Sub OnRaisePropertyChanged(ByVal strPropertyName As String)

            If Me.PropertyChangedEvent IsNot Nothing Then

                RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(strPropertyName))
            End If

        End Sub

        Protected Sub UpdateFocusedField()

            Dim fwE As FrameworkElement = TryCast(Keyboard.FocusedElement, FrameworkElement)

            If fwE IsNot Nothing Then

                Dim expression As BindingExpression = Nothing

                If TypeOf fwE Is TextBox Then
                    expression = fwE.GetBindingExpression(TextBox.TextProperty)
                    '
                    'TODO developers add more controls types here.  Won't be that many.
                    'this would include custom TextBox controls or 3rd party TextBox controls 
                End If

                If expression IsNot Nothing Then
                    expression.UpdateSource()
                End If

            End If

        End Sub

#End Region

    End Class

End Namespace
