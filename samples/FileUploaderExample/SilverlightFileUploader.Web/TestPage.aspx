<!--
Copyright 2003-2009 Virtual Chemistry, Inc. (VCI)
http://www.virtualchemistry.com
Using .Net, Silverlight, SharePoint and more to solve your tough problems in web-based data management.

Author: Peter Coley
-->
<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TestPage.aspx.cs" Inherits="SilverlightFileUploader.Web.TestPage" %>
<%@ Register TagPrefix="vci" Namespace="Vci.FileUploader" Assembly="Vci.FileUploader" %>
<%@ OutputCache Location="None" VaryByParam="None" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>File Uploader Test Page</title>
    
    <script type="text/javascript">
    
        window.onload = function() {

            // get the instance of UploaderControl that corresponds to the uploader1 server control
            var uploader1 = UploaderControl.getInstance("uploader1");
            
            // attach event handlers
            uploader1.attachEvent("onuploadstarted", uploader1_onuploadstarted);
            uploader1.attachEvent("onfileuploaded", uploader1_onfileuploaded);
        };
        
        // when an upload is started, set a context parameter that will be passed down to the ExampleFileProcessor
        function uploader1_onuploadstarted() {
            
            document.getElementById("divDebug").innerHTML = "";
            
            var uploader1 = UploaderControl.getInstance("uploader1");
            uploader1.setUserContext("some identifying information, like the user name, or whatever");            
        }
        
        // do some custom js code when each file has completed uploading
        function uploader1_onfileuploaded(fileGuid, fileName, fileSize) {
        
            document.getElementById("divDebug").innerHTML += "file uploaded: " + fileGuid + " " + fileName + " " + fileSize + "<br/>";
        }
        
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div style="font-family:Tahoma,arial;font-size:12px;">
            <div style="margin-bottom:20px;">
                <div style="font-weight:bold;font-size:16px;">Silverlight File Uploader Example Page</div>
                <div style="margin-top:10px;font-weight:bold;font-size:14px;color:Green;">Important!</div>
                <div style="margin-top:5px;">
                    Before using this example, ensure that the SandboxPath web.config app setting is properly configured; see the
                    comments in web.config and Sandbox.cs for more details.
                </div>
                <div style="margin-top:5px;margin-bottom:5px;">
                    Your files will currently be uploaded to:
                </div>
                <div style="font-weight:bold;font-size:14px;">
                    <%=_sandbox%>
                </div>
            </div>
            
            <div style="width:600px;height:400px;">
                <vci:UploaderControl ID="uploader1" runat="server" UploadedFileProcessorType="Vci.FileUploader.ExampleFileProcessor,Vci.FileUploader" />
            </div>
        
            <div style="margin-top:10px;">
                As files are uploaded, some info about each file will be written below:
            </div>
            <div id="divDebug" style="margin-top:5px;">
                
            </div>
        </div>
    </form>
</body>
</html>
