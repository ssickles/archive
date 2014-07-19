using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using IdentityStream.BioAPI;
using Microsoft.Win32;
using Neurotec.Biometrics;
using Neurotec.DeviceManager;

namespace IdentityStream.BSPs.Capture.Neuro
{
    public class NeuroCapture : ISynchronizeInvoke, ICapture
    {
        private object _locker = new object();
        private bool _isScanning = false;
        private bool _isCapturing = false;
        private FPScannerMan _scannerMan;

        public NeuroCapture()
        {
            Startup();
            
            SystemEvents.PowerModeChanged += new PowerModeChangedEventHandler(PowerModeChanged);
        }

        /// <summary>
        /// Method for responding to the power mode changed event. For some reason, the scanner man
        /// object gets messed up when going into suspend mode. If we release all of the scanners
        /// when suspend mode is started and reaquire them when resuming, scanning can resume.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">Contains the mode to identify whether we're entering or leaving suspend mode.</param>
        private void PowerModeChanged(object sender, PowerModeChangedEventArgs e)
        {
            if (e.Mode == PowerModes.Suspend)
            {
                Shutdown();
            }
            else if (e.Mode == PowerModes.Resume)
            {
                Startup();
            }
        }

        private void Startup()
        {
            _scannerMan = new FPScannerMan(this);

            foreach (FPScanner scanner in _scannerMan.Scanners)
            {
                AddScanner(scanner);
            }

            _isScanning = true;
        }

        // Use interop to call the method necessary
        // to clean up the unmanaged resource.
        [DllImport("Kernel32")]
        private extern static Boolean CloseHandle(IntPtr handle);

        private void Shutdown()
        {
            if (_scannerMan != null)
            {
                foreach (FPScanner scanner in _scannerMan.Scanners)
                {
                    RemoveScanner(scanner);
                    CloseHandle(scanner.Handle);
                }

                _scannerMan.Dispose();
                _scannerMan = null;
            }

            _isScanning = false;
        }

        private void AddScanner(FPScanner Scanner)
        {
            try
            {
                if (!Scanner.IsCapturing)
                {
                    Scanner.FingerPlaced += new EventHandler(Scanner_FingerPlaced);
                    Scanner.FingerRemoved += new EventHandler(Scanner_FingerRemoved);
                    Scanner.ImageScanned += new FPScannerImageScannedEventHandler(Scanner_ImageScanned);
                    Scanner.StartCapturing();
                }
            }
            catch (Neurotec.NotActivatedException ex)
            {
                throw new Exception("A scanning module has not been registered. Please check your scanning services to make sure they have been started.", ex);
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Unable to start capturing using device: {0}.", Scanner.Id), ex);
            }
        }

        private void RemoveScanner(FPScanner Scanner)
        {
            try
            {
                if (Scanner.IsCapturing)
                {
                    Scanner.StopCapturing();
                    Scanner.FingerPlaced -= new EventHandler(Scanner_FingerPlaced);
                    Scanner.FingerRemoved -= new EventHandler(Scanner_FingerRemoved);
                    Scanner.ImageScanned -= new FPScannerImageScannedEventHandler(Scanner_ImageScanned);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Unable to stop capturing using device: {0}.", Scanner.Id), ex);
            }
        }

        private void Scanner_FingerPlaced(object sender, EventArgs e)
        {
            if (_isCapturing)
            {
                OnSourcePlaced();
            }
        }

        private void Scanner_FingerRemoved(object sender, EventArgs e)
        {
            if (_isCapturing)
            {
                OnSourceRemoved();
            }
        }

        private void Scanner_ImageScanned(object sender, FPScannerImageScannedEventArgs ea)
        {
            //the lock is used to prevent a race condition
            //if a template is captured, capturing is set to false
            //a new call to StartCapturing is required in order to get the next template
            lock (_locker)
            {
                if (_isCapturing)
                {
                    NfeExtractionStatus status;
                    Template template = null;
                    try
                    {
                        NFExtractor extractor = new NFExtractor();
                        NFRecord record = extractor.Extract(ea.Image, NFPosition.Unknown, NFImpressionType.LiveScanPlain, out status);
                        template = new Template(record.Save(), record.Minutiae.Count);
                        extractor.Dispose();
                    }
                    catch (Exception ex)
                    {
                        OnTemplateCaptured(new TemplateCapturedEventArgs(template, TemplateCapturedStatus.ExtractorException));
                        return;
                    }

                    if (status == NfeExtractionStatus.QualityCheckFailed)
                    {
                        OnTemplateCaptured(new TemplateCapturedEventArgs(template, TemplateCapturedStatus.QualityCheckFailed));
                        return;
                    }

                    if (status == NfeExtractionStatus.TooFewMinutiae)
                    {
                        OnTemplateCaptured(new TemplateCapturedEventArgs(template, TemplateCapturedStatus.MinutiaCountFailed));
                        return;
                    }

                    OnTemplateCaptured(new TemplateCapturedEventArgs(template, TemplateCapturedStatus.Success));
                }
            }
        }

        #region ICapture Members

        public event EventHandler SourcePlaced;

        private void OnSourcePlaced()
        {
            EventHandler obj = SourcePlaced;
            if (obj != null)
            {
                obj(this, EventArgs.Empty);
            }
        }

        public event EventHandler SourceRemoved;

        private void OnSourceRemoved()
        {
            EventHandler obj = SourceRemoved;
            if (obj != null)
            {
                obj(this, EventArgs.Empty);
            }
        }

        public event EventHandler<TemplateCapturedEventArgs> TemplateCaptured;

        private void OnTemplateCaptured(TemplateCapturedEventArgs e)
        {
            EventHandler<TemplateCapturedEventArgs> obj = TemplateCaptured;
            if (obj != null)
            {
                obj(this, e);
            }
        }

        public StartCapturingStatus StartCapturing()
        {
            lock (_locker)
            {
                _isCapturing = true;
            }

            if (_scannerMan.Scanners.Count == 0)
            {
                return StartCapturingStatus.NoScannersFound;
            }
            else
            {
                return StartCapturingStatus.Success;
            }
        }

        public void StopCapturing()
        {
            lock (_locker)
            {
                _isCapturing = false;
            }
        }

        #endregion

        #region ISynchronizeInvoke Members

        public IAsyncResult BeginInvoke(Delegate method, object[] args)
        {
            return null;
        }

        public object EndInvoke(IAsyncResult result)
        {
            return null;
        }

        public object Invoke(Delegate method, object[] args)
        {
            return method.DynamicInvoke(args);
        }

        public bool InvokeRequired
        {
            get
            {
                return _isScanning;
            }
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            Shutdown();

            Microsoft.Win32.SystemEvents.PowerModeChanged -= new Microsoft.Win32.PowerModeChangedEventHandler(PowerModeChanged);

            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
