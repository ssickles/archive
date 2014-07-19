
Namespace Validation

    Public Class RuleEventArgs
        Inherits EventArgs

#Region " Declarations "

        Protected _strBrokenRuleDescription As String = String.Empty
        Protected _strCustomMessage As String = String.Empty
        Protected _strPropertyFriendlyName As String = String.Empty
        Protected _strPropertyName As String = String.Empty
        Protected _strRuleSet As String = String.Empty

#End Region

#Region " Properties "

        Public Property BrokenRuleDescription() As String
            Get

                If String.IsNullOrEmpty(_strBrokenRuleDescription) Then
                    Return String.Format("Missing Broken Rule Description For {0}", Me.PropertyName)

                Else
                    Return _strBrokenRuleDescription
                End If

            End Get
            Set(ByVal Value As String)
                _strBrokenRuleDescription = Value
            End Set
        End Property

        Public Property CustomMessage() As String
            Get
                Return _strCustomMessage
            End Get
            Set(ByVal Value As String)
                _strCustomMessage = Value
            End Set
        End Property

        ''' <summary>
        ''' Friendly name of property for error reporting purposes.  If not provided, this will be generated from the property name by parsing the property name.
        ''' </summary>
        Public Property PropertyFriendlyName() As String
            Get
                Return _strPropertyFriendlyName
            End Get
            Set(ByVal Value As String)
                _strPropertyFriendlyName = Value
            End Set
        End Property

        Public Property PropertyName() As String
            Get
                Return _strPropertyName
            End Get
            Set(ByVal Value As String)
                _strPropertyName = Value
            End Set
        End Property

        Public Property RuleSet() As String
            Get
                Return _strRuleSet
            End Get
            Set(ByVal Value As String)
                _strRuleSet = Value
            End Set
        End Property

#End Region

#Region " Constructor "

        Public Sub New(ByVal strPropertyName As String)
            _strPropertyName = strPropertyName

        End Sub

        Public Sub New(ByVal strPropertyName As String, ByVal strPropertyFriendlyName As String, Optional ByVal strRuleSet As String = "", Optional ByVal strCustomMessage As String = "")
            _strPropertyName = strPropertyName
            _strPropertyFriendlyName = strPropertyFriendlyName
            _strRuleSet = strRuleSet
            _strCustomMessage = strCustomMessage

        End Sub

#End Region

#Region " Methods "

        Public Shared Function GetPropertyFriendlyName(ByVal e As RuleEventArgs) As String

            If String.IsNullOrEmpty(e.PropertyFriendlyName) Then
                'TODO developers you may not like this.  I do, so I put it in.  You can remove it at your option.
                Return StringFormatting.CamelCaseString.GetWords(e.PropertyName)

            Else
                Return e.PropertyFriendlyName
            End If

        End Function

        Public Overrides Function ToString() As String
            Return _strPropertyName

        End Function

#End Region

    End Class

End Namespace
