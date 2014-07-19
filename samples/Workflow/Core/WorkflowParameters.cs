using System.Collections.Generic;
using System.Runtime.Serialization;

namespace CdcSoftware.Workflow.Core
{
    [DataContract]
	public class WorkflowParameters
	{
        private string _test;

        [DataMember]
        public string Test
        {
            get { return _test; }
            set { _test = value; }
        }
	}
}
