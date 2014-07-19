
Namespace BusinessEntity

    Public Class AuditAttribute
        Inherits Attribute

#Region " Declarations "

        Private _intSortOrder As Integer

#End Region

#Region " Properties "

        Public Property SortOrder() As Integer
            Get
                Return _intSortOrder
            End Get
            Set(ByVal Value As Integer)
                _intSortOrder = Value
            End Set
        End Property

#End Region

#Region " Constructor "

        Public Sub New(Optional ByVal intSortOrder As Integer = 99999)
            _intSortOrder = intSortOrder

        End Sub

#End Region

    End Class

End Namespace