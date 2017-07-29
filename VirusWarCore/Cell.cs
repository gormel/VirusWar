using System;
using Newtonsoft.Json;

namespace VirusWarCore
{
    public class Cell
    {
        public Cell(Guid playerId, bool alive)
        {
            PlayerId = playerId;
            Alive = alive;
        }
        
        [JsonProperty("playerId")]
        public Guid PlayerId { get; private set; }
        [JsonProperty("alive")]
        public bool Alive { get; private set; }
    }
}
