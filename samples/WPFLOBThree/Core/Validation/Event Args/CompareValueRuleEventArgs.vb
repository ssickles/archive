
Namespace Validation

    Public Class CompareValueRuleEventArgs
        Inherits RuleEventArgs

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

        Public Sub New(ByVal e As CompareValueValidatorAttribute, ByVal strPropertyName As String)
            MyBase.New(strPropertyName)
            _enumComparisionType = e.ComparisionType
            _objCompareToValue = e.CompareToValue
            _bolIsRequired = e.IsRequired
            MyBase.PropertyFriendlyName = e.PropertyFriendlyName
            MyBase.CustomMessage = e.CustomMessage
            MyBase.RuleSet = e.RuleSet

        End Sub

        Public Sub New(ByVal strPropertyName As String, ByVal enumComparisionType As ComparisionType, ByVal objCompareToValue As IComparable, ByVal bolIsRequired As Boolean)
            MyBase.New(strPropertyName)
            _enumComparisionType = enumComparisionType
            _objCompareToValue = objCompareToValue
            _bolIsRequired = bolIsRequired

        End Sub

#End Region

    End Class

End Namespace
