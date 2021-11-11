using UnityEngine;
using System.Collections;
using Multiplayer;

public class CheckersSelectable : Selectable
{
    public static Color HighlightColor = Color.magenta;
    protected Color oldColor;

    public override bool CanBeSelected(GameController controller)
    {
        var piece = GetComponent<Unit>();
        return controller.GetComponentInChildren<UnitOwnershipHandler>().GetOwner(piece) ==
               controller.GetComponentInChildren<TurnHandler>().CurrentTurnPlayer;
    }

    public override void OnSelectedPost(GameController controller)
    {
        var sr = GetComponent<SpriteRenderer>();
        oldColor = sr.color;
        sr.color = HighlightColor;
    }

    public override void OnDeselected(GameController controller)
    {
        var sr = GetComponent<SpriteRenderer>();
        sr.color = oldColor;
    }
}
