'----------------------------------------------------------------------
'   This file is part of the Windows Workflow Foundation SDK Code Samples.
'  
'   Copyright (C) Microsoft Corporation.  All rights reserved.
'  
' This source code is intended only as a supplement to Microsoft
' Development Tools and/or on-line documentation.  See these other
' materials for detailed information regarding Microsoft code samples.
'  
' THIS CODE AND INFORMATION ARE PROVIDED AS IS WITHOUT WARRANTY OF ANY
' KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE
' IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
' PARTICULAR PURPOSE.
'----------------------------------------------------------------------

Imports System
Imports System.Threading
Imports System.Workflow.Runtime
Imports System.Workflow.Runtime.Hosting


Class WorkflowApplication
    Shared WaitHandle As New AutoResetEvent(False)

    Shared Sub Main()
        Using workflowRuntime As New WorkflowRuntime()
            Try
                Const connectionString As String = "Initial Catalog=SqlPersistenceService;Data Source=localhost;Integrated Security=SSPI;"
                workflowRuntime.AddService(New SqlWorkflowPersistenceService(connectionString))

                AddHandler workflowRuntime.WorkflowCompleted, AddressOf OnWorkflowCompleted
                AddHandler workflowRuntime.WorkflowTerminated, AddressOf OnWorkflowTerminated
                AddHandler workflowRuntime.WorkflowAborted, AddressOf OnWorkflowAborted

                Dim workflowInstance As WorkflowInstance
                workflowInstance = workflowRuntime.CreateWorkflow(GetType(Microsoft.Samples.Workflow.Compensation.PurchaseOrder))
                workflowInstance.Start()

                WaitHandle.WaitOne()

                workflowRuntime.StopRuntime()
            Catch ex As Exception
                If ex.InnerException IsNot Nothing Then
                    Console.WriteLine(ex.InnerException.Message)
                Else
                    Console.WriteLine(ex.Message)
                End If
                workflowRuntime.StopRuntime()
            End Try
        End Using
    End Sub

    Shared Sub OnWorkflowCompleted(ByVal sender As Object, ByVal e As WorkflowCompletedEventArgs)
        Console.WriteLine("Ending workflow...")
        WaitHandle.Set()
    End Sub

    Shared Sub OnWorkflowTerminated(ByVal sender As Object, ByVal e As WorkflowTerminatedEventArgs)
        Console.WriteLine(e.Exception.Message)
        WaitHandle.Set()
    End Sub

    Shared Sub OnWorkflowAborted(ByVal sender As Object, ByVal e As WorkflowEventArgs)
        Console.WriteLine("Ending workflow...")
        WaitHandle.Set()
    End Sub

End Class


