
Namespace Validation

    Public Class ComparePropertyRuleEventArgs
        Inherits RuleEventArgs

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

        Public Sub New(ByVal e As ComparePropertyValidatorAttribute, ByVal strPropertyName As String)
            MyBase.New(strPropertyName)
            _enumComparisionType = e.ComparisionType
            _strCompareToPropertyName = e.CompareToPropertyName
            _bolIsRequired = e.IsRequired
            MyBase.PropertyFriendlyName = e.PropertyFriendlyName
            MyBase.CustomMessage = e.CustomMessage
            MyBase.RuleSet = e.RuleSet

        End Sub

        Public Sub New(ByVal strPropertyName As String, ByVal enumComparisionType As ComparisionType, ByVal strCompareToPropertyName As String, ByVal bolIsRequired As Boolean)
            MyBase.New(strPropertyName)
            _enumComparisionType = enumComparisionType
            _strCompareToPropertyName = strCompareToPropertyName
            _bolIsRequired = bolIsRequired

        End Sub

#End Region

    End Class

End Namespace
