using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using VirusWarCore;

namespace VirusWar.Controllers
{
    public class GameController : ApiController
    {
        private static PlayFactory mPlayFactory = new PlayFactory();

        [HttpPost]
        [Route("~/api/session")]
        public string CreateSession()
        {
            return mPlayFactory.CreatePlay().Id.ToString();
        }

        [HttpPost]
        [Route("~/api/join/{session}")]
        public string JoinSession(string session)
        {
            Guid sessionId;
            if (!Guid.TryParse(session, out sessionId))
                return "";
            return mPlayFactory.GetPlay(sessionId).AddPlayer().ToString();
        }
    }
}
