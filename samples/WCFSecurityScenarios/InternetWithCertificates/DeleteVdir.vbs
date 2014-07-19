vDirName = "HelloIndigoWebHost_Certificates" ' Replace with the name of the virtual directory

Set shell = Wscript.CreateObject( "WScript.Shell" )

' Does this IIS application already exist in the metabase?
On Error Resume Next
Set objIIS = GetObject("IIS://localhost/W3SVC/1/Root/" & vDirName)
If Err.Number > 0 Then
    shell.Popup "An virtual directory named " & vDirName & " does not exists. ", 0, "Error", 16 ' 16 = Stop
    Wscript.quit
End If

Set objIIS = GetObject("IIS://localhost/W3SVC/1/Root")
objIIS.Delete "IISWebVirtualDir", vDirName

If Err.Number = 0 Then
    shell.Popup "Virtual directory deleted sucessfully", 0, "All done", 64 ' 64 = Information
Else
    shell.Popup Err.Description, 0, "Error", 16 ' 16 = Stop
End If

