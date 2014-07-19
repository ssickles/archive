
Namespace WPF

    <ValueConversion(GetType(Nullable(Of Integer)), GetType(String))> _
    Public Class NullableIntegerFormatConverter
        Implements IValueConverter

#Region " Methods "

        Public Function Convert(ByVal value As Object, ByVal targetType As Type, ByVal parameter As Object, ByVal culture As System.Globalization.CultureInfo) As Object Implements System.Windows.Data.IValueConverter.Convert

            If value Is Nothing Then
                Return ""
            End If

            Dim intValue As Integer

            If Integer.TryParse(value.ToString, intValue) Then

                Dim strFormatString As String = parameter.ToString

                If Not String.IsNullOrEmpty(strFormatString) Then
                    Return String.Format(culture, strFormatString, intValue)

                Else
                    Return intValue.ToString
                End If

            Else
                Return ""
            End If

        End Function

        Public Function ConvertBack(ByVal value As Object, ByVal targetType As Type, ByVal parameter As Object, ByVal culture As System.Globalization.CultureInfo) As Object Implements System.Windows.Data.IValueConverter.ConvertBack

            If value Is Nothing OrElse value.ToString.Trim.Length = 0 Then
                Return Nothing
            End If

            Dim intValue As Integer

            If Integer.TryParse(value.ToString, intValue) Then
                Return intValue

            Else
                Return value
            End If

        End Function

#End Region

    End Class

End Namespace