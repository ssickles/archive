namespace SampleService
{
    using OpenSpan.TypeManagement;
    using System;
    using System.ComponentModel;

    public class SampleTextDisplayer : Component
    {
        [MemberVisibility(MemberVisibilityLevel.DefaultOn)]
        public string GetText()
        {
            ISampleHostService service = this.Site.GetService(typeof(ISampleHostService)) as ISampleHostService;
            if (service == null)
            {
                throw new NullReferenceException("Could not find SampleHostService.");
            }
            return service.GetSampleText();
        }
    }
}

