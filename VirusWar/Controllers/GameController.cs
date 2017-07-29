using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace VirusWar.Controllers
{
    public class GameController : ApiController
    {
        [HttpPost]
        [Route("~/api/session")]
        public string CreateSession()
        {
            return Guid.NewGuid().ToString();
        }
    }
}
