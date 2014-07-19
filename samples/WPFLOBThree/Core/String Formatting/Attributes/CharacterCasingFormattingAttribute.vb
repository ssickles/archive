
Namespace StringFormatting

    <AttributeUsage(AttributeTargets.Property, AllowMultiple:=False, Inherited:=False)> _
    Public Class CharacterCasingFormattingAttribute
        Inherits Attribute

#Region " Declarations "

        Private _enumCharacterCasing As CharacterCasing = CharacterCasing.None

#End Region

#Region " Properties "

        Public Property CharacterCasing() As CharacterCasing
            Get
                Return _enumCharacterCasing
            End Get
            Set(ByVal Value As CharacterCasing)
                _enumCharacterCasing = Value
            End Set
        End Property

#End Region

#Region " Constructors and Load & Unload "

        Public Sub New(ByVal enumCharacterCasing As CharacterCasing)
            _enumCharacterCasing = enumCharacterCasing

        End Sub

#End Region

    End Class

End Namespace
