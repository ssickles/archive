
Namespace WPF

    <ValueConversion(GetType(String), GetType(Boolean))> _
    Public Class StringLengthToBooleanConverter
        Implements IValueConverter

#Region " Methods "

        Public Function Convert(ByVal value As Object, ByVal targetType As System.Type, ByVal parameter As Object, ByVal culture As System.Globalization.CultureInfo) As Object Implements System.Windows.Data.IValueConverter.Convert

            If value Is Nothing OrElse String.IsNullOrEmpty(CType(value, String)) Then
                Return False

            Else
                Return True
            End If

        End Function

        Public Function ConvertBack(ByVal value As Object, ByVal targetType As System.Type, ByVal parameter As Object, ByVal culture As System.Globalization.CultureInfo) As Object Implements System.Windows.Data.IValueConverter.ConvertBack
            Throw New NotSupportedException

        End Function

#End Region

    End Class

End Namespace