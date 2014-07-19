Imports System.Collections.ObjectModel

''' <summary>
''' This code supports having one ResourceDictionary for each skin.
''' If your application will have more than one, you'll need to
''' modify this code to use collections instead of a single 
''' resource dictionary.
''' </summary>
Class Application

#Region " Declarations "

    Private _objLoadedSkinResourceDictionary As ResourceDictionary

#End Region

#Region " Properties "

    Public ReadOnly Property MergedDictionaries() As Collection(Of ResourceDictionary)
        Get
            Return MyBase.Resources.MergedDictionaries
        End Get
    End Property

#End Region

#Region " Methods "

    Private Sub Application_Startup(ByVal sender As Object, ByVal e As System.Windows.StartupEventArgs) Handles Me.Startup

        For Each obj As ResourceDictionary In Me.MergedDictionaries

            If obj.Source IsNot Nothing AndAlso obj.Source.OriginalString.Contains("\Skins\") Then
                _objLoadedSkinResourceDictionary = obj
                'only have one to load up so we are out of here
                Exit Sub
            End If

        Next

    End Sub

    Public Sub ApplySkin(ByVal objSkinDictionaryUri As Uri)

        If String.IsNullOrEmpty(objSkinDictionaryUri.OriginalString) Then
            Throw New NullReferenceException("The skin dictionary URI OriginalString was null or empty.")
        End If

        Dim objNewSkinDictionary As ResourceDictionary = TryCast(Application.LoadComponent(objSkinDictionaryUri), ResourceDictionary)

        If objNewSkinDictionary Is Nothing Then
            Throw New NullReferenceException(String.Format("The {0} ResourceDictionary could not be loaded.", objSkinDictionaryUri.OriginalString))
        End If

        Me.MergedDictionaries.Remove(_objLoadedSkinResourceDictionary)
        _objLoadedSkinResourceDictionary = objNewSkinDictionary
        Me.MergedDictionaries.Add(objNewSkinDictionary)

    End Sub

#End Region

End Class
