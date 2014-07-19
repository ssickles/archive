using System;
using System.Collections.Generic;
using System.Threading;
using IdentityStream.BioAPI.Properties;
using Neurotec;
using Neurotec.Biometrics;
using Neurotec.Licensing;

namespace IdentityStream.BioAPI
{
    public class CaptureSession
    {
        private object _locker = new object();
        private AutoResetEvent _captureReset = new AutoResetEvent(false);
        private bool _finished = false;
        private List<ICapture> _bsps = new List<ICapture>();
        private int _numSamples = 0;
        private List<Template> _capturedSamples;

        /// <summary>
        /// This internal method establishes a biometric session.
        /// Each BSP provided will be dynamically instantiated.
        /// </summary>
        /// <param name="BSPs"></param>
        /// <exception cref="System.ArgumentException" />
        /// <exception cref="IdentityStream.BioAPI.BspLoadException" />
        /// <exception cref="IdentityStream.BioAPI.LicenseNotAvailableException" />
        /// <exception cref="IdentityStream.BioAPI.LicensingException" />
        internal CaptureSession(List<Type> BSPs)
        {
            ObtainLicense("SingleComputerLicense:VFExtractor");

            foreach (Type type in BSPs)
            {
                if (!typeof(ICapture).IsAssignableFrom(type))
                {
                    throw new ArgumentException(string.Format("The provided type ({0}) does not implement ICapture.", type.ToString()));
                }
                try
                {
                    _bsps.Add((ICapture)Activator.CreateInstance(type));
                }
                catch (Exception ex)
                {
                    throw new BspLoadException(type, "Unable to load the BSP.", ex);
                }
            }
        }

        /// <summary>
        /// This method is responsible for acquiring a license from the Neuro license server
        /// </summary>
        /// <param name="LicenseName"></param>
        /// <exception cref="IdentityStream.BioAPI.LicenseNotAvailableException" />
        /// <exception cref="IdentityStream.BioAPI.LicensingException" />
        private void ObtainLicense(string LicenseName)
        {
            try
            {
                bool available = NLicense.Obtain(Settings.Default.LicenseServer, Settings.Default.LicensePort, LicenseName);

                if (!available)
                {
                    throw new LicenseNotAvailableException(LicenseName, "License could not be obtained.");
                }
            }
            catch (NeurotecException ex)
            {
                throw new LicensingException("Error while obtaining license. Please check if Neurotec License Manager is running.", ex);
            }
        }

        /// <summary>
        /// This method is solely responsible for creating a single template from a set of 3 or more templates.
        /// It uses the Neuro SDK to create a "better" template from the set provided.
        /// </summary>
        /// <param name="Templates"></param>
        /// <returns>IdentityStream.BioAPI.Template</returns>
        /// <exception cref="IdentityStream.BioAPI.GeneralizationException" />
        private Template Generalize(List<Template> Templates)
        {
            NFRecord[] records = new NFRecord[Templates.Count];
            for (int i = 0; i < Templates.Count; i++)
            {
                records[i] = new NFRecord(Templates[i].Data);
            }

            int baseTemplateIndex;
            try
            {
                NFExtractor extractor = new NFExtractor();
                NFRecord record = extractor.Generalize(records, out baseTemplateIndex);
                extractor.Dispose();
                return new Template(record.Save(), record.Minutiae.Count);
            }
            catch (Exception ex)
            {
                throw new GeneralizationException(Templates.Count, "Unable to generalize templates.", ex);
            }
        }

