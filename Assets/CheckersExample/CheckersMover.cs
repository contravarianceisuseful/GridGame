using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BoardGame;
using GridGame;
using UnityEngine.EventSystems;

public class Move
{
    public readonly Tile Origin;
    public readonly Tile Destination;
    

    public Move(Tile origin, Tile destination)
    {
        Origin = origin;
        Destination = destination;        
    }

    public static Move GenerateMoveFromTile(Tile origin, Tile destination)
    {
        return new Move(origin, destination);
    }

    public static List<Move> GenerateMovesFromTiles(Tile origin, List<Tile> destinations)
    {
        return destinations.Select(destination => new Move(origin, destination)).ToList();
    }
}

public class TakeMove : Move
{
    public TakeMove(Tile origin, Tile destination) : base(origin, destination)
    {
        this.ToTake = new List<Unit>();
    }

    public readonly List<Unit> ToTake;

    public bool IsTaking()
    {
        return UnitToTake() != null;
    }

    public Unit UnitToTake()
    {
        if (ToTake == null || ToTake.Count == 0)
        {
            return null;
        }
        if (ToTake.Count != 1)
        {
            throw new Exception("Accessing a null list or list with multiple elements");
        }
        return ToTake[0];

    }
}

namespace CheckersExample
{
    public class CheckersMover : Mover
    {
        public override List<Move> GetPossibleMoves(GameController controller)
        {
            var moves = new List<Move>();
            var origin = GetLocation(controller);
            var grid = controller.GetComponentInChildren<Grid>();
            var posHandler = controller.GetComponentInChildren<PositionHandler>();
            var unitHandler = controller.GetComponentInChildren<UnitOwnershipHandler>();
            Taker taker = GetComponent<Taker>();

            var dirs = new List<SquareDir> { SquareDir.UpLeft, SquareDir.UpRight };
            bool inverted = gameObject.HasComponent<Inverted>();

            foreach (var givenDir in dirs)
            {
                var dir = inverted ? givenDir.GetOpposite() : givenDir;
                var tempTile = grid.GetAdjecentTileInDir(GetLocation(controller), dir);
                if (posHandler.IsEmpty(tempTile))
                {
                    moves.Add(Move.GenerateMoveFromTile(origin,tempTile));
                }
                else
                {
                    Unit occupier = posHandler.GetMoverOnTile(tempTile).GetComponent<Unit>();
                    var diagBehind = grid.GetAdjecentTileInDir(tempTile, givenDir);
                    //TODO currently it takes like a chess pawn. Needs to jump over instead. So need to check the square behind as well. 
                    if (taker.CanTake(occupier.GetComponent<Takeable>(), controller) && posHandler.IsEmpty(diagBehind))
                    {
                        var tempTakeTile = grid.GetAdjecentTileInDir(tempTile, dir);
                        moves.Add(new TakeMove(origin, diagBehind) {ToTake = { occupier}});
                    }
                }
            }
            return moves;
        }
    }

}   

