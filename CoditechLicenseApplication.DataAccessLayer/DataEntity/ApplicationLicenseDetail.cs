//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Coditech.DataAccessLayer.DataEntity
{
    using System;

    public partial class ApplicationLicenseDetail : CoditechEntityBaseModel
    {
        public int ApplicationLicenseId { get; set; }
        public string ClientName { get; set; }
        public string LicenseType { get; set; }
        public string DomainName { get; set; }
        public string APIKey { get; set; }
        public System.DateTime ValidFromDate { get; set; }
        public System.DateTime ValidUptoDate { get; set; }
        public bool IsActive { get; set; }
        public Nullable<long> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<long> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
    }
}