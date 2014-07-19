Imports VBIntegrationService.ServiceReference1

Module Module1

    Sub Main()
        Dim client As IntegratorClient
        client = New IntegratorClient()
        client.ReplaceIdentitySourceId("", "")
    End Sub

End Module
