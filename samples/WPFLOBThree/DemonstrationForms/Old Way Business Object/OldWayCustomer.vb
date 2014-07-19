Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Diagnostics
Imports System.Reflection
Imports System.Text
Imports System.Windows

'WARNING - This is DEMO CODE ONLY.  Please do not code your
'business objects like this.  This is just to get some
'functionality to demo the FormNotification control only.
'
'Future articles will have real business objects and a data access layer
'
Public Class OldWayCustomer
    Implements INotifyPropertyChanged
    Implements IDataErrorInfo

#Region " Declarations "

    Private _strFirstName As String = String.Empty
    Private _strLastName As String = String.Empty

#End Region

#Region " Events "

    Public Event PropertyChanged(ByVal sender As Object, ByVal e As System.ComponentModel.PropertyChangedEventArgs) Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged

#End Region

#Region " Properties "

    Public ReadOnly Property [Error]() As String Implements System.ComponentModel.IDataErrorInfo.Error
        Get
            Return Me.CheckProperties(String.Empty)
        End Get
    End Property

    Public Property FirstName() As String
        Get
            Return _strFirstName
        End Get
        Set(ByVal value As String)
            _strFirstName = value
            RaisePropertyChanged("FirstName")
        End Set
    End Property

    Default Public ReadOnly Property Item(ByVal strPropertyName As String) As String Implements System.ComponentModel.IDataErrorInfo.Item
        Get
            Return Me.CheckProperties(strPropertyName)
        End Get
    End Property

    Public Property LastName() As String
        Get
            Return _strLastName
        End Get
        Set(ByVal value As String)
            _strLastName = value
            RaisePropertyChanged("LastName")
        End Set
    End Property

#End Region

#Region " Methods "

    Private Function CheckProperties(ByVal strPropertyName As String) As String

        Dim strResult As String = String.Empty

        If String.IsNullOrEmpty(strPropertyName) OrElse String.Compare(strPropertyName, "FirstName", True) = 0 Then

            If String.IsNullOrEmpty(_strFirstName) Then
                strResult = "First Name is a required field."
            End If

        End If

        If String.IsNullOrEmpty(strPropertyName) OrElse String.Compare(strPropertyName, "LastName", True) = 0 Then

            If String.IsNullOrEmpty(_strLastName) Then

                If strResult.Length <> 0 Then
                    strResult += vbCrLf & vbCrLf
                End If

                strResult += "Last Name is a required field."
            End If

        End If

        Return strResult

    End Function

    Public Function IsValid() As Boolean

        If Me.Error.Length = 0 Then
            Return True

        Else

            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Error"))
            Return False
        End If

    End Function

    Protected Sub RaisePropertyChanged(ByVal strPropertyName As String)

        RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(strPropertyName))
        RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs("Error"))

    End Sub

#End Region

End Class
