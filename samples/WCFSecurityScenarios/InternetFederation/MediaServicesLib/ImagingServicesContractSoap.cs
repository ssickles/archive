// © 2005-2006 Michele Leroux Bustamante. All rights reserved 
// Blogs: www.thatindigogirl.com, www.dasblonde.net
// IDesign: www.idesign.net
using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;

namespace MediaServicesLib
{
    [ServiceContract(Namespace = "http://www.thatindigogirl.com/samples/2005/12/ImagingServices")]
    public interface ImagingServicesContractSoap
    {

        [OperationContract]
        void UploadImage(ImageInfo imageInfo);

    }

}