        /// <summary>
        /// The Capture method is a blocking call that waits for templates to be scanned.
        /// To stop blocking and return control of the thread, call Abort from another thread.
        /// </summary>
        /// <param name="SamplesPerTemplate"></param>
        /// <returns>IdentityStream.BioAPI.Template</returns>
        /// <exception cref="IdentityStream.BioAPI.GeneralizationException" />
        /// <exception cref="IdentityStream.BioAPI.BspStartupException" />
        public Template Capture(int SamplesPerTemplate)
        {
            Template result = null;
            _finished = false;
            _numSamples = SamplesPerTemplate;
            _capturedSamples = new List<Template>();
            foreach (ICapture bsp in _bsps)
            {
                bsp.SourcePlaced += new EventHandler(bsp_SourcePlaced);
                bsp.SourceRemoved += new EventHandler(bsp_SourceRemoved);
                bsp.TemplateCaptured += new EventHandler<TemplateCapturedEventArgs>(bsp_TemplateCaptured);
                try
                {
                    bsp.StartCapturing();
                }
                catch (Exception ex)
                {
                    throw new BspStartupException(bsp.GetType(), "Unable to successfully call StartCapturing() for the BSP.", ex);
                }
            }

            //block until all samples have been captured and generalized
            _captureReset.WaitOne();

            if (_capturedSamples.Count == _numSamples)
            {
                if (_capturedSamples.Count == 1)
                {
                    result = _capturedSamples[0];
                }
                else
                {
                    result = Generalize(_capturedSamples);
                }
            }

            foreach (ICapture bsp in _bsps)
            {
                bsp.StopCapturing();
                bsp.SourcePlaced -= bsp_SourcePlaced;
                bsp.SourceRemoved -= bsp_SourceRemoved;
                bsp.TemplateCaptured -= bsp_TemplateCaptured;
            }

            return result;
        }

        public void Abort()
        {
            lock (_locker)
            {
                _finished = true;
                _captureReset.Set();
            }
        }

        private void bsp_TemplateCaptured(object sender, TemplateCapturedEventArgs e)
        {
            lock (_locker)
            {
                if (!_finished)
                {
                    switch (e.Status)
                    {
                        case TemplateCapturedStatus.ExtractorException:
                            OnImageCaptured(new ImageCapturedEventArgs(ImageCapturedStatus.ExtractorException));
                            break;
                        case TemplateCapturedStatus.MinutiaCountFailed:
                            OnImageCaptured(new ImageCapturedEventArgs(ImageCapturedStatus.MinutiaCountFailed));
                            break;
                        case TemplateCapturedStatus.QualityCheckFailed:
                            OnImageCaptured(new ImageCapturedEventArgs(ImageCapturedStatus.QualityCheckFailed));
                            break;
                        case TemplateCapturedStatus.Success:
                            if (e.Template.MinutiaCount < Settings.Default.MinimumMinutiaCount)
                            {
                                OnImageCaptured(new ImageCapturedEventArgs(ImageCapturedStatus.MinutiaCountFailed));
                            }
                            else
                            {
                                OnImageCaptured(new ImageCapturedEventArgs(ImageCapturedStatus.Success));
                                _capturedSamples.Add(e.Template);
                                if (_capturedSamples.Count == _numSamples)
                                {
                                    _finished = true;
                                    _captureReset.Set();
                                }
                            }
                            break;
                    }
                }
            }
        }

        private void bsp_SourceRemoved(object sender, EventArgs e)
        {
            OnSourceRemoved();
        }

        private void bsp_SourcePlaced(object sender, EventArgs e)
        {
            OnSourcePlaced();
        }

        #region Events

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

        public event EventHandler<ImageCapturedEventArgs> ImageCaptured;

        private void OnImageCaptured(ImageCapturedEventArgs e)
        {
            EventHandler<ImageCapturedEventArgs> obj = ImageCaptured;
            if (obj != null)
            {
                obj(this, e);
            }
        }

        #endregion

        internal void Dispose()
        {
            foreach (ICapture bsp in _bsps)
            {
                bsp.Dispose();
            }
            _bsps.Clear();
        }
    }

    public class BspLoadException : Exception
    {
        public BspLoadException(Type Type, string Message, Exception ex)
            : base(Message, ex)
        {
            this.Type = Type;
        }

        public Type Type { get; private set; }
    }

    public class BspStartupException : Exception
    {
        public BspStartupException(Type Type, string Message, Exception ex)
            : base(Message, ex)
        {
            this.Type = Type;
        }

        public Type Type { get; private set; }
    }

    public class LicensingException : Exception
    {
        public LicensingException(string Message, Exception ex)
            : base(Message, ex)
        {

        }
    }

    public class LicenseNotAvailableException : Exception
    {
        public LicenseNotAvailableException(string LicenseName, string Message)
            : base(Message)
        {
            this.LicenseName = LicenseName;
        }

        public string LicenseName { get; private set; }
    }

    public class GeneralizationException : Exception
    {
        public GeneralizationException(int NumTemplates, string Message, Exception ex)
            : base(Message, ex)
        {
            this.NumTemplates = NumTemplates;
        }

        public int NumTemplates { get; private set; }
    }
}
