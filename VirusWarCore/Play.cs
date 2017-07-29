using System;
using System.Collections.Generic;
using System.Linq;

namespace VirusWarCore
{
    public class Play
    {
        private struct Coord
        {
            public int X;
            public int Y;

            public Coord(int x, int y)
            {
                X = x;
                Y = y;
            }
        }
        private Player[] mPlayers = new Player[2];
        private int mLastAddedPlayer = -1;
        private int mCurrentPlayerIndex = 0;
        private int NextPlayerIndex => (mCurrentPlayerIndex + 1) % 2;
        private int mTurn = 0;
        private int mCurrentAction = 0;
        private bool mLastSkip;

        public Cell[][] Field { get; } = Enumerable.Range(0, 10).Select(i => new Cell[10]).ToArray();
        public int FieldVersion { get; private set; }
        public Guid Id { get; } = Guid.NewGuid();
        public Player CurrentPlayer => mPlayers[mCurrentPlayerIndex];
        public Player Winner { get; private set; } = null;
        
        public Player AddPlayer()
        {
            if (mLastAddedPlayer > 1)
                return null;
            var result = new Player(Guid.NewGuid(), mLastAddedPlayer + 1);
            mPlayers[++mLastAddedPlayer] = result;
            return result;
        }

        public bool Avaliable(int x, int y)
        {
            if (mLastAddedPlayer < 1)
                return false;
            if (Winner != null)
                return false;
            if (mTurn < 2)
                return x == mCurrentPlayerIndex * 10 && y == NextPlayerIndex * 10;
            return AvaliableInner(x, y, new List<Coord>());
        }

        private bool AvaliableInner(int x, int y, List<Coord> used)
        {
            if (x < 0 || x > 9 || y < 0 || y > 9)
                return false;
            var fieldValue = Field[x][y];
            if (!fieldValue.Alive || fieldValue.PlayerId == mPlayers[mCurrentPlayerIndex].Id)
                return false;
            for (int iX = x - 1; iX < x + 2; iX++)
            {
                for (int iY = y - 1; iY < y + 2; iY++)
                {
                    if (iX == x && iY == y || used.Any(c => c.X == iX && c.Y == iY))
                        continue;
                    fieldValue = Field[iX][iY];
                    if (fieldValue.Alive && fieldValue.PlayerId == mPlayers[mCurrentPlayerIndex].Id)
                        return true;
                    if (!fieldValue.Alive && fieldValue.PlayerId == mPlayers[NextPlayerIndex].Id)
                        if (AvaliableInner(iX, iY, used.Concat(new[] {new Coord(x, y)}).ToList()))
                            return true;
                }
            }
            return false;
        }

        public bool Action(int x, int y)
        {
            if (!Avaliable(x, y))
                return false;
            if (Field[x][y] == null)
                Field[x][y] = new Cell(mPlayers[mCurrentPlayerIndex].Id, true);
            else if (Field[x][y].Alive && Field[x][y].PlayerId == mPlayers[NextPlayerIndex].Id)
                Field[x][y] = new Cell(mPlayers[NextPlayerIndex].Id, false);
            else
                return false;

            FieldVersion++;
            mCurrentAction = (mCurrentAction + 1) % 3;
            mLastSkip = false;
            if (mCurrentAction == 0)
            {
                mCurrentPlayerIndex = NextPlayerIndex;
                CheckWinner();
                mTurn++;
            }
            return true;
        }

        public bool Skip()
        {
            if (mLastAddedPlayer < 1)
                return false;
            if (mLastSkip)
            {
                Winner = new Player(Guid.NewGuid(), -1);
                return true;
            }
            if (mCurrentAction != 0)
                return false;
            mCurrentPlayerIndex = NextPlayerIndex;
            CheckWinner();
            return true;
        }

        private void CheckWinner()
        {
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    if (Field[i][j].Alive && Field[i][j].PlayerId == mPlayers[mCurrentPlayerIndex].Id)
                        return;
                }
            }
            Winner = mPlayers[NextPlayerIndex];
        }

        public bool Concede()
        {
            if (mLastAddedPlayer < 1)
                return false;

            Winner = mPlayers[NextPlayerIndex];
            return true;
        }
    }
}
