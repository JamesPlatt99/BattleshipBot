using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace Battleships.ExamplePlayer
{
    public class ShotGenerator
    {
        public static bool FoundShip = false;
        public static char Direction = 'u';
        private static int _radius = 1;
        public static Position LastHit;
        private static List<Position> _positionsShot;
        public static char[,] Board = new char[10, 10];
        public static bool LastShotHit = false;
        public Position GetPosition()
        {
            return FoundShip ? DestroyShip() : SearchForShip();
        }

        private Position DestroyShip()
        {
            return Direction == 'u' ? CheckAdjacentPositions() : CheckAlongDirection();
        }

        private Position CheckAlongDirection()
        {
            Contract.Ensures(Contract.Result<Position>() != null);
            Position curPos = LastHit;
            int i = 1;
            bool isReversing = false;
            while (true)
            {
                switch (Direction)
                {
                    case 'v':
                        curPos.Row = (char) ((int) curPos.Row + i);
                        break;
                    case 'h':
                        curPos.Col += i;
                        break;
                    default:
                        FoundShip = false;
                        Direction = 'u';
                        return SearchForShip();
                }
                
                if ((OutSideOfBoardBounds(curPos) | !LastShotHit | !CheckNewPosition(curPos) && !CheckHit(curPos)) && !isReversing)
                {
                    i = i * -1;
                    isReversing = true;
                    if (i == 1)
                    {
                        FoundShip = false;
                        Direction = 'u';
                        return SearchForShip();
                    }
                }

                if (CheckNewPosition(curPos))
                {
                    return curPos;
                }
            }
        }

        private bool OutSideOfBoardBounds(Position curPos)
        {
            return curPos.Col > 10 | curPos.Col < 1 | curPos.Row > 'J' | curPos.Row < 'A';
        }

        private bool CheckHit(Position position)
        {
            if (position.Col > 10 | position.Col < 1 | position.Row > 'J' | position.Row < 'A')
            {
                return false;
            }
            return (Board[(int) position.Row - 65, position.Col-1] == 'h');
        }

        private Position CheckAdjacentPositions()
        {
            Position pos = LastHit;
            while (true)
            {
                pos = new Position((char) ((int) LastHit.Row + 1), LastHit.Col);
                if (CheckNewPosition(pos))
                {
                    break;
                }
                pos = new Position((char) ((int) LastHit.Row - 1), LastHit.Col);
                if (CheckNewPosition(pos))
                {
                    break;
                }
                pos = new Position((char) ((int) LastHit.Row), LastHit.Col + 1);
                if (CheckNewPosition(pos))
                {
                    break;
                }
                pos = new Position((char) ((int) LastHit.Row), LastHit.Col - 1);
                if (CheckNewPosition(pos))
                {
                    break;
                }
                LastHit = null;
                Direction = 'u';
                FoundShip = false;
                return SearchForShip();
            }
            _positionsShot.Add(pos);
            return pos;
        }

        private Position SearchForShip()
        {
            for (int row = 6 - _radius; row <= 5 + _radius; row++)
            {
                for (int col = 6 - _radius; col <= 5 + _radius; col++)
                {
                    Position newPosition = new Position((char) (row + 64), col);
                    if (row % 2 != col % 2 && CheckNewPosition(newPosition))
                    {
                        _positionsShot.Add(newPosition);
                        return newPosition;
                    }
                }
            }
            if (_radius < 5)
            {
                _radius++;
            }
            return SearchForShip();
        }

        private bool CheckNewPosition(Position position)
        {
            if (position.Col > 10 | position.Col < 1 | position.Row > 'J' | position.Row < 'A')
            {
                return false;
            }
            if (_positionsShot == null)
            {
                return true;
            }
            foreach (Position curPosition in _positionsShot)
            {
                
                if (curPosition.Row == position.Row && (int)curPosition.Col == (int)position.Col)
                {
                    return false;
                }

            }
            return true;
        }

        public static char GetDirection(Position position)
        {
            if ((int)position.Row - 64 <10 && Board[(int) position.Row - 64, position.Col-1] == 'h')
            {
                return 'v';
            }
            if ((int) position.Row - 66 >= 0 && Board[(int) position.Row - 66, position.Col - 1] == 'h')
            {
                return 'v';
            }
            if (position.Col < 10 && Board[(int) position.Row - 65, position.Col] == 'h') 
            {
                return 'h';
            }
            if (position.Col - 2 > 0 && Board[(int) position.Row - 65, position.Col - 2] == 'h')
            {
               return 'h'; 
            }
                
            return 'u';
        }

        public static void resetProps()
        {
            ShotGenerator._positionsShot = new List<Position>();
            Board = new char[10, 10];
            FoundShip = false;
            _radius = 1;
            Direction = 'u';
        }
    }
}