
Namespace Validation

    Public Class RangeRuleEventArgs
        Inherits RuleEventArgs

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

        Public Sub New(ByVal e As RangeValidatorAttribute, ByVal strPropertyName As String)
            MyBase.New(strPropertyName)
            _enumLowerRangeBoundaryType = e.LowerRangeBoundaryType
            _enumUpperRangeBoundaryType = e.UpperRangeBoundaryType
            _objLowerValue = e.LowerValue
            _objUpperValue = e.UpperValue
            _bolIsRequired = e.IsRequired
            MyBase.PropertyFriendlyName = e.PropertyFriendlyName
            MyBase.CustomMessage = e.CustomMessage
            MyBase.RuleSet = e.RuleSet

        End Sub

        Public Sub New(ByVal strPropertyName As String, ByVal enumLowerRangeBoundaryType As RangeBoundaryType, ByVal objLowerValue As IComparable, ByVal enumUpperRangeBoundaryType As RangeBoundaryType, ByVal objUpperValue As IComparable, ByVal bolIsRequired As Boolean)
            MyBase.New(strPropertyName)
            _enumLowerRangeBoundaryType = enumLowerRangeBoundaryType
            _objLowerValue = objLowerValue
            _enumUpperRangeBoundaryType = enumUpperRangeBoundaryType
            _objUpperValue = objUpperValue
            _bolIsRequired = bolIsRequired

        End Sub

#End Region

    End Class

End Namespace
