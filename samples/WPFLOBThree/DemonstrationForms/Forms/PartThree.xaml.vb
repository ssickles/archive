Imports Core.WPF
Partial Public Class PartThree
    Inherits UserControlBase

#Region " Declarations "

    Private WithEvents _objCustomer As Customer

#End Region

#Region " Constructor & Loaded "

    Public Sub New()
        ' This call is required by the Windows Form Designer.
        InitializeComponent()

    End Sub

    Private Sub PartThree_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded

        If Me.DataContext Is Nothing Then
            NewRecord()
            Me.txtFirstName.Focus()
        End If

    End Sub

#End Region

#Region " Methods "

    Private Sub btnNew_Click(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnNew.Click
        NewRecord()

    End Sub

    Private Sub btnSave_Click(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnSave.Click
        '
        UpdateFocusedField()

        If Not MyBase.HasValidationExceptionErrors AndAlso _objCustomer.IsValid Then
            'simulate saving the record in a database.
            Me.frmNotification.NotificationMessage = "Customer record saved"
            '
            '
            MessageBox.Show(_objCustomer.ToAuditString(String.Empty, vbCrLf), "Customer Class Audit Message", MessageBoxButton.OK, MessageBoxImage.Information)
            MessageBox.Show(_objCustomer.ToClassString(vbCrLf, True), "Customer Class Message", MessageBoxButton.OK, MessageBoxImage.Information)

        Else
            Me.frmNotification.ErrorHeaderText = "Save Errors"
        End If

    End Sub

    Private Sub NewRecord()
        Me.frmNotification.NotificationMessage = String.Empty
        Me.frmNotification.ErrorHeaderText = "Required Items"
        MyBase.ClearValidationExceptionErrors()
        _objCustomer = New Customer()
        Me.DataContext = _objCustomer
        'this is optional.
        'this will cause any edit errors to display
        _objCustomer.CheckAllRules()

    End Sub

#End Region

End Class
