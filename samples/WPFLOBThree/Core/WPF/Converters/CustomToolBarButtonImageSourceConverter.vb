
Namespace WPF

    <ValueConversion(GetType(String), GetType(BitmapImage))> _
    Public Class CustomToolBarButtonImageSourceConverter
        Implements IMultiValueConverter

#Region " Methods "

        Public Function Convert(ByVal values() As Object, ByVal targetType As System.Type, ByVal parameter As Object, ByVal culture As System.Globalization.CultureInfo) As Object Implements System.Windows.Data.IMultiValueConverter.Convert

            Dim strImage As String = String.Empty

            If CType(values(0), Boolean) Then
                strImage = values(1).ToString

            Else
                strImage = values(2).ToString
            End If

            If Not String.IsNullOrEmpty(strImage) Then

                Dim objURI As New Uri(strImage, UriKind.Relative)
                Return New BitmapImage(objURI)

            Else
                Return Nothing
            End If

        End Function

        Public Function ConvertBack(ByVal value As Object, ByVal targetTypes() As System.Type, ByVal parameter As Object, ByVal culture As System.Globalization.CultureInfo) As Object() Implements System.Windows.Data.IMultiValueConverter.ConvertBack
            Throw New NotSupportedException

        End Function

#End Region

    End Class

End Namespace
