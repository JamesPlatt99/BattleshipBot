﻿using System.Runtime.Remoting.Metadata.W3cXsd2001;

namespace Battleships.ExamplePlayer
{
    using Battleships.Player.Interface;
    using System.Collections.Generic;

    public class ExamplePlayer : IBattleshipsBot
    {
        internal IGridSquare LastTarget;
        private readonly HashSet<IGridSquare> shipsHit = new HashSet<IGridSquare>();
        public static IEnumerable<Position> OccupiedPositions = new List<Position>();
        public string Name => "Bot Allu";

        public IEnumerable<IShipPosition> GetShipPositions()
        {
            OccupiedPositions = new List<Position>();
            ShipPositionGenerator shipPositionGenerator= new ShipPositionGenerator();
            ShotGenerator.resetProps();
            return new List<IShipPosition>
            {
                GetShipPosition(shipPositionGenerator.GetPosition(5)),
                GetShipPosition(shipPositionGenerator.GetPosition(4)),
                GetShipPosition(shipPositionGenerator.GetPosition(3)),
                GetShipPosition(shipPositionGenerator.GetPosition(3)),
                GetShipPosition(shipPositionGenerator.GetPosition(2))
            };
        }

        public IGridSquare SelectTarget()
        {
            ShotGenerator shotGenerator = new ShotGenerator();
            Position newPosition = shotGenerator.GetPosition();
            IGridSquare nextTarget = new GridSquare(newPosition.Row, newPosition.Col);
            LastTarget = nextTarget;
            return nextTarget;
        }

        public void HandleShotResult(IGridSquare square, bool wasHit)
        {
            ShotGenerator.LastShotHit = false;
            if (!wasHit) return;
            ShotGenerator.LastShotHit = true;
            ShotGenerator.FoundShip = true;
            ShotGenerator.LastHit = new Position(square.Row, square.Column);
            ShotGenerator.Board[(int) square.Row - 65, square.Column-1] = 'h';
            ShotGenerator.Direction = ShotGenerator.GetDirection(new Position(square.Row, square.Column));
            shipsHit.Add(square);
        }

        public void HandleOpponentsShot(IGridSquare square) {}

        private static ShipPosition GetShipPosition(ShipPositionPlacement position)
        {
            return new ShipPosition(new GridSquare(position.startRow, position.startCol), new GridSquare(position.endRow, position.endCol));
        }

        private IGridSquare GetNextTarget()
        {
            if (LastTarget == null)
            {
                return new GridSquare('A', 1);
            }

            var row = LastTarget.Row;
            var col = LastTarget.Column + 1;
            if (LastTarget.Column != 10)
            {
                return new GridSquare(row, col);
            }

            row = (char)(row + 1);
            if (row > 'J')
            {
                row = 'A';
            }
            col = 1;
            return new GridSquare(row, col);
        }
    }
}