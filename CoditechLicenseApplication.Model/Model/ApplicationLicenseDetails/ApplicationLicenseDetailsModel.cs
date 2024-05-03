namespace Coditech.Model
{
    public class ApplicationLicenseDetailsModel : BaseModel
    {
        public int ApplicationLicenseId { get; set; }
        public string ClientName { get; set; }
        public string LicenseType { get; set; }
        public string DomainName { get; set; }
        public string APIKey { get; set; }
        public System.DateTime ValidFromDate { get; set; }
        public System.DateTime ValidUptoDate { get; set; }
        public bool IsActive { get; set; }
    }
}
