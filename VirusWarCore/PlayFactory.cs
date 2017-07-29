using System;
using System.Collections.Generic;

namespace VirusWarCore
{
    public class PlayFactory
    {
        private Dictionary<Guid, Play> mPlays = new Dictionary<Guid, Play>(); 

        public Play CreatePlay()
        {
            //todo: remove finished plays
            var result = new Play();
            mPlays.Add(result.Id, result);
            return result;
        }

        public Play GetPlay(Guid playId)
        {
            Play result;
            if (!mPlays.TryGetValue(playId, out result))
                return null;
            return result;
        }
    }
}
