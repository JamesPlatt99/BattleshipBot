using System;

namespace BattleshipBot
{
    public class ShipPositionGenerator
    {
        public Position GetPosition(int shipSize)
        {
            Position position = new Position();
            Random random = new Random();
            while (true)
            {
                position.startCol = GetStartColOrRow();
                position.startRow = (char)(GetStartColOrRow() + 64);
                bool isHorizontal;
                if (isHorizontal = OrientationIsHorizontal())
                {
                    position.endCol = position.startCol + shipSize - 1;
                    position.endRow = position.startRow;
                }
                else
                {
                    position.endCol = position.startCol;
                    position.endRow = (char)((int)position.startRow + shipSize - 1);
                }
                if (CheckValidCoOrdinate(isHorizontal))
                {
                    break;
                }
            }
            
            return position;
        }

        private bool CheckValidCoOrdinate(bool isHorizontal)
        {
            
        }

        private bool OrientationIsHorizontal()
        {
            Random random = new Random();
            return (random.Next(1, 2) == 1);
        }

        private int GetStartColOrRow()
        {
            Random random = new Random();
            return random.Next(1,10);
        }
    }
}