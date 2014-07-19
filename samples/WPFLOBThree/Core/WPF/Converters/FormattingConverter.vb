
Namespace WPF

    <ValueConversion(GetType(Object), GetType(String))> _
    Public Class FormattingConverter
        Implements IValueConverter

#Region " Methods "

        ''' <summary>
        '''     Takes a number, date or string and formats the value according to the paramenter and culture
        ''' </summary>
        ''' <param name="value" type="Object">
        '''     <para>
        '''         Number, date or string
        '''     </para>
        ''' </param>
        ''' <param name="targetType" type="System.Type">
        '''     <para>
        '''         This is automatically passed in and is the type of the target (calling) UI Element.
        '''     </para>
        ''' </param>
        ''' <param name="parameter" type="Object">
        '''     <para>
        '''        A .Net formatting code.  
        '''        Example, for currency pass the familar "{0:c}" like this : \{0:c\} or like this {}{0:c}
        '''        Both ways work.
        '''        You can also include a string like this, ' My Money \{0:c\}'
        '''        If you do not use the {}{0:c} syntax, then it is necessary to escape the "{" and "}" otherwise parser will get confused because "{" and "}" is used as delimiters in the Binding statement.
        '''        See the remarks below for an example of the Binding statement.
        '''     </para>
        ''' </param>
        ''' <param name="culture" type="System.Globalization.CultureInfo">
        '''     <para>
        '''         Not required to be passed in xaml code. WPF automatically passes the current culture info for you.
        '''     </para>
        ''' </param>
        ''' <returns>
        '''     The value, formatted according to the parameter.
        ''' </returns>
        ''' <remarks>
        ''' Usage Examples:
        ''' Text="{Binding Path=BirthDay, Converter={StaticResource FormattingConverter}, ConverterParameter=\{0:M\}, Mode=Default}"
        ''' Text="{Binding Path=BirthDay, Converter={StaticResource FormattingConverter}, ConverterParameter=\{0:R\}, Mode=Default}"
        ''' Text="{Binding Path=BirthDay, Converter={StaticResource FormattingConverter}, ConverterParameter='{}{0:ddd, d MMM yyyy} is my day!', Mode=Default}"
        ''' </remarks>
        Public Function Convert(ByVal value As Object, ByVal targetType As System.Type, ByVal parameter As Object, ByVal culture As System.Globalization.CultureInfo) As Object Implements System.Windows.Data.IValueConverter.Convert

            If parameter IsNot Nothing Then

                Dim strFormatString As String = parameter.ToString

                If Not String.IsNullOrEmpty(strFormatString) Then
                    Return String.Format(culture, strFormatString, value)
                End If

            End If

            Return value.ToString

        End Function

        ''' <summary>
        ''' Attempts to convert the value back using a type specific TypeConverter
        ''' </summary>
        Public Function ConvertBack(ByVal value As Object, ByVal targetType As System.Type, ByVal parameter As Object, ByVal culture As System.Globalization.CultureInfo) As Object Implements System.Windows.Data.IValueConverter.ConvertBack

            Dim objTypeConverter As System.ComponentModel.TypeConverter = System.ComponentModel.TypeDescriptor.GetConverter(targetType)
            Dim objReturnValue As Object = Nothing

            If objTypeConverter.CanConvertFrom(value.[GetType]()) Then

                Try
                    objReturnValue = objTypeConverter.ConvertFrom(value)

                Catch ex As Exception
                    'HACK - developers you have two options here.
                    '1.  Return nothing which in effect wipes out the offending text in the binding control
                    '
                    'objReturnValue = Nothing
                    '
                    '
                    '2.  Return the value.  Then allow the data binding to fail further down the chain, ie. when it attempts to bind to the entity object.  This failure will be handled by the data binding pipeline.
                    '
                    objReturnValue = value
                End Try

            End If

            Return objReturnValue

        End Function

#End Region

    End Class

End Namespace