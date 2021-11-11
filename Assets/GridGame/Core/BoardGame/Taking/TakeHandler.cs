using UnityEngine;
using System.Collections;
using BoardGame;

public class TakeHandler : ControllerComponent
{

    public virtual void Take(Taker taker, Takeable takeable)
    {
        taker.OnTake(takeable, controller);
        takeable.OnTaken(taker, controller);
        RemoveTakenUnit(takeable);
    }

    public virtual bool ValidTake(Taker taker, Takeable toTake)
    {
        return taker.CanTake(toTake, controller) && toTake.CanBeTakenBy(taker, controller);
    }

    public virtual void RemoveTakenUnit(Takeable taken)
    {
        GetComponent<UnitOwnershipHandler>().RemoveUnit(taken.ThisUnit);
        GetComponent<PositionHandler>().RemoveMover(taken.GetComponent<Mover>());
    }
	
}
