using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IdentityStream.BioAPI
{
    public interface ICapture: IDisposable
    {
        event EventHandler SourcePlaced;
        event EventHandler SourceRemoved;
        event EventHandler<TemplateCapturedEventArgs> TemplateCaptured;
        StartCapturingStatus StartCapturing();
        void StopCapturing();
    }

    public enum StartCapturingStatus
    {
        Success,
        NoScannersFound
    }
}
