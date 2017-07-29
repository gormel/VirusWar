using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirusWarCore
{
    public class Player
    {
        public Player(Guid id, int order)
        {
            Id = id;
            Order = order;
        }

        public Guid Id { get; private set; }
        public int Order { get; private set; }
    }
}
