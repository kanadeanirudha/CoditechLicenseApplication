
using System;
using System.ComponentModel.DataAnnotations;

namespace Coditech.ViewModel
{
    public class ApplicationLicenseDetailsViewModel : BaseViewModel
    {
        public int ApplicationLicenseId { get; set; }
        [Required]
        public string ClientName { get; set; }
        [Required]
        public string LicenseType { get; set; }
        [Required]
        public string DomainName { get; set; }
        [Required]
        public string APIKey { get; set; }
        [Required]
        public DateTime ValidFromDate { get; set; }
        public DateTime ValidUptoDate { get; set; }
        public bool IsActive { get; set; }
    }
}
