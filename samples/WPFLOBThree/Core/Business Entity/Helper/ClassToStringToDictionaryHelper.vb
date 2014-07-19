Imports System.ComponentModel
Imports System.Text
Imports Core.Validation
Imports Core.BusinessEntity

Namespace BusinessEntity

    ''' <summary>
    ''' Class provides helper methods to create audit log strings, complete class property values listings in either String or IDictionary formats.
    ''' </summary>
    Public Class ClassToStringToDictionaryHelper

#Region " Methods "

        ''' <summary>
        ''' Used to generate an Dictionary(Of String, String) for each property decorated with the AuditAttribute.
        ''' </summary>
        ''' <typeparam name="T">Class Type</typeparam>
        ''' <param name="obj">Instance of class to audit</param>
        ''' <param name="strDefaultValue">Default message if no class propeties are decorated</param>
        ''' <returns>IDictionary</returns>
        Public Shared Function AuditToIDictionary(Of T)(ByVal obj As T, ByVal strDefaultValue As String) As IDictionary

            Dim objDictionary As New Dictionary(Of String, String)
            Return AuditToIDictionary(obj, strDefaultValue, objDictionary)

        End Function

        ''' <summary>
        ''' Used to generate an Dictionary(Of String, String) for each property decorated with the AuditAttribute.
        ''' </summary>
        ''' <typeparam name="T">Class Type</typeparam>
        ''' <param name="obj">Instance of class to audit</param>
        ''' <param name="strDefaultValue">Default message if no class propeties are decorated</param>
        ''' <param name="objIDictionary">Pass an IDictionary object that needs to be populated.  Typically this would be the Data property of an Exception object.</param>
        ''' <returns>IDictionary</returns>
        Public Shared Function AuditToIDictionary(Of T)(ByVal obj As T, ByVal strDefaultValue As String, ByVal objIDictionary As IDictionary) As IDictionary

            Dim objList As New List(Of SortablePropertyBasket)

            For Each prop As PropertyDescriptor In TypeDescriptor.GetProperties(obj)

                Dim atr As AuditAttribute = TryCast(prop.Attributes(GetType(AuditAttribute)), AuditAttribute)

                If atr IsNot Nothing Then

                    Dim atrCaption As CaptionAttribute = TryCast(prop.Attributes(GetType(CaptionAttribute)), CaptionAttribute)
                    Dim strCaption As String

                    If atrCaption IsNot Nothing Then
                        strCaption = atrCaption.Caption

                    Else
                        strCaption = StringFormatting.CamelCaseString.GetWords(prop.DisplayName)
                    End If

                    objList.Add(New SortablePropertyBasket(atr.SortOrder, prop.DisplayName, strCaption, prop.GetValue(obj).ToString))
                End If

            Next

            If objList.Count > 0 Then
                objList.Sort()

                For Each objPropertyBasket As SortablePropertyBasket In objList
                    objIDictionary.Add(objPropertyBasket.PropertyName, objPropertyBasket.Value)
                Next

            Else
                objIDictionary.Add("DefaultValue", strDefaultValue)
            End If

            Return objIDictionary

        End Function

        ''' <summary>
        ''' This function returns a string of each property decorated with the AuditAttribute.  The string displays the property name, property friendly name and property value.  This function is typically used when a developer need to make an audit log entry.  It provides a very simple method to generate these messages.
        ''' </summary>
        ''' <typeparam name="T">Class Type</typeparam>
        ''' <param name="obj">Instance of the class</param>
        ''' <param name="strDefaultValue">If no properties have been decorated with the AuditAttribute, then this message is displayed.</param>
        ''' <param name="strDelimiter">What delimiter do you want between each property.  Defaults to comma.  Could use vbcrlf, etc.</param>
        Public Shared Function AuditToString(Of T)(ByVal obj As T, ByVal strDefaultValue As String, Optional ByVal strDelimiter As String = ", ") As String

            Dim sb As New StringBuilder(2048)
            Dim objList As New List(Of SortablePropertyBasket)

            For Each prop As PropertyDescriptor In TypeDescriptor.GetProperties(obj)

                Dim atr As AuditAttribute = TryCast(prop.Attributes(GetType(AuditAttribute)), AuditAttribute)

                If atr IsNot Nothing Then

                    Dim atrCaption As CaptionAttribute = TryCast(prop.Attributes(GetType(CaptionAttribute)), CaptionAttribute)
                    Dim strCaption As String

                    If atrCaption IsNot Nothing Then
                        strCaption = atrCaption.Caption
                    Else
                        strCaption = StringFormatting.CamelCaseString.GetWords(prop.DisplayName)

                    End If

                    If Not prop.GetValue(obj) Is Nothing Then
                        objList.Add(New SortablePropertyBasket(atr.SortOrder, prop.DisplayName, strCaption, prop.GetValue(obj).ToString))
                    Else
                        objList.Add(New SortablePropertyBasket(atr.SortOrder, prop.DisplayName, strCaption, "Null"))
                    End If

                End If

            Next

            If objList.Count > 0 Then
                objList.Sort()

                For Each objPropertyBasket As SortablePropertyBasket In objList
                    sb.Append(objPropertyBasket.ToString)
                    sb.Append(strDelimiter)
                Next

                If sb.Length > strDelimiter.Length Then
                    sb.Length -= strDelimiter.Length
                End If

            Else
                sb.Append(strDefaultValue)
            End If

            Return sb.ToString

        End Function

        ''' <summary>
        ''' Used to generate a Dictionary(Of String, String) for each property in the entity.  Dictionary is property name, property value.
        ''' </summary>
        ''' <typeparam name="T">Class Type</typeparam>
        ''' <param name="obj">Instance of class to audit</param>
        ''' <param name="strDefaultValue">Default message if no class propeties are decorated</param>
        ''' <param name="bolSortByPropertyName">Normally sorts the output by property name.  To leave in ordinal order, set to False</param>        
        ''' <returns>IDictionary</returns>
        Public Shared Function ClassToIDictionary(Of T)(ByVal obj As T, ByVal strDefaultValue As String, Optional ByVal bolSortByPropertyName As Boolean = True) As IDictionary

            Dim objDictionary As New Dictionary(Of String, String)
            Return ClassToIDictionary(obj, strDefaultValue, objDictionary, bolSortByPropertyName)

        End Function

        ''' <summary>
        ''' Used to generate a Dictionary(Of String, String) for each property in the entity.  Dictionary is property name, property value.  This method signature allows a IDictionary object to be passed in.  This feature is useful when generating a name value pair dictionary to store in an Exception object's Data property.
        ''' </summary>
        ''' <typeparam name="T">Class Type</typeparam>
        ''' <param name="obj">Instance of class to audit</param>
        ''' <param name="strDefaultValue">Default message if no class propeties are decorated</param>
        ''' <param name="objIDictionary">Pass an IDictionary object that needs to be populated.  Typicaly this would be the Data property of an Exception object.</param>
        ''' <param name="bolSortByPropertyName">Normally sorts the output by property name.  To leave in ordinal order, set to False</param>
        ''' <returns>IDictionary</returns>
        Public Shared Function ClassToIDictionary(Of T)(ByVal obj As T, ByVal strDefaultValue As String, ByVal objIDictionary As IDictionary, Optional ByVal bolSortByPropertyName As Boolean = True) As IDictionary

            Dim objList As New List(Of SortablePropertyBasket)

            For Each prop As PropertyDescriptor In TypeDescriptor.GetProperties(obj)

                Dim atr As AuditAttribute = TryCast(prop.Attributes(GetType(AuditAttribute)), AuditAttribute)
                objList.Add(New SortablePropertyBasket(1, prop.DisplayName, String.Empty, prop.GetValue(obj).ToString))
            Next

            If objList.Count > 0 Then

                If bolSortByPropertyName Then
                    objList.Sort()
                End If

                For Each objPropertyBasket As SortablePropertyBasket In objList
                    objIDictionary.Add(objPropertyBasket.PropertyName, objPropertyBasket.Value)
                Next

            Else
                objIDictionary.Add("DefaultValue", strDefaultValue)
            End If

            Return objIDictionary

        End Function

        ''' <summary>
        ''' Returns a string with each property and value in the class.  The string displays the property name, property friendly name and property value.
        ''' </summary>
        ''' <typeparam name="T">Class Type</typeparam>
        ''' <param name="obj">Instance of the class</param>
        ''' <param name="strDelimiter">What delimiter do you want between each property.  Defaults to comma.  Could use vbcrlf, etc.</param>
        ''' <param name="bolSortByPropertyName">Normally sorts the output by property name.  To leave in ordinal order, set to False</param>
        Public Shared Function ClassToString(Of T)(ByVal obj As T, Optional ByVal strDelimiter As String = ", ", Optional ByVal bolSortByPropertyName As Boolean = True) As String

            Dim sb As New StringBuilder(4096)
            Dim objList As New List(Of SortablePropertyBasket)

            For Each prop As PropertyDescriptor In TypeDescriptor.GetProperties(obj)

                Dim atr As AuditAttribute = TryCast(prop.Attributes(GetType(AuditAttribute)), AuditAttribute)
                Dim intSortOrder As Integer = 1

                If atr IsNot Nothing Then
                    intSortOrder = atr.SortOrder
                End If

                Dim atrCaption As CaptionAttribute = TryCast(prop.Attributes(GetType(CaptionAttribute)), CaptionAttribute)
                Dim strCaption As String

                If atrCaption IsNot Nothing Then
                    strCaption = atrCaption.Caption

                Else
                    strCaption = StringFormatting.CamelCaseString.GetWords(prop.DisplayName)
                End If

                If Not prop.GetValue(obj) Is Nothing Then
                    objList.Add(New SortablePropertyBasket(intSortOrder, prop.DisplayName, strCaption, prop.GetValue(obj).ToString))
                Else
                    objList.Add(New SortablePropertyBasket(intSortOrder, prop.DisplayName, strCaption, "Null"))
                End If

            Next

            If objList.Count > 0 Then

                If bolSortByPropertyName Then
                    objList.Sort()
                End If

                For Each objPropertyBasket As SortablePropertyBasket In objList
                    sb.Append(objPropertyBasket.ToString)
                    sb.Append(strDelimiter)
                Next

                If sb.Length > strDelimiter.Length Then
                    sb.Length -= strDelimiter.Length
                End If

            Else
                sb.Append("Class has no properties")
            End If

            Return sb.ToString

        End Function

#End Region

#Region " Private Class(s) "

        Private Class SortablePropertyBasket
            Implements IComparable(Of SortablePropertyBasket)

            Private _intSortOrder As Integer
            Private _strFriendlyName As String = String.Empty
            Private _strPropertyName As String = String.Empty
            Private _strValue As String = String.Empty

            Public ReadOnly Property FriendlyName() As String
                Get
                    Return _strFriendlyName
                End Get
            End Property

            Public ReadOnly Property PropertyName() As String
                Get
                    Return _strPropertyName
                End Get
            End Property

            Public ReadOnly Property SortOrder() As Integer
                Get
                    Return _intSortOrder
                End Get
            End Property

            Public ReadOnly Property Value() As String
                Get
                    Return _strValue
                End Get
            End Property

            Public Sub New(ByVal intSortOrder As Integer, ByVal strPropertyName As String, ByVal strFriendlyName As String, ByVal strValue As String)

                If strValue Is Nothing Then
                    strValue = String.Empty
                End If

                _intSortOrder = intSortOrder
                _strPropertyName = strPropertyName
                _strFriendlyName = strFriendlyName
                _strValue = strValue

            End Sub

            Public Function CompareTo(ByVal other As SortablePropertyBasket) As Integer Implements System.IComparable(Of SortablePropertyBasket).CompareTo
                Return String.Compare(String.Concat(Microsoft.VisualBasic.Right(String.Concat("0000", Me.SortOrder.ToString), 4), Me.PropertyName), String.Concat(Microsoft.VisualBasic.Right(String.Concat("0000", other.SortOrder.ToString), 4), other.PropertyName))

            End Function

            Public Overrides Function ToString() As String

                If _strFriendlyName.Length = 0 Then
                    Return String.Format("{0} = {1}", Me.PropertyName, Me.Value)

                Else
                    Return String.Format("{0} ( {1} ) = {2}", Me.FriendlyName, Me.PropertyName, Me.Value)
                End If

            End Function

        End Class

#End Region

    End Class

End Namespace
