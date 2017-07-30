using System;
using System.Web.Http;
using VirusWarCore;

namespace VirusWar.Controllers
{
    public class GameController : ApiController
    {
        private static readonly PlayFactory PlayFactory = new PlayFactory();

        [HttpPost]
        [Route("~/api/session")]
        public string CreateSession()
        {
            return PlayFactory.CreatePlay().Id.ToString();
        }

        [HttpPost]
        [Route("~/api/join/{session}")]
        public Player JoinSession(string session)
        {
            Guid sessionId;
            if (!Guid.TryParse(session, out sessionId))
                return null;
            return PlayFactory.GetPlay(sessionId).AddPlayer();
        }

        [HttpPost]
        [Route("~/api/action/{session}/{player}/{x}/{y}")]
        public bool DoAction(string session, string player, string x, string y)
        {
            int iX;
            if (!int.TryParse(x, out iX))
                return false;

            int iY;
            if (!int.TryParse(y, out iY))
                return false;

            Guid sess;
            if (!Guid.TryParse(session, out sess))
                return false;

            var play = PlayFactory.GetPlay(sess);
            if (play == null)
                return false;

            if (play.CurrentPlayer.Id.ToString() != player)
                return false;

            return play.Action(iX, iY);
        }

        [HttpGet]
        [Route("~/api/field/{sessionId}")]
        public Cell[][] GetField(string sessionId)
        {
            Guid session;
            if (!Guid.TryParse(sessionId, out session))
                return null;

            var play = PlayFactory.GetPlay(session);
            if (play == null)
                return null;

            return play.Field;
        }

        [HttpGet]
        [Route("~/api/field_version/{sessionId}")]
        public int GetFieldVersion(string sessionId)
        {
            Guid session;
            if (!Guid.TryParse(sessionId, out session))
                return -1;

            var play = PlayFactory.GetPlay(session);
            if (play == null)
                return -1;

            return play.FieldVersion;
        }

        [HttpPost]
        [Route("~/api/skip/{sessionId}/{playerId}")]
        public bool Skip(string sessionId, string playerId)
        {
            Guid sess;
            if (!Guid.TryParse(sessionId, out sess))
                return false;

            var play = PlayFactory.GetPlay(sess);
            if (play == null)
                return false;

            if (play.CurrentPlayer.ToString() != playerId)
                return false;

            return play.Skip();
        }

        [HttpPost]
        [Route("~/api/skip/{sessionId}/{playerId}")]
        public bool Concede(string sessionId, string playerId)
        {
            Guid sess;
            if (!Guid.TryParse(sessionId, out sess))
                return false;

            var play = PlayFactory.GetPlay(sess);
            if (play == null)
                return false;

            if (play.CurrentPlayer.ToString() != playerId)
                return false;

            return play.Concede();
        }

        [HttpGet]
        [Route("~/api/current_player/{sessionId}")]
        public string CurrentPlayer(string sessionId)
        {
            Guid sess;
            if (!Guid.TryParse(sessionId, out sess))
                return null;

            var play = PlayFactory.GetPlay(sess);
            if (play == null)
                return null;

            return play.CurrentPlayer.ToString();
        }

        [HttpGet]
        [Route("~/api/winner/{sessionId}")]
        public Player GetWinner(string sessionId)
        {
            Guid sess;
            if (!Guid.TryParse(sessionId, out sess))
                return null;

            var play = PlayFactory.GetPlay(sess);
            if (play == null)
                return null;

            return play.Winner;
        }
    }
}
