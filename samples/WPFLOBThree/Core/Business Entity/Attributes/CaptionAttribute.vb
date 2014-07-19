
Namespace BusinessEntity

    Public Class CaptionAttribute
        Inherits Attribute

#Region " Declarations "

        Private _strCaption As String = String.Empty

#End Region

#Region " Properties "

        Public Property Caption() As String
            Get
                Return _strCaption
            End Get
            Set(ByVal Value As String)
                _strCaption = Value
            End Set
        End Property

#End Region

#Region " Constructor "

        Public Sub New(ByVal strCaption As String)
            _strCaption = strCaption

        End Sub

#End Region

#Region " Methods "

        Public Overrides Function ToString() As String
            Return _strCaption

        End Function

#End Region

    End Class

End Namespace