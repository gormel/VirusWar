using System;
using System.Collections.Generic;
using System.Linq;

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

        public PlayState[] GetPlays()
        {
            return mPlays.Values.Select(p => new PlayState(p.Id.ToString(), p.IsPlaying)).ToArray();
        }
    }
}
