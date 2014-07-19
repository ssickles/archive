#Region "File Header"
'
' COPYRIGHT:   Copyright 2009 
'              Infralution
'
#End Region
Imports System.ComponentModel

'' <summary>
'' Sample enum illustrating used of a localized enum type converter
'' </summary>
<TypeConverter(GetType(LocalizedEnumConverter))> _
Public Enum SampleEnum
    VerySmall
    Small
    Medium
    Large
    VeryLarge
End Enum
