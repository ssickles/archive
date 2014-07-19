
Namespace WPF

    'This forum post discusses why this converter is required.
    'http://forums.microsoft.com/MSDN/ShowPost.aspx?PostID=1829709&SiteID=1

    <ValueConversion(GetType(Object), GetType(Object))> _
    Public Class ForceReReadConverter
        Implements IValueConverter

#Region " Methods "

        Public Function Convert(ByVal value As Object, ByVal targetType As System.Type, ByVal parameter As Object, ByVal culture As System.Globalization.CultureInfo) As Object Implements System.Windows.Data.IValueConverter.Convert
            Return value

        End Function

        Public Function ConvertBack(ByVal value As Object, ByVal targetType As System.Type, ByVal parameter As Object, ByVal culture As System.Globalization.CultureInfo) As Object Implements System.Windows.Data.IValueConverter.ConvertBack
            Return value

        End Function

#End Region

    End Class

End Namespace