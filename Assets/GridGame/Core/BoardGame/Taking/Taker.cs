using UnityEngine;
using System.Collections;

public class Taker : UnitComponent
{
    public virtual bool CanTake(Takeable toTake, GameController controller)
    {
        return controller.GetComponent<UnitOwnershipHandler>()
            .AreEnemies(ThisUnit, toTake.GetComponent<Unit>());
    }

    public virtual void OnTake(Takeable taken, GameController controller)
    {
        
    }
	
}
