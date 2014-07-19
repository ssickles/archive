
Class ApplicationMainWindow

#Region " Methods "

    Private Sub OnMenuItemExitClick(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs)
        Me.Close()

    End Sub

    Private Sub OnViewMenuItemClick(ByVal sender As Object, ByVal e As RoutedEventArgs)

        Dim objClicked As MenuItem = CType(e.OriginalSource, MenuItem)

        For Each mi As MenuItem In CType(sender, MenuItem).Items
            mi.IsChecked = mi Is objClicked
        Next

        CType(Application.Current, Application).ApplySkin(New Uri(TryCast(objClicked.CommandParameter, String), UriKind.Relative))

    End Sub

#End Region

End Class
