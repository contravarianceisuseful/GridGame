using UnityEngine;
using System.Collections;
using BoardGame;
using GridGame;

public class PositionHandlerStandard : PositionHandler
{
    public override void AddMover(Mover mover, Tile tile)
    {
        base.AddMover(mover, tile);
        Move(mover, tile);
    }

    public override void Move(Mover mover, Tile destination)
    {
        base.Move(mover, destination);
        mover.transform.SetParent(destination.transform);
        mover.transform.localPosition = Vector3.zero;
    }
}
