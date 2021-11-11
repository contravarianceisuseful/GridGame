using UnityEngine;
using System.Collections;
using BoardGame;
using GridGame;

public class MoveWithTakingHandler : PositionHandler
{

    public bool ContainsEnemy(Unit unit, Tile tile)
    {
        if (IsEmpty(tile))
        {
            return false;
        }
        var otherUnit = GetMoverOnTile(tile).GetComponent<Unit>();
        return GetComponent<UnitOwnershipHandler>().AreEnemies(unit, otherUnit);
    }

    public override void Move(Mover mover, Tile destination)
    {
        var occupier = GetMoverOnTile(destination).GetComponent<Takeable>();
        var th = GetComponent<TakeHandler>();
        if (th.ValidTake(mover.GetComponent<Taker>(), occupier))
        {
            th.Take(mover.GetComponent<Taker>(), occupier);
        }
        base.Move(mover, destination);
    }
}
