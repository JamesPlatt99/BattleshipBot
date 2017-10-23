using System;
using System.Collections.Generic;
using System.Linq;
using Battleships.Player.Interface;

namespace Battleships.ExamplePlayer
{
    public class ShipPositionGenerator
    {
        static Random random = new Random();
        public ShipPositionPlacement GetPosition(int shipSize)
        {
            ShipPositionPlacement position = new ShipPositionPlacement();
            
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
                if (CheckValidPlacement(isHorizontal, position))
                {
                    break;
                }
            }
            return position;
        }

        private bool CheckValidPlacement(bool isHorizontal, ShipPositionPlacement position)
        {
            List<Position> newPositions = new List<Position>();
            if (isHorizontal)
            {
                for (int i = position.startCol; i <= position.endCol; i++)
                {
                    if (i > 10)
                    {
                        return false;
                    }
                    GridSquare gridSquare = new GridSquare(position.startRow, (char)i);
                    if (!CheckCoOrdinateIsFree(new Position((char) position.startRow, i)))
                    {
                        return false;
                    }
                    newPositions.Add(new Position((char)position.startRow, i));
                }
            }
            else
            {
                for (int i = (int)position.startRow; i <= (int)position.endRow; i++)
                {
                    if (i > 75)
                    {
                        return false;
                    }
                    if (!CheckCoOrdinateIsFree(new Position((char)i, position.startCol)))
                    {
                        return false;
                    }
                    newPositions.Add(new Position((char)i,position.startCol));
                }
            }
            ExamplePlayer.OccupiedPositions = ExamplePlayer.OccupiedPositions.Concat(newPositions);
            return true;
        }

        private bool CheckCoOrdinateIsFree(Position position)
        {
            foreach (Position occupiedPosition in ExamplePlayer.OccupiedPositions)
            {
                for (int row = -1; row <= 1; row++)
                {
                    for (int col = -1; col <= 1; col++)
                    {
                        if (occupiedPosition.Row == position.Row + row && (int)occupiedPosition.Col == (int)position.Col + col)
                        {
                            return false;
                        }
                    }
                }
                
            }
            return true;
        }

        private bool OrientationIsHorizontal()
        {
            return (random.Next(1, 2) == 1);
        }

        private int GetStartColOrRow()
        {
            return random.Next(1, 10);
        }
    }
}