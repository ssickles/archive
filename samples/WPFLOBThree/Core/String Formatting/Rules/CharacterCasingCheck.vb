
Namespace StringFormatting

    Public Class CharacterCasingCheck

#Region " Declarations "

        Private _strLookFor As String = String.Empty
        Private _strReplaceWith As String = String.Empty

#End Region

#Region " Properties "

        Public ReadOnly Property LookFor() As String
            Get
                Return _strLookFor
            End Get
        End Property

        Public ReadOnly Property ReplaceWith() As String
            Get
                Return _strReplaceWith
            End Get
        End Property

#End Region

#Region " Constructors "

        Public Sub New(ByVal strLookFor As String, ByVal strReplaceWith As String)

            If strLookFor.Length <> strReplaceWith.Length Then
                Throw New ArgumentException("The LookFor and ReplaceWith strings must be the same length.")
            End If

            _strLookFor = strLookFor
            _strReplaceWith = strReplaceWith

        End Sub

#End Region

    End Class

End Namespace
