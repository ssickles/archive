
Namespace Validation

    <AttributeUsage(AttributeTargets.Property, AllowMultiple:=False, Inherited:=False)> _
    Public Class RangeValidatorAttribute
        Inherits BaseValidatorAttribute

#Region " Declarations "

        Private _bolIsRequired As Boolean = False
        Private _enumLowerRangeBoundaryType As RangeBoundaryType = RangeBoundaryType.Exclusive
        Private _enumUpperRangeBoundaryType As RangeBoundaryType = RangeBoundaryType.Exclusive
        Private _objLowerValue As IComparable
        Private _objUpperValue As IComparable

#End Region

#Region " Properties "

        Public Property IsRequired() As Boolean
            Get
                Return _bolIsRequired
            End Get
            Set(ByVal Value As Boolean)
                _bolIsRequired = Value
            End Set
        End Property

        Public Property LowerRangeBoundaryType() As RangeBoundaryType
            Get
                Return _enumLowerRangeBoundaryType
            End Get
            Set(ByVal Value As RangeBoundaryType)
                _enumLowerRangeBoundaryType = Value
            End Set
        End Property

        Public Property LowerValue() As IComparable
            Get
                Return _objLowerValue
            End Get
            Set(ByVal Value As IComparable)
                _objLowerValue = Value
            End Set
        End Property

        Public Property UpperRangeBoundaryType() As RangeBoundaryType
            Get
                Return _enumUpperRangeBoundaryType
            End Get
            Set(ByVal Value As RangeBoundaryType)
                _enumUpperRangeBoundaryType = Value
            End Set
        End Property

        Public Property UpperValue() As IComparable
            Get
                Return _objUpperValue
            End Get
            Set(ByVal Value As IComparable)
                _objUpperValue = Value
            End Set
        End Property

#End Region

#Region " Constructor "

        Public Sub New(ByVal enumLowerRangeBoundaryType As RangeBoundaryType, ByVal objLowerValue As Date, ByVal enumUpperRangeBoundaryType As RangeBoundaryType, ByVal objUpperValue As Date, ByVal bolIsRequired As Boolean)
            _enumLowerRangeBoundaryType = enumLowerRangeBoundaryType
            _objLowerValue = objLowerValue
            _enumUpperRangeBoundaryType = enumUpperRangeBoundaryType
            _objUpperValue = objUpperValue
            _bolIsRequired = bolIsRequired

        End Sub

        Public Sub New(ByVal enumLowerRangeBoundaryType As RangeBoundaryType, ByVal objLowerValue As Decimal, ByVal enumUpperRangeBoundaryType As RangeBoundaryType, ByVal objUpperValue As Decimal, ByVal bolIsRequired As Boolean)
            _enumLowerRangeBoundaryType = enumLowerRangeBoundaryType
            _objLowerValue = objLowerValue
            _enumUpperRangeBoundaryType = enumUpperRangeBoundaryType
            _objUpperValue = objUpperValue
            _bolIsRequired = bolIsRequired

        End Sub

        Public Sub New(ByVal enumLowerRangeBoundaryType As RangeBoundaryType, ByVal objLowerValue As Double, ByVal enumUpperRangeBoundaryType As RangeBoundaryType, ByVal objUpperValue As Double, ByVal bolIsRequired As Boolean)
            _enumLowerRangeBoundaryType = enumLowerRangeBoundaryType
            _objLowerValue = objLowerValue
            _enumUpperRangeBoundaryType = enumUpperRangeBoundaryType
            _objUpperValue = objUpperValue
            _bolIsRequired = bolIsRequired

        End Sub

        Public Sub New(ByVal enumLowerRangeBoundaryType As RangeBoundaryType, ByVal objLowerValue As Integer, ByVal enumUpperRangeBoundaryType As RangeBoundaryType, ByVal objUpperValue As Integer, ByVal bolIsRequired As Boolean)
            _enumLowerRangeBoundaryType = enumLowerRangeBoundaryType
            _objLowerValue = objLowerValue
            _enumUpperRangeBoundaryType = enumUpperRangeBoundaryType
            _objUpperValue = objUpperValue
            _bolIsRequired = bolIsRequired

        End Sub

        Public Sub New(ByVal enumLowerRangeBoundaryType As RangeBoundaryType, ByVal objLowerValue As Long, ByVal enumUpperRangeBoundaryType As RangeBoundaryType, ByVal objUpperValue As Long, ByVal bolIsRequired As Boolean)
            _enumLowerRangeBoundaryType = enumLowerRangeBoundaryType
            _objLowerValue = objLowerValue
            _enumUpperRangeBoundaryType = enumUpperRangeBoundaryType
            _objUpperValue = objUpperValue
            _bolIsRequired = bolIsRequired

        End Sub

        Public Sub New(ByVal enumLowerRangeBoundaryType As RangeBoundaryType, ByVal objLowerValue As Short, ByVal enumUpperRangeBoundaryType As RangeBoundaryType, ByVal objUpperValue As Short, ByVal bolIsRequired As Boolean)
            _enumLowerRangeBoundaryType = enumLowerRangeBoundaryType
            _objLowerValue = objLowerValue
            _enumUpperRangeBoundaryType = enumUpperRangeBoundaryType
            _objUpperValue = objUpperValue
            _bolIsRequired = bolIsRequired

        End Sub

        Public Sub New(ByVal enumLowerRangeBoundaryType As RangeBoundaryType, ByVal objLowerValue As Single, ByVal enumUpperRangeBoundaryType As RangeBoundaryType, ByVal objUpperValue As Single, ByVal bolIsRequired As Boolean)
            _enumLowerRangeBoundaryType = enumLowerRangeBoundaryType
            _objLowerValue = objLowerValue
            _enumUpperRangeBoundaryType = enumUpperRangeBoundaryType
            _objUpperValue = objUpperValue
            _bolIsRequired = bolIsRequired

        End Sub

        Public Sub New(ByVal enumLowerRangeBoundaryType As RangeBoundaryType, ByVal objLowerValue As IComparable, ByVal enumUpperRangeBoundaryType As RangeBoundaryType, ByVal objUpperValue As IComparable, ByVal bolIsRequired As Boolean)
            _enumLowerRangeBoundaryType = enumLowerRangeBoundaryType
            _objLowerValue = objLowerValue
            _enumUpperRangeBoundaryType = enumUpperRangeBoundaryType
            _objUpperValue = objUpperValue
            _bolIsRequired = bolIsRequired

        End Sub

#End Region

#Region " Methods "

        Public Overrides Function Create(ByVal strPropertyName As String) As Validator
            Return New Validator(AddressOf Validation.ComparisionValidationRules.InRangeRule, New RangeRuleEventArgs(Me, strPropertyName))

        End Function

#End Region

    End Class

End Namespace
