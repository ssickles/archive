vDirName = "HelloIndigoWebHost_Certificates" 

Set shell = Wscript.CreateObject( "WScript.Shell" )

' Get the name of the current directory
Set fso = WScript.CreateObject( "Scripting.FileSystemObject" )
vDirPath = fso.GetFolder( ".\HelloIndigoWebHost_Certificates" ).Path

' Does this IIS application already exist in the metabase?
On Error Resume Next
Set objIIS = GetObject( "IIS://localhost/W3SVC/1/Root/" & vDirName )
If Err.Number = 0 Then
    result = shell.Popup( "A virtual directory named " & vDirName & " already exists. " & vbCrLf & vbCrLf & "Would you like it re-mapped for this sample?", 0 ,"Remap Virtual Directory?", 4 + 32 )' 4 = YesNo & 32 = Question
    If result = 6 Then ' 6 = Yes
        DeleteVirtualDirectory vDirName 
    Else
        WScript.Quit
    End If
End If

' Get the Computer name using Wscript.Network and assign to IUSR to create IIS IUSR account name
Set network = WScript.CreateObject( "WScript.Network" )
owner = "IUSR_" & network.ComputerName
Set network = Nothing

'Using IIS Administration object , turn on script/execute permissions and define the virtual directory as an 'in-process application. 
Set objIIS  = GetObject( "IIS://localhost/W3SVC/1/Root" )
Set vDirObj = objIIS.Create( "IISWebVirtualDir", vDirName )

vDirObj.Path                  = vDirPath
vDirObj.AuthNTLM              = True
vDirObj.AccessRead            = True
vDirObj.AccessWrite           = True 
vDirObj.AccessScript          = True
vDirObj.AccessExecute         = True
vDirObj.AuthAnonymous         = True
vDirObj.AnonymousUserName     = owner
vDirObj.AnonymousPasswordSync = True
vDirObj.AppCreate True
vDirObj.SetInfo 

If Err.Number > 0 Then
    shell.Popup Err.Description, 0, "Error", 16 ' 16 = Stop
    WScript.Quit
Else
    shell.Popup "Virtual directory created." & vbCrLf & "setting folder permissions ..." , 1, "Status", 64 ' 64 = Information
End If

' Change necessary folder permissions using CACLS.exe
aclCmd = "cmd /c echo y| CACLS "
aclCmd = aclCmd & """" & vDirPath & """"
aclCmd = aclCmd & " /E /G " & owner & ":C"
rtc = shell.Run( aclCmd , 0, True )

aclCmd = "cmd /c echo y| CACLS "
aclCmd = aclCmd & """" & vDirPath & """"
aclCmd = aclCmd & " /E /G ""VS Developers"":C"
rtc = shell.Run( aclCmd , 0, True )

If Err.Number > 0 Then
    shell.Popup Err.Description, 0, "Error", 16 ' 16 = Stop
    WScript.Quit
Else
    res = vDirName & " has been created at" & vbCrLf & vDirPath
    shell.Popup res, 0, "All done", 64 ' 64 = Information
End If

Sub DeleteVirtualDirectory( NameOfVdir )

    Set iis = GetObject("IIS://localhost/W3SVC/1/Root")
    iis.Delete "IISWebVirtualDir", vDirName
    
    If Err.Number = 0 Then
        shell.Popup "Virtual directory deleted sucessfully", 1, "Status", 64 ' 64 = Information
    Else
        shell.Popup Err.Description, 0, "Error", 16 ' 16 = Stop
    End If

End Sub