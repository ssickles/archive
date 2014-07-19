/*
Copyright 2003-2009 Virtual Chemistry, Inc. (VCI)
http://www.virtualchemistry.com
Using .Net, Silverlight, SharePoint and more to solve your tough problems in web-based data management.

Author: Peter Coley
*/

using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.IO;
using System.Windows.Threading;

namespace Vci.Silverlight.FileUploader
{
    public class HttpFileUploader : IFileUploader
    {
        private UserFile _file;
        private long _dataLength;
        private long _dataSent;
        private string _uploadedFileProcessorType;
        private string _contextParam;
        private bool _canceling = false;
        private bool _processing = false;

        private long ChunkSize = 1024 * 1024 * 3; // 3 MB in bytes
        private string UploadUrl;

        public HttpFileUploader(UserFile file, string httpHandlerPath, long ChunkSizeMB)
        {
            _file = file;

            if (ChunkSizeMB > 0)
                ChunkSize = 1024 * 1024 * ChunkSizeMB;

            _dataLength = _file.FileSize;
            _dataSent = 0;

            if (string.IsNullOrEmpty(httpHandlerPath))
                throw new ArgumentException("A valid http handler was not specified.");

            UriBuilder builder = new UriBuilder(Application.Current.Host.Source.Scheme, Application.Current.Host.Source.Host,
                Application.Current.Host.Source.Port, httpHandlerPath);

            UploadUrl = builder.Uri.ToString();
        }

        #region IFileUploader Members

        /// <summary>
        /// Start the file upload
        /// </summary>
        /// <param name="UploadedFileProcessorType">Type for an object that will do post-processing on the server</param>
        /// <param name="ContextParam">an additional context parameter</param>
        public void StartUpload(string UploadedFileProcessorType, string ContextParam)
        {
            _uploadedFileProcessorType = UploadedFileProcessorType;
            _contextParam = ContextParam;
            StartUpload(false, false);
        }

        public event EventHandler UploadFinished;
        public event EventHandler UploadCanceled;
        public event EventHandler<UploadErrorOccurredEventArgs> UploadErrorOccurred;
        public event EventHandler<UploadDataSentArgs> UploadDataSent;
        public event EventHandler UploadProcessingStarted;

        #endregion

        private void OnUploadFinished()
        {
            EventHandler handler = UploadFinished;
            if (handler != null)
                handler(this, null);
        }

        private void OnUploadCanceled()
        {
            EventHandler handler = UploadCanceled;
            if (handler != null)
                handler(this, null);
        }

        private void OnUploadErrorOccurred(UploadErrorOccurredEventArgs e)
        {
            EventHandler<UploadErrorOccurredEventArgs> handler = UploadErrorOccurred;
            if (handler != null)
                handler(this, e);
        }

        private void OnUploadDataSent(UploadDataSentArgs e)
        {
            EventHandler<UploadDataSentArgs> handler = UploadDataSent;
            if (handler != null)
                handler(this, e);
        }

        private void OnUploadProcessingStarted()
        {
            EventHandler handler = UploadProcessingStarted;
            if (handler != null)
                handler(this, null);
        }

        private void StartUpload(bool cancel, bool process)
        {
            bool isFirstChunk = _dataSent == 0;
            bool isLastChunk = _dataLength - _dataSent <= ChunkSize;

            UriBuilder httpHandlerUrlBuilder = new UriBuilder(UploadUrl);
            httpHandlerUrlBuilder.Query = string.Format("{0}guid={1}&first={2}&last={3}&cancel={4}&process={5}&name={6}{7}{8}",
                string.IsNullOrEmpty(httpHandlerUrlBuilder.Query) ? "" : httpHandlerUrlBuilder.Query.Remove(0, 1) + "&",
                _file.Guid,
                isFirstChunk,
                isLastChunk,
                cancel,
                process,
                _file.FileName,
                string.IsNullOrEmpty(_uploadedFileProcessorType) ? "" : "&processor=" + _uploadedFileProcessorType,
                string.IsNullOrEmpty(_contextParam) ? "" : "&context=" + _contextParam);

            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(httpHandlerUrlBuilder.Uri);
            webRequest.Method = "POST";
            webRequest.BeginGetRequestStream(new AsyncCallback(WriteToStreamCallback), webRequest);
        }

