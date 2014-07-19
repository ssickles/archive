
Namespace Validation

    <AttributeUsage(AttributeTargets.Property, AllowMultiple:=False, Inherited:=False)> _
    Public NotInheritable Class StringLengthValidatorAttribute
        Inherits BaseValidatorAttribute

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

        Public Sub New(ByVal bolAllowNullString As Boolean, ByVal intMinimumLength As Integer, ByVal intMaximumLength As Integer)
            _bolAllowNullString = bolAllowNullString
            _intMinimumLength = intMinimumLength
            _intMaximumLength = intMaximumLength

        End Sub

        Public Sub New(ByVal intMaximumLength As Integer)
            _intMaximumLength = intMaximumLength

        End Sub

        Public Sub New(ByVal intMinimumLength As Integer, ByVal intMaximumLength As Integer)
            _intMinimumLength = intMinimumLength
            _intMaximumLength = intMaximumLength

        End Sub

#End Region

#Region " Methods "

        Public Overrides Function Create(ByVal strPropertyName As String) As Validator
            Return New Validator(AddressOf Validation.StringValidationRules.StringLengthRule, New StringLengthRuleEventArgs(Me, strPropertyName))

        End Function

#End Region

    End Class

End Namespace
