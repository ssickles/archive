Partial Public Class PartTwo

#Region " Declarations "

    Private _objCustomer As OldWayCustomer

#End Region

#Region " Constructor & Loaded "

    Public Sub New()
        ' This call is required by the Windows Form Designer.
        InitializeComponent()

    End Sub

    Private Sub PartTwo_Loaded(ByVal sender As Object, _
                               ByVal e As System.Windows.RoutedEventArgs) _
            Handles Me.Loaded

        If Me.DataContext Is Nothing Then
            NewRecord()
            Me.txtFirstName.Focus()
        End If

    End Sub

#End Region

#Region " Methods "

    Private Sub btnNew_Click(ByVal sender As Object, _
                             ByVal e As System.Windows.RoutedEventArgs) _
            Handles btnNew.Click
        NewRecord()

    End Sub

    Private Sub btnSave_Click(ByVal sender As Object, _
                              ByVal e As System.Windows.RoutedEventArgs) _
            Handles btnSave.Click
        UpdateFocusedField()

        If _objCustomer.IsValid Then
            Me.frmNotification.NotificationMessage = "Customer record saved"
        End If

    End Sub

    Private Sub NewRecord()
        Me.frmNotification.NotificationMessage = String.Empty
        _objCustomer = New OldWayCustomer()
        Me.DataContext = _objCustomer

    End Sub

    Private Sub UpdateFocusedField()

        Dim fwE As FrameworkElement = TryCast(Keyboard.FocusedElement, FrameworkElement)

        If fwE IsNot Nothing Then

            Dim expression As BindingExpression = Nothing

            If TypeOf fwE Is TextBox Then
                expression = fwE.GetBindingExpression(TextBox.TextProperty)
                'TODO add more controls types here.  Won't be that many.
            End If

            If expression IsNot Nothing Then
                expression.UpdateSource()
            End If

        End If

    End Sub

#End Region

End Class
