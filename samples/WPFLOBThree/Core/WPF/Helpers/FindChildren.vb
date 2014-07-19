Imports System.Windows

Namespace WPF

    Public Class FindChilden

#Region " Methods "

        Public Shared Function FindVisualChild(Of childItem As DependencyObject)(ByVal objParent As DependencyObject) As childItem

            For i As Integer = 0 To VisualTreeHelper.GetChildrenCount(objParent) - 1

                Dim child As DependencyObject = VisualTreeHelper.GetChild(objParent, i)

                If child IsNot Nothing AndAlso TypeOf child Is childItem Then
                    Return DirectCast(child, childItem)

                Else

                    Dim childOfChild As childItem = FindVisualChild(Of childItem)(child)

                    If childOfChild IsNot Nothing Then
                        Return childOfChild
                    End If

                End If

            Next

            Return Nothing

        End Function

#End Region

    End Class

End Namespace