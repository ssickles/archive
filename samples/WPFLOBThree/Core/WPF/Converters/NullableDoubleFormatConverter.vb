
Namespace WPF

    <ValueConversion(GetType(Nullable(Of Double)), GetType(String))> _
    Public Class NullableDoubleFormatConverter
        Implements IValueConverter

#Region " Methods "

        Public Function Convert(ByVal value As Object, ByVal targetType As Type, ByVal parameter As Object, ByVal culture As System.Globalization.CultureInfo) As Object Implements System.Windows.Data.IValueConverter.Convert

            If value Is Nothing Then
                Return ""
            End If

            Dim dblValue As Double

            If Double.TryParse(value.ToString, dblValue) Then

                Dim strFormatString As String = parameter.ToString

                If Not String.IsNullOrEmpty(strFormatString) Then
                    Return String.Format(culture, strFormatString, dblValue)

                Else
                    Return dblValue.ToString
                End If

            Else
                Return ""
            End If

        End Function

        Public Function ConvertBack(ByVal value As Object, ByVal targetType As Type, ByVal parameter As Object, ByVal culture As System.Globalization.CultureInfo) As Object Implements System.Windows.Data.IValueConverter.ConvertBack

            If value Is Nothing OrElse value.ToString.Trim.Length = 0 Then
                Return Nothing
            End If

            Dim dblValue As Double

            If Double.TryParse(value.ToString, dblValue) Then
                Return dblValue

            Else
                Return value
            End If

        End Function

#End Region

    End Class

End Namespace