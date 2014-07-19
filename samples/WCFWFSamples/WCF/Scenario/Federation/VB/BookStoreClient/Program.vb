' Copyright (c) Microsoft Corporation.  All Rights Reserved.

Imports System

Imports System.Windows.Forms

Imports System.Security.Permissions

<Assembly: SecurityPermission(SecurityAction.RequestMinimum, Execution:=True)> 
Namespace Microsoft.ServiceModel.Samples.Federation

    Class Program

        ''' <summary>
        ''' The main entry point for the application.
        ''' </summary>
        <STAThread()> _
        Public Shared Sub Main()

            Application.EnableVisualStyles()
            Application.Run(New BookStoreClientForm())

        End Sub

    End Class

End Namespace
