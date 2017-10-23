using System;
using System.Collections.Generic;
using System.Linq;
using Battleships.Player.Interface;

namespace Battleships.ConsoleApp
{
    public class ShipsPlacement
    {
        private const int TotalNumberOfShipCells = 17;
        private readonly HashSet<IGridSquare> cellsOfShipsHit = new HashSet<IGridSquare>();
        private readonly List<Ship> ships;

        public ShipsPlacement(IBattleshipsBot bot)
        {
            try
            {
                ShipPositions = bot.GetShipPositions();
                ships = ShipPositions.Select(ship => new Ship(ship)).ToList();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                ships = null;
            }
        }

        public IEnumerable<IShipPosition> ShipPositions { get; }

        public bool IsValid()
        {
            var shipsAvailableOfSize = new Dictionary<int, int> { { 2, 1 }, { 3, 2 }, { 4, 1 }, { 5, 1 } };
            var coordinatesAdjacentToShip = new bool[12, 12];

            if (ships == null)
            {
                return false;
            }

            foreach (var shipPosition in ships)
            {
                if (shipPosition.IsValid && shipsAvailableOfSize[shipPosition.ShipLength] != 0
                    && IsPositionValid(shipPosition, coordinatesAdjacentToShip))
                {
                    shipsAvailableOfSize[shipPosition.ShipLength] -= 1;
                    OccupyGridSquares(shipPosition, coordinatesAdjacentToShip);
                }
                else
                {
                    return false;
                }
            }
            return shipsAvailableOfSize.All(ship => ship.Value == 0);
        }

        public bool IsHit(IGridSquare target)
        {
            if (ships.Any(ship => ship.IsTargetInShip(target)))
            {
                cellsOfShipsHit.Add(target);
                return true;
            }
            return false;
        }

        public bool AllHit()
        {
            return cellsOfShipsHit.Count == TotalNumberOfShipCells;
        }

        private void OccupyGridSquares(Ship shipPosition, bool[,] coordinatesAdjacentToShip)
        {
            for (var i = shipPosition.StartingSquare.Column - 1; i <= shipPosition.EndingSquare.Column + 1; i++)
            {
                for (var j = GetIndexFromChar(shipPosition.StartingSquare.Row) - 1; j <= GetIndexFromChar(shipPosition.EndingSquare.Row) + 1; j++)
                {
                    coordinatesAdjacentToShip[i, j] = true;
                }
            }
        }

        private bool IsPositionValid(Ship shipPosition, bool[,] coordinatesAdjacentToShip)
        {
            for (var i = shipPosition.StartingSquare.Column; i <= shipPosition.EndingSquare.Column; i++)
            {
                for (var j = GetIndexFromChar(shipPosition.StartingSquare.Row); j <= GetIndexFromChar(shipPosition.EndingSquare.Row); j++)
                {
                    if (coordinatesAdjacentToShip[i, j])
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        private int GetIndexFromChar(char row)
        {
            return row - 'A' + 1;
        }
    }
}