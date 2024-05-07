using Coditech.BusinessLogicLayer;
using Coditech.Model;

using System.Web.Http;

namespace Coditech.Controllers
{
    public class ActiveLicenseApiController : ApiController
    {

        //GET api/activelicenseapi/IsApplicationLicenseActive?apiKeyWithDomainName=fdsfsd
        [HttpGet]
        public ActiveApplicationLicenseModel IsApplicationLicenseActive(string apiKeyWithDomainName)
        {
            ActiveApplicationLicenseModel model = new ApplicationLicenseDetailsBA().IsApplicationLicenseActive(apiKeyWithDomainName);
            return model;
        }
    }
}
