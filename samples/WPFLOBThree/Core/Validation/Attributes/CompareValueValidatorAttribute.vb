
Namespace Validation

    <AttributeUsage(AttributeTargets.Property, AllowMultiple:=True, Inherited:=False)> _
    Public NotInheritable Class CompareValueValidatorAttribute
        Inherits BaseValidatorAttribute

#Region " Declarations "

        Private _bolIsRequired As Boolean = False
        Private _enumComparisionType As ComparisionType = ComparisionType.Equal
        Private _objCompareToValue As IComparable

#End Region

#Region " Properties "

        Public Property CompareToValue() As IComparable
            Get
                Return _objCompareToValue
            End Get
            Set(ByVal Value As IComparable)
                _objCompareToValue = Value
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

        Public Sub New(ByVal enumComparisionType As ComparisionType, ByVal objCompareToValue As Date, ByVal bolIsRequired As Boolean)
            _enumComparisionType = enumComparisionType
            _objCompareToValue = objCompareToValue
            _bolIsRequired = bolIsRequired

        End Sub

        Public Sub New(ByVal enumComparisionType As ComparisionType, ByVal objCompareToValue As Decimal, ByVal bolIsRequired As Boolean)
            _enumComparisionType = enumComparisionType
            _objCompareToValue = objCompareToValue
            _bolIsRequired = bolIsRequired

        End Sub

        Public Sub New(ByVal enumComparisionType As ComparisionType, ByVal objCompareToValue As Double, ByVal bolIsRequired As Boolean)
            _enumComparisionType = enumComparisionType
            _objCompareToValue = objCompareToValue
            _bolIsRequired = bolIsRequired

        End Sub

        Public Sub New(ByVal enumComparisionType As ComparisionType, ByVal objCompareToValue As Integer, ByVal bolIsRequired As Boolean)
            _enumComparisionType = enumComparisionType
            _objCompareToValue = objCompareToValue
            _bolIsRequired = bolIsRequired

        End Sub

        Public Sub New(ByVal enumComparisionType As ComparisionType, ByVal objCompareToValue As Long, ByVal bolIsRequired As Boolean)
            _enumComparisionType = enumComparisionType
            _objCompareToValue = objCompareToValue
            _bolIsRequired = bolIsRequired

        End Sub

        Public Sub New(ByVal enumComparisionType As ComparisionType, ByVal objCompareToValue As Short, ByVal bolIsRequired As Boolean)
            _enumComparisionType = enumComparisionType
            _objCompareToValue = objCompareToValue
            _bolIsRequired = bolIsRequired

        End Sub

        Public Sub New(ByVal enumComparisionType As ComparisionType, ByVal objCompareToValue As Single, ByVal bolIsRequired As Boolean)
            _enumComparisionType = enumComparisionType
            _objCompareToValue = objCompareToValue
            _bolIsRequired = bolIsRequired

        End Sub

        Public Sub New(ByVal enumComparisionType As ComparisionType, ByVal objCompareToValue As IComparable, ByVal bolIsRequired As Boolean)
            _enumComparisionType = enumComparisionType
            _objCompareToValue = objCompareToValue
            _bolIsRequired = bolIsRequired

        End Sub

#End Region

#Region " Methods "

        Public Overrides Function Create(ByVal strPropertyName As String) As Validator
            Return New Validator(AddressOf Validation.ComparisionValidationRules.CompareValueRule, New CompareValueRuleEventArgs(Me, strPropertyName))

        End Function

#End Region

    End Class

End Namespace
