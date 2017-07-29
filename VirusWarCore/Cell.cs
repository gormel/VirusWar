using System;

namespace VirusWarCore
{
    public class Cell
    {
        public Cell(Guid playerId, bool alive)
        {
            PlayerId = playerId;
            Alive = alive;
        }
        
        public Guid PlayerId { get; private set; }
        public bool Alive { get; private set; }
    }
}
