using Newtonsoft.Json;

namespace VirusWarCore
{
    public class PlayState
    {
        public PlayState(string playId, bool isPlaying)
        {
            PlayId = playId;
            IsPlaying = isPlaying;
        }

        [JsonProperty("playId")]
        public string PlayId { get; private set; }
        [JsonProperty("isPlaying")]
        public bool IsPlaying { get; private set; }
    }
}
