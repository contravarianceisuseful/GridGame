using System.Collections.Generic;
using GridGame;
using UnityEngine;

namespace BoardGame
{
    public abstract class Mover : MonoBehaviour
    {
        public Tile GetLocation(GameController controller)
        {
            return controller.GetComponentInChildren<PositionHandler>().GetTileOfMover(this);
        }

        public abstract List<Move> GetPossibleMoves(GameController controller);

        public virtual void OnMove(Tile destination, GameController controller)
        {
            
        }

        public virtual void AfterMove(GameController controller)
        {
            
        }

        public Tile MirrorAbout(Tile destination, GameController controller, Mirror mirrorDir)
        {
            var DiffCoord = controller.GetComponentInChildren<Grid>()
                .GetCoordFromOriginToTarget(destination, GetLocation(controller));
            int x = DiffCoord.x;
            int y = DiffCoord.y;

            switch (mirrorDir)
            {
                    case Mirror.X:
                    x = -x;
                    break;
                    case Mirror.Y:
                    y = -y;
                    break;
                    case Mirror.Flip:
                    x = -x;
                    y = -y;
                    break;
            }
            return controller.GetComponentInChildren<Grid>().GetTileAtCoord(x, y);
        }

        public virtual List<Move> CreateMovelist(Mover mover, List<Tile> moveTiles, GameController controller)
        {
            var moves = new List<Move>();
            foreach (var moveTile in moveTiles)
            {
                var move = new Move(GetLocation(controller), moveTile);
                moves.Add(move);
            }
            return moves;
        }
    }

    public abstract class MoverWithTaking : Mover
    {
        public override List<Move> CreateMovelist(Mover mover, List<Tile> moveTiles, GameController controller)
        {
            var moves = new List<Move>();
            foreach (var moveTile in moveTiles)
            {
                var move = new Move(GetLocation(controller), moveTile);
                moves.Add(move);
            }
            return moves;
        }
    }
}