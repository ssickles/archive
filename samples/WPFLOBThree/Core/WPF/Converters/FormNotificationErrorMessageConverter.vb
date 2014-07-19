
Namespace WPF

    <ValueConversion(GetType(String), GetType(String), ParameterType:=GetType(String))> _
    Public Class FormNotificationErrorMessageConverter
        Implements IMultiValueConverter

#Region " Methods "

        Public Function Convert(ByVal values() As Object, ByVal targetType As System.Type, ByVal parameter As Object, ByVal culture As System.Globalization.CultureInfo) As Object Implements System.Windows.Data.IMultiValueConverter.Convert

            Dim sb As New System.Text.StringBuilder(1024)

            For Each obj As Object In values
                If obj.ToString.Length > 0 Then
                    sb.AppendLine(obj.ToString)
                End If
            Next

            Return sb.ToString
        End Function

        Public Function ConvertBack(ByVal value As Object, ByVal targetTypes() As System.Type, ByVal parameter As Object, ByVal culture As System.Globalization.CultureInfo) As Object() Implements System.Windows.Data.IMultiValueConverter.ConvertBack
            Throw New NotSupportedException

        End Function

#End Region

    End Class

End Namespace