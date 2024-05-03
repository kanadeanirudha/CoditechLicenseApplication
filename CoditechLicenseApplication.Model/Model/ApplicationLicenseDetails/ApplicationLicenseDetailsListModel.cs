using System.Collections.Generic;

namespace Coditech.Model
{
    public class ApplicationLicenseDetailsListModel : BaseListModel
    {
        public List<ApplicationLicenseDetailsModel> ApplicationLicenseDetailsList { get; set; }
        public ApplicationLicenseDetailsListModel()
        {
            ApplicationLicenseDetailsList = new List<ApplicationLicenseDetailsModel>();
        }
    }
}