        private void WriteToStreamCallback(IAsyncResult asynchronousResult)
        {
            HttpWebRequest webRequest = (HttpWebRequest)asynchronousResult.AsyncState;
            using (Stream requestStream = webRequest.EndGetRequestStream(asynchronousResult))
            {
                try
                {
                    // read the next chunk

                    // if a cancel was requested, don't read any data at all -- just send an empty request that can do clean-up on the server
                    bool cancelRequested = _canceling;
                    if (!cancelRequested)
                    {
                        lock (_file.SyncState)
                        {
                            cancelRequested = _file.SyncState.CancelRequested;
                        }
                    }

                    // don't read any data if this is sending the request to do post-processing on the file
                    if (!cancelRequested && !_processing)
                    {
                        byte[] buffer = new Byte[4096];
                        int bytesRead = 0;
                        int tempTotal = 0;

                        // set the start position to where we left off
                        _file.FileStream.Position = _dataSent;

                        while ((bytesRead = _file.FileStream.Read(buffer, 0, buffer.Length)) != 0 && tempTotal + bytesRead < ChunkSize)
                        {
                            requestStream.Write(buffer, 0, bytesRead);
                            requestStream.Flush();

                            _dataSent += bytesRead;
                            tempTotal += bytesRead;

                            // notify that data was sent
                            OnUploadDataSent(new UploadDataSentArgs(_dataSent));

                            // check if a cancel has been requested at each iteration, and stop reading data immediately
                            lock (_file.SyncState)
                            {
                                if (_file.SyncState.CancelRequested)
                                    break;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    // if an error occurs while reading the file, abort the upload and raise the error event
                    try
                    {
                        // use a try block, b/c it's possible that the webRequest object itself caused the exception
                        webRequest.Abort();
                    }
                    catch { }

                    OnUploadErrorOccurred(new UploadErrorOccurredEventArgs(ex.ToString()));
                    return;
                }
            }

            // get the response from the HttpHandler
            webRequest.BeginGetResponse(new AsyncCallback(ReadHttpResponseCallback), webRequest);
        }

        private void ReadHttpResponseCallback(IAsyncResult asynchronousResult)
        {
            bool error = false;
            string errorMsg = null;

            //Check if the response is OK
            try
            {
                HttpWebRequest webRequest = (HttpWebRequest)asynchronousResult.AsyncState;
                HttpWebResponse webResponse = (HttpWebResponse)webRequest.EndGetResponse(asynchronousResult);
                if (webResponse.StatusCode != HttpStatusCode.OK)
                {
                    error = true;
                    errorMsg = webResponse.StatusDescription;
                }
                else
                {
                    using (Stream s = webResponse.GetResponseStream())
                    using (StreamReader sr = new StreamReader(s))
                        errorMsg = sr.ReadToEnd();
                    if (!string.IsNullOrEmpty(errorMsg))
                        error = true;
                }
            }
            catch (Exception ex)
            {
                error = true;
                errorMsg = ex.ToString();
            }

            if (error)
            {
                OnUploadErrorOccurred(new UploadErrorOccurredEventArgs(errorMsg));
            }
            else
            {
                if (_canceling)
                    // cancel completed successfully
                    OnUploadCanceled();
                else if (_processing)
                    // processing completed successfully
                    OnUploadFinished();
                else
                {
                    // Not finished yet, continue uploading, unless a cancel has been requested
                    bool cancel = false;
                    lock (_file.SyncState)
                    {
                        cancel = _file.SyncState.CancelRequested;
                    }

                    // set a flag to indicate that we are attempting to cancel the upload; this will tell the code above to check for
                    // a successful cancel, and trigger the UploadCanceled event (if an error occurs, the error event is triggered instead).
                    if (cancel)
                    {
                        _canceling = true;
                        // send a request with the cancel flag set to true, which gives the server an opportunity to clean up
                        StartUpload(true, false);
                    }
                    else
                    {
                        if (_dataSent >= _dataLength)
                        {
                            // if all data has been sent, see if there is a file processor and send a request to do the processing;
                            // the next file can begin uploading while a file is being processed, but the file is not set to Finished
                            // until processing has completed
                            if (!string.IsNullOrEmpty(_uploadedFileProcessorType))
                            {
                                // send a request that will do post-processing
                                _processing = true;
                                StartUpload(false, true);
                                OnUploadProcessingStarted();
                            }
                            else
                                // upload finished, and there is no post-processing type specified, so we're done
                                OnUploadFinished();
                        }
                        else
                            // send the next chunk of data
                            StartUpload(false, false);
                    }
                }
            }
        }
    }
}
