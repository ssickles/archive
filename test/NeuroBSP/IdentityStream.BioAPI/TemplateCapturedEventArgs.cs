using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IdentityStream.BioAPI
{

    public class TemplateCapturedEventArgs : EventArgs
    {
        public TemplateCapturedEventArgs(Template Template, TemplateCapturedStatus Status)
            : base()
        {
            this.Template = Template;
            this.Status = Status;
        }

        public Template Template { get; private set; }
        public TemplateCapturedStatus Status { get; private set; }
    }

    public enum TemplateCapturedStatus
    {
        Success,
        QualityCheckFailed,
        MinutiaCountFailed,
        ExtractorException
    }
}
