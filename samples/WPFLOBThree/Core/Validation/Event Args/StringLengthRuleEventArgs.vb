
Namespace Validation

    Public Class StringLengthRuleEventArgs
        Inherits RuleEventArgs

#Region " Declarations "

        Private _bolAllowNullString As Boolean = False
        Private _intMaximumLength As Integer = -1
        Private _intMinimumLength As Integer = -1

#End Region

#Region " Properties "

        Public Property AllowNullString() As Boolean
            Get
                Return _bolAllowNullString
            End Get
            Set(ByVal Value As Boolean)
                _bolAllowNullString = Value
            End Set
        End Property

        Public Property MaximumLength() As Integer
            Get
                Return _intMaximumLength
            End Get
            Set(ByVal Value As Integer)
                _intMaximumLength = Value
            End Set
        End Property

        Public Property MinimumLength() As Integer
            Get
                Return _intMinimumLength
            End Get
            Set(ByVal Value As Integer)
                _intMinimumLength = Value
            End Set
        End Property

#End Region

#Region " Constructor "

        Public Sub New(ByVal e As StringLengthValidatorAttribute, ByVal strPropertyName As String)
            MyBase.New(strPropertyName)
            _bolAllowNullString = e.AllowNullString
            _intMaximumLength = e.MaximumLength
            _intMinimumLength = e.MinimumLength
            MyBase.PropertyFriendlyName = e.PropertyFriendlyName
            MyBase.CustomMessage = e.CustomMessage
            MyBase.RuleSet = e.RuleSet

        End Sub

        Public Sub New(ByVal strPropertyName As String, ByVal intMinimumLength As Integer, ByVal intMaximumLength As Integer, Optional ByVal bolAllowNullString As Boolean = False)
            MyBase.New(strPropertyName)
            _intMinimumLength = intMinimumLength
            _intMaximumLength = intMaximumLength
            _bolAllowNullString = bolAllowNullString

        End Sub

#End Region

    End Class

End Namespace
