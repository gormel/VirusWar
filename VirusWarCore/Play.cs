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
        private int mCurrentPlayerIndex;
        private int NextPlayerIndex => (mCurrentPlayerIndex + 1) % 2;
        private int mTurn;
        private int mCurrentAction;
        private bool mLastSkip;

        public Cell[][] Field { get; private set; }
        public int FieldVersion { get; private set; }
        public Guid Id { get; } = Guid.NewGuid();
        public Player CurrentPlayer => mPlayers[mCurrentPlayerIndex];
        public Player Winner { get; private set; }
        public bool IsPlaying { get; private set; } = false;

        private void BeginBattle()
        {
            Field = new Cell[10][];
            for (int i = 0; i < Field.Length; i++)
            {
                Field[i] = new Cell[10];
                for (int j = 0; j < Field[i].Length; j++)
                {
                    Field[i][j] = new Cell(Guid.Empty, false, new Guid());
                }
            }
            Field[0][9] = new Cell(Guid.Empty, false, mPlayers[mCurrentPlayerIndex].Id);
            IsPlaying = true;
        }

        public Player AddPlayer()
        {
            if (mLastAddedPlayer > 1)
                return null;
            var result = new Player(Guid.NewGuid(), mLastAddedPlayer + 1);
            mPlayers[++mLastAddedPlayer] = result;
            if (mLastAddedPlayer == 1)
                BeginBattle();
            return result;
        }

        public bool Avaliable(int x, int y)
        {
            if (mLastAddedPlayer < 1)
                return false;
            if (Winner != null)
                return false;
            if (x < 0 || x > 9 || y < 0 || y > 9)
                return false;
            return Field[x][y].AvaliableFor == mPlayers[mCurrentPlayerIndex].Id;
        }

        private void UpdateAvalibility(int x, int y, List<Coord> used, bool subCall = false)
        {
            if (used.Any(c => c.X == x && c.Y == y))
                return;
            var xyId = Field[x][y].PlayerId;
            if (xyId != mPlayers[mCurrentPlayerIndex].Id)
                return;
            if (!Field[x][y].Alive && !subCall)
                return;
            for (int i = x - 1; i < x + 2; i++)
            {
                for (int j = y - 1; j < y + 2; j++)
                {
                    if (i < 0 || i > 9 || j < 0 || j > 9 || i == x && j == y)
                        continue;

                    if (Field[i][j].PlayerId == Guid.Empty || Field[i][j].Alive && Field[i][j].PlayerId != xyId)
                        Field[i][j] = new Cell(Field[i][j].PlayerId, Field[i][j].Alive, xyId);
                    else if (!Field[i][j].Alive && Field[i][j].PlayerId != xyId)
                        UpdateAvalibility(i, j, used.Concat(new [] { new Coord(x, y) }).ToList(), true);
                    else
                        Field[i][j] = new Cell(Field[i][j].PlayerId, Field[i][j].Alive, Guid.Empty);
                }
            }
        }

        public bool Action(int x, int y)
        {
            if (!Avaliable(x, y))
                return false;
            if (Field[x][y].PlayerId == Guid.Empty)
                Field[x][y] = new Cell(mPlayers[mCurrentPlayerIndex].Id, true, Guid.Empty);
            else if (Field[x][y].Alive && Field[x][y].PlayerId == mPlayers[NextPlayerIndex].Id)
                Field[x][y] = new Cell(mPlayers[NextPlayerIndex].Id, false, Guid.Empty);
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

            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    Field[i][j] = new Cell(Field[i][j].PlayerId, Field[i][j].Alive, Guid.Empty);
                }
            }
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    UpdateAvalibility(i, j, new List<Coord>());
                }
            }

            if (mTurn == 1 && mCurrentAction == 0)
            {
                Field[9][0] = new Cell(Guid.Empty, false, mPlayers[1].Id);
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
            if (mTurn < 2)
                return;
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
