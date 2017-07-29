using System;
using Newtonsoft.Json;

namespace VirusWarCore
{
    public class Player
    {
        public Player(Guid id, int order)
        {
            Id = id;
            Order = order;
        }

        [JsonProperty("id")]
        public Guid Id { get; private set; }
        [JsonProperty("order")]
        public int Order { get; private set; }
    }
}
