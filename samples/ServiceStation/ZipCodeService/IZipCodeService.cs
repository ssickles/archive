using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;
using Extensions;

namespace ZipCodeServiceLibrary
{
    [ServiceContract]
    public interface IZipCodeService
    {
        [ZipCodeCaching]
        [ZipCodeValidation]
        [OperationContract]
        string Lookup(string zipcode);
    }
}
