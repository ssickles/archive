
Namespace Validation

    <AttributeUsage(AttributeTargets.Property, AllowMultiple:=True, Inherited:=False)> _
    Public NotInheritable Class ComparePropertyValidatorAttribute
        Inherits BaseValidatorAttribute

#Region " Declarations "

        Private _bolIsRequired As Boolean = False
        Private _enumComparisionType As ComparisionType = ComparisionType.Equal
        Private _strCompareToPropertyName As String = String.Empty

#End Region

#Region " Properties "

        Public Property CompareToPropertyName() As String
            Get
                Return _strCompareToPropertyName
            End Get
            Set(ByVal Value As String)
                _strCompareToPropertyName = Value
            End Set
        End Property

        Public Property ComparisionType() As ComparisionType
            Get
                Return _enumComparisionType
            End Get
            Set(ByVal Value As ComparisionType)
                _enumComparisionType = Value
            End Set
        End Property

        Public Property IsRequired() As Boolean
            Get
                Return _bolIsRequired
            End Get
            Set(ByVal Value As Boolean)
                _bolIsRequired = Value
            End Set
        End Property

#End Region

#Region " Constructor "

        Public Sub New(ByVal enumComparisionType As ComparisionType, ByVal strCompareToPropertyName As String, ByVal bolIsRequired As Boolean)
            _enumComparisionType = enumComparisionType
            _strCompareToPropertyName = strCompareToPropertyName
            _bolIsRequired = bolIsRequired

        End Sub

#End Region

#Region " Methods "

        Public Overrides Function Create(ByVal strPropertyName As String) As Validator
            Return New Validator(AddressOf Validation.ComparisionValidationRules.ComparePropertyRule, New ComparePropertyRuleEventArgs(Me, strPropertyName))

        End Function

#End Region

    End Class

End Namespace
