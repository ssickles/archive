#Region "File Header"
'
' COPYRIGHT:   Copyright 2009 
'              Infralution
'
#End Region

'' <summary>
'' Defines a type converter for translating enum values defined in this project
'' </summary>
Public Class LocalizedEnumConverter
    Inherits Infralution.Localization.Wpf.ResourceEnumConverter

    '' <summary>
    '' Create a new instance of the converter using translations from the given resource manager
    '' </summary>
    '' <param name="type"></param>
    Public Sub New(ByVal enumType As Type)
        MyBase.New(enumType, My.Resources.ResourceManager)
    End Sub

End Class