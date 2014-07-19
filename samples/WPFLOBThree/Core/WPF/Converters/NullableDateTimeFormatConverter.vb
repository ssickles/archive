Namespace WPF

    <ValueConversion(GetType(Nullable(Of DateTime)), GetType(String))> _
    Class NullableDateTimeFormatConverter
        Implements IValueConverter

#Region " Methods "

        Public Function Convert(ByVal value As Object, ByVal targetType As Type, ByVal parameter As Object, ByVal culture As System.Globalization.CultureInfo) As Object Implements System.Windows.Data.IValueConverter.Convert

            If value Is Nothing Then
                Return ""
            End If

            Dim datDate As DateTime

            If DateTime.TryParse(value.ToString, datDate) Then

                Dim strFormatString As String = parameter.ToString

                If Not String.IsNullOrEmpty(strFormatString) Then
                    Return String.Format(culture, strFormatString, datDate)

                Else
                    Return datDate.ToString
                End If

            Else
                Return ""
            End If

        End Function

        Public Function ConvertBack(ByVal value As Object, ByVal targetType As Type, ByVal parameter As Object, ByVal culture As System.Globalization.CultureInfo) As Object Implements System.Windows.Data.IValueConverter.ConvertBack

            If value Is Nothing OrElse value.ToString.Trim.Length = 0 Then
                Return Nothing
            End If

            Dim datDate As DateTime

            If DateTime.TryParse(value.ToString, datDate) Then
                Return datDate

            ElseIf value.ToString = "." OrElse String.Compare(value.ToString, "t", True) = 0 Then
                Return DateTime.Today

            Else
                Return value
            End If

        End Function

#End Region

    End Class

End Namespace