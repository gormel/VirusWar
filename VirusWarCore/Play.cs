using System;
using System.Linq;

namespace VirusWarCore
{
    public class Play
    {
        private Guid[] mPlayers = new Guid[2];
        private int mLastAddedPlayer = -1;
        private int mCurrentPlayerIndex = 0;

        public int[][] Field { get; } = Enumerable.Range(0, 10).Select(i => new int[10]).ToArray();
        public int FieldVersion { get; private set; }
        public Guid Id { get; } = Guid.NewGuid();
        public Guid CurrentPlayer => mPlayers[mCurrentPlayerIndex];
        
        public Guid AddPlayer()
        {
            if (mLastAddedPlayer > 1)
                return Guid.Empty;
            var result = Guid.NewGuid();
            mPlayers[++mLastAddedPlayer] = result;
            return result;
        }
    }
}
