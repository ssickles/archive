
Namespace WPF

    ''' <summary>
    ''' Converter that returns the ValidationError.ErrorContent or the first inner exception message.
    ''' This converter is handly because it will return the message from an InnerException or the ErrorContent.  
    ''' Some data binding exceptions are package in a System.Reflection.TargetInvocationException and this converter will return the message from the actual exception rather than the useless TargetInvocationException message.
    ''' This converter usefullness is demonstrated when IDataErrorInfo property validation routines throw exceptions.  Those exceptions will bubble throught the WPF data binding pipeline and the InnerException will be displayed using this converter.
    ''' </summary>
    <ValueConversion(GetType(ValidationError), GetType(String))> _
    Public Class ValdiationErrorGetErrorMessageConverter
        Implements IValueConverter

#Region " Methods "

        Public Function Convert(ByVal value As Object, ByVal targetType As System.Type, ByVal parameter As Object, ByVal culture As System.Globalization.CultureInfo) As Object Implements System.Windows.Data.IValueConverter.Convert

            Dim sb As New System.Text.StringBuilder(1024)

            For Each objVB As ValidationError In DirectCast(value, System.Collections.ObjectModel.ReadOnlyObservableCollection(Of ValidationError))
                If objVB.Exception Is Nothing OrElse objVB.Exception.InnerException Is Nothing Then
                    sb.AppendLine(objVB.ErrorContent.ToString)

                Else
                    sb.AppendLine(objVB.Exception.InnerException.Message)
                End If

            Next

            'remove the last line feed
            If sb.Length > 2 Then
                sb.Length -= 2
            End If

            Return sb.ToString
        End Function

        Public Function ConvertBack(ByVal value As Object, ByVal targetType As System.Type, ByVal parameter As Object, ByVal culture As System.Globalization.CultureInfo) As Object Implements System.Windows.Data.IValueConverter.ConvertBack
            Throw New NotSupportedException
        End Function

#End Region

    End Class


End Namespace