
using System;
using System.ComponentModel.DataAnnotations;

namespace Coditech.ViewModel
{
    public class ApplicationLicenseDetailsViewModel : BaseViewModel
    {
        public int ApplicationLicenseId { get; set; }

        [Required(ErrorMessage= "Client Name is required")]
        public string ClientName { get; set; }

        [Required(ErrorMessage="License Type is required")]
        public string LicenseType { get; set; }
        [Required(ErrorMessage = "Domain Name is required")]
        public string DomainName { get; set; }
        public string APIKey { get; set; }

        [Required(ErrorMessage = "Please enter a valid date.")]
        [DataType(DataType.Date)]
        public DateTime ValidFromDate { get; set; }

        [Required(ErrorMessage = "Please enter a valid date.")]
        [DataType(DataType.Date)]
        public DateTime ValidUptoDate { get; set; }
        public bool IsActive { get; set; }
    }
}
