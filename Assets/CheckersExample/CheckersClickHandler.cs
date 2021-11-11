using UnityEngine;
using System.Collections;

public class CheckersClickHandler : ClickHandler
{ 
    public override void OnLeftClick(GameController controller, GameObject clicked)
    {
        if (clicked.GetComponent<Unit>() != null)
        {
            if (clicked.GetComponent<CheckersSelectable>().CanBeSelected(controller))
            {
                controller.GetComponent<SelectionHandler>().
                    SetSelected(clicked.GetComponent<CheckersSelectable>());
            }
        }
    }

    public override void OnRightClick(GameController controller, GameObject clicked)
    {
        throw new System.NotImplementedException();
    }
}


