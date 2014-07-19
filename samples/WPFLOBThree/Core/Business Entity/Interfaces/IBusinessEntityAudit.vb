
Namespace BusinessEntity

    Public Interface IBusinessEntityAudit

        Function ToAuditIDictionary(ByVal strDefaultValue As String, ByVal objIDictionary As IDictionary) As IDictionary

        Function ToAuditString(ByVal strDefaultValue As String, Optional ByVal strDelimiter As String = ", ") As String

        Function ToClassIDictionary(ByVal strDefaultValue As String, ByVal objIDictionary As IDictionary, Optional ByVal bolSortByPropertyName As Boolean = True) As IDictionary

        Function ToClassString(Optional ByVal strDelimiter As String = ", ", Optional ByVal bolSortByProperytName As Boolean = True) As String

    End Interface

End Namespace