using System.Collections.Generic;

namespace Coditech.ViewModel
{
    public class ApplicationLicenseDetailsListViewModel : BaseViewModel
    {

        public List<ApplicationLicenseDetailsViewModel> ApplicationLicenseDetailsList { get; set; }

        public ApplicationLicenseDetailsListViewModel()
        {
            ApplicationLicenseDetailsList = new List<ApplicationLicenseDetailsViewModel>();
        }
    }
}
