using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IdentityStream.BioAPI
{
    public class ImageCapturedEventArgs: EventArgs
    {
        public ImageCapturedEventArgs(ImageCapturedStatus Status)
            : base()
        {
            this.Status = Status;
        }

        public ImageCapturedStatus Status { get; private set; }
    }

    public enum ImageCapturedStatus
    {
        Success,
        QualityCheckFailed,
        MinutiaCountFailed,
        ExtractorException
    }
}
