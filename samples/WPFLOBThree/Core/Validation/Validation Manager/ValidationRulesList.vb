
Namespace Validation

    Public Class ValidationRulesList

#Region " Declarations "

        Private _objList As List(Of IValidationRuleMethod)

#End Region

#Region " Properties "

        Public ReadOnly Property List() As List(Of IValidationRuleMethod)
            Get

                If _objList Is Nothing Then
                    _objList = New List(Of IValidationRuleMethod)
                End If

                Return _objList
            End Get
        End Property

#End Region

    End Class

End Namespace
