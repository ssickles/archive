/*
Copyright 2003-2009 Virtual Chemistry, Inc. (VCI)
http://www.virtualchemistry.com
Using .Net, Silverlight, SharePoint and more to solve your tough problems in web-based data management.

Author: Peter Coley
*/

if(typeof(UploaderControl) == "undefined" || UploaderControl == null) {

UploaderControls = {};

UploaderControl = function(id) {
	
	if(arguments.length > 0)
		this.init(id);
};

UploaderControl.getInstance = function(id) {
	var x = UploaderControls[id];
	if(x == null)
		x = UploaderControls[id] = new UploaderControl(id);
	return x;
};

UploaderControl.eventObjects = {};

UploaderControl.prototype = {
	
	id : null,
	silverlightClientId : null,
	loaded : false,
	uploadInProgress : false,
	userContext : null,
	
	init : function(id) {
	
	    this.id = id;
	    this.events = {};
	    
		UploaderControl.eventObjects[this.id] = this;
		
		//
		// These are the available js events that you can handle on the UploaderControl.  See attachEvent for details on how to use.
		//
		this.events["onuploadstarted"] = null; // the user has started uploading some files; this is your last chance to set the user context
		this.events["onfileuploaded"] = null; // (fileGuid, fileName, fileSize) called for each file when it has completed uploading
		this.events["onallfilesuploaded"] = null; // all files have completed uploading (or been canceled or removed from the list)
		this.events["onerror"] = null; // (errorMessage)
	},

	initialize : function(userContext) {

        this.userContext = userContext;
        this.silverlightClientId = "silverlight" + this.id;

        // it's possible that the control can finish loading before the load handling js function is written to the page, so do a check
        if(!this.loaded) {
            try {
                var obj = document.getElementById(this.silverlightClientId);
                if(obj && obj.Content && obj.Content.Files)
                    this.onLoad();
            } catch(ex) {}
        }
    },
    
    //
    // Public methods.
    //
    
    setUserContext : function(ctx) {
    /// <summary>
    /// A string that is sent as the Context parameter to server-side file upload processors.  Must be set before
    /// file uploading has begun.
    /// </summary>
    
        this.userContext = ctx;
    },

	attachEvent : function(eventName, func, context) {
	/// <summary>
	/// Public method for attaching event handlers to this control.
	/// </summary>
	/// <param name="eventName">the name of the event</param>
	/// <param name="func">function pointer to the event handler</param>
	/// <param name="context">context to use when calling func (becomes the 'this' keyword within your function)</param>
	/// <example>
	/// var uploader = UploaderControl.getInstance("upload1");
	/// uploader.attachEvent("onfileuploaded", myFunc);
	/// ...
	/// function myFunc(fileGuid, fileName, fileSize) {
	///     ...
	/// }
	/// </example>

		if(context == null) context = this;
		if(func == null)
			this.events[eventName] = null;
		else
			this.events[eventName] = {func : func, context : context};
	},
    
    
    //
    // below are some example utility methods that access methods on the silverlight object
    //
    
    clearFileList : function() {
    
        if(this.loaded) {
            var obj = document.getElementById(this.silverlightClientId);
            try {
                if(obj && obj.Content && obj.Content.UploaderControl)
                    obj.Content.UploaderControl.ClearFileList();
            } catch(ex) {}
        }
    },
    
    getFileCount : function() {

        var ret = -1;
        if(this.loaded) {
            var obj = document.getElementById(this.silverlightClientId);
            try {
                if(obj && obj.Content && obj.Content.Files)
                    ret = obj.Content.Files.TotalFilesSelected;
            } catch(ex) {}
        }
        
        return ret;
    },
    
    
    //
    // private methods for hooking stuff up to the silverlight control
    //
    
    onLoad : function() {
    
        this.loaded = true;
        
        var obj = document.getElementById(this.silverlightClientId);
        obj.Content.UploaderControl.UploadStarted = window["UploaderControl_uploadStarted_" + this.id];
        obj.Content.Files.SingleFileUploadFinished = window["UploaderControl_singleFileUploadFinished_" + this.id];
        obj.Content.Files.AllFilesFinished = window["UploaderControl_allFilesFinished_" + this.id];
        obj.Content.Files.ErrorOccurred = window["UploaderControl_errorOccurred_" + this.id];
    },
    
    uploadStarted : function(sender, e) {
    
        this.invokeEventHandler("onuploadstarted");
        
        this.uploadInProgress = true;
        var obj = document.getElementById(this.silverlightClientId);
        obj.Content.Files.UserContextParameter = this.userContext;        
    },
    
    singleFileUploadFinished : function(sender, e) {
    
        this.invokeEventHandler("onfileuploaded", e.FileGuid, e.FileName, e.FileSize);
    },
    
    allFilesFinished : function(sender, e) {
    
        this.uploadInProgress = false;
        this.invokeEventHandler("onallfilesuploaded");
    },
    
    errorOccurred : function(sender, e) {
    
        this.invokeEventHandler("onerror", e.ErrorMessage);
    },
    
	getEventHandler : function(eventName) {
		return this.events[eventName];
	},
	
	invokeEventHandler : function(eventName) {

		if((handler = this.events[eventName]) != null) {
			var args = new Array();
			for(var i = 1; i < arguments.length; i++)
				args.push(arguments[i]);
			return handler.func.apply(handler.context, args);
		}
		
		return null;
	}    
};

}
