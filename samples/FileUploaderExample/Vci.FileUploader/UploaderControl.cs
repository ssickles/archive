/*
Copyright 2003-2009 Virtual Chemistry, Inc. (VCI)
http://www.virtualchemistry.com
Using .Net, Silverlight, SharePoint and more to solve your tough problems in web-based data management.

Author: Peter Coley
*/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.IO;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

using Vci.Core;

namespace Vci.FileUploader
{
    /// <summary>
    /// Convenient wrapper on the silverlight uploader control that facilitates passing initialization parameters,
    /// and initializes javascript object wrapper that can be used to interact with the control.
    /// </summary>
    public class UploaderControl : WebControl
    {
        private bool _multiSelect = true;

        private string _scriptPath = "~/scripts/uploadercontrol.js";
        private string _xapFile = "~/ClientBin/SilverlightFileUploader.xap";
        private string _handlerPath = "~/UploaderControlHandler.ashx";

        /// <summary>
        /// Maximum number of concurrent uploads.  Defaults to 2.
        /// </summary>
        public int MaxUploads { get; set; }

        /// <summary>
        /// Maximum size of an uploaded file in KB.  Default is 102400 KB (100 MB).
        /// </summary>
        public int MaxFileSizeKB { get; set; }

        /// <summary>
        /// Chunk size in MB.  Defaults to 3 MB.
        /// </summary>
        public int ChunkSizeMB { get; set; }

        /// <summary>
        /// Filter for the types of files that can be uploaded.
        /// </summary>
        public string FileFilter { get; set; }

        /// <summary>
        /// If true, multiple files can be uploaded and chosen at once.  Defaults to true.
        /// </summary>
        public bool MultiSelect { get { return _multiSelect; } set { _multiSelect = value; } }

        /// <summary>
        /// If specified, this is a class that implements IUploadedFileProcessor.  This Type's ProcessFile
        /// method will be invoked on the server for each file that is successfully uploaded.
        /// </summary>
        public string UploadedFileProcessorType { get; set; }

        /// <summary>
        /// Location of the uploadercontrol.js file; defaults to ~/scripts/uploadercontrol.js.
        /// </summary>
        public string ScriptPath { get { return _scriptPath; } set { _scriptPath = value; } }

        /// <summary>
        /// Path to the xap file, defaults to ~/ClientBin/SilverlightFileUploader.xap.
        /// </summary>
        public string XapPath { get { return _xapFile; } set { _xapFile = value; } }

        /// <summary>
        /// Path to the http handler that will receive uploaded file data, defaults to ~/UploaderControlHandler.ashx.
        /// </summary>
        public string HandlerPath { get { return _handlerPath; } set { _handlerPath = value; } }

        /// <summary>
        /// The user context is passed as ContextParam to the upload file processor.  You may also set this value in
        /// javascript via the setUserContext property before uploading has begun.
        /// </summary>
        public string UserContext { get; set; }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            ClientScriptManager csm = Page.ClientScript;

            if (!csm.IsClientScriptIncludeRegistered("uploadercontrol"))
                csm.RegisterClientScriptInclude("uploadercontrol", ResolveClientUrl(_scriptPath));
        }

        protected override void Render(HtmlTextWriter writer)
        {
            ClientScriptManager csm = Page.ClientScript;

            string source = ParseUrlProperty(_xapFile);

            Dictionary<string, string> initParamList = new Dictionary<string, string>();
            initParamList["UploadHandlerPath"] = ParseUrlProperty(_handlerPath);

            if (MaxUploads > 0)
                initParamList["MaxUploads"] = MaxUploads.ToString();

            if (MaxFileSizeKB > 0)
                initParamList["MaxFileSizeKB"] = MaxFileSizeKB.ToString();

            if (ChunkSizeMB > 0)
                initParamList["ChunkSizeMB"] = ChunkSizeMB.ToString();

            if (!string.IsNullOrEmpty(FileFilter))
                initParamList["FileFilter"] = FileFilter;

            if (!string.IsNullOrEmpty(UploadedFileProcessorType))
                initParamList["UploadedFileProcessorType"] = UploadedFileProcessorType;

            initParamList["MultiSelect"] = _multiSelect.ToString();

            string sInitParams = SilverlightUtility.BuildInitParamsString(initParamList);

            // NOTE: this uses "windowless" mode by default; feel free to change this as you see fit... in an app like
            // this that is not particularly intensive on the UI side of things, using windowless mode makes embedding
            // the control into your pages much more manageable, particularly in firefox -- avoids a lot of the issues
            // with setting dimensions on containers and using display:none

            writer.Write(@"
<object id=""silverlight" + this.ID + @""" data=""data:application/x-silverlight-2,"" type=""application/x-silverlight-2"" width=""100%"" height=""100%"">
    <param name=""source"" value=""" + source + @"""/>
    <param name=""minRuntimeVersion"" value=""2.0.31005.0"" />
    <param name=""autoUpgrade"" value=""true"" />
    <param name=""initParams"" value=""" + sInitParams + @""" />
    <param name=""windowless"" value=""true"" />
    <param name=""onLoad"" value=""UploaderControl_onLoad_" + this.ID + @""" />
    <a href=""http://go.microsoft.com/fwlink/?LinkID=124807"" style=""text-decoration: none;"">
        <img src=""http://go.microsoft.com/fwlink/?LinkId=108181"" alt=""Get Microsoft Silverlight"" style=""border-style: none""/>
    </a>
</object>
<iframe style='visibility:hidden;height:0;width:0;border:0px'></iframe>
");

            string script = @"
function UploaderControl_onLoad_" + this.ID + @"() {
    UploaderControl.getInstance(""" + this.ID + @""").onLoad();
}

function UploaderControl_uploadStarted_" + this.ID + @"(sender, e) {
    UploaderControl.getInstance(""" + this.ID + @""").uploadStarted(sender, e);    
}

function UploaderControl_singleFileUploadFinished_" + this.ID + @"(sender, e) {
    UploaderControl.getInstance(""" + this.ID + @""").singleFileUploadFinished(sender, e);    
}

function UploaderControl_allFilesFinished_" + this.ID + @"(sender, e) {
    UploaderControl.getInstance(""" + this.ID + @""").allFilesFinished(sender, e);    
}

function UploaderControl_errorOccurred_" + this.ID + @"(sender, e) {
    UploaderControl.getInstance(""" + this.ID + @""").errorOccurred(sender, e);    
}

UploaderControl.getInstance(""" + this.ID + @""").initialize(" + (UserContext == null ? "null" : "\"" + UserContext + "\"") + @");
";
            csm.RegisterStartupScript(this.GetType(), this.ID, script, true);
        }

        /// <summary>
        /// Replaces ~ with the application path
        /// </summary>
        /// <param name="Url"></param>
        /// <returns></returns>
        private string ParseUrlProperty(string Url)
        {
            string ret = Url;
            if (Url.StartsWith("~"))
            {
                string appPath = Page.Request.ApplicationPath;
                if (appPath == "/") appPath = "";
                ret = appPath + Url.Substring(1);
            }
            return ret;
        }
    }
}
