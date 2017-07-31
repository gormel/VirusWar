using System;
using Newtonsoft.Json;

namespace VirusWarCore
{
    public class Cell
    {
        public Cell(Guid playerId, bool alive, Guid avaliableFor)
        {
            PlayerId = playerId;
            Alive = alive;
            AvaliableFor = avaliableFor;
        }
        
        [JsonProperty("playerId")]
        public Guid PlayerId { get; private set; }
        [JsonProperty("alive")]
        public bool Alive { get; private set; }
        [JsonProperty("avaliableFor")]
        public Guid AvaliableFor { get; private set; }
    }
}
