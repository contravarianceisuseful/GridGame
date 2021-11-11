using UnityEngine;
using System.Collections;

public class Takeable : UnitComponent
{
    public virtual void OnTaken(Taker taker, GameController controller)
    {
        
    }

    public virtual bool CanBeTakenBy(Taker taker, GameController controller)
    {
        return controller.GetComponent<UnitOwnershipHandler>()
            .AreEnemies(ThisUnit, taker.GetComponent<Unit>());
    }
}
