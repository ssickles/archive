// © 2005-2006 Michele Leroux Bustamante. All rights reserved 
// Blogs: www.thatindigogirl.com, www.dasblonde.net
// IDesign: www.idesign.net
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Threading;
using CustomAuthorization;
using System.IdentityModel.Claims;
using System.Security;

namespace MediaServicesLib
{
    public class MediaServices: ImagingServicesContractSoap
    {
        
        #region ImagingServicesContractSoap Members

        public void UploadImage(ImageInfo imageInfo)
        {
            // does this custom principal have the right claims?
            bool isAuthorized = false;

            IClaimSetPrincipal principal = Thread.CurrentPrincipal as IClaimSetPrincipal;
            IEnumerable<Claim> claims = principal.Claims.FindClaims(CustomClaimTypes.Read, Rights.PossessProperty);
            foreach (Claim c in claims)
            {
                string resource = c.Resource as string;
                if (resource == "Application")
                    isAuthorized = true;

            }
            
            if (!isAuthorized)
                throw new SecurityException
                ("Unauthorized access. Email claim not found.");

            // TODO: save uploaded image...

            return;
        }

        #endregion

        
    }
}
