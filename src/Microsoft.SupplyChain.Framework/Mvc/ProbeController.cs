using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Microsoft.SupplyChain.Framework.Mvc
{
    [ServiceRequestActionFilter]
    public class ProbeController : ApiController
    {
        public HttpResponseMessage Get()
        {
            return Request.CreateResponse(HttpStatusCode.OK, "Everything is fine");
        }
    }
}
