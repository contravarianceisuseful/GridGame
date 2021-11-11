using UnityEngine;
using System.Collections;

public class SelectablePiece : Selectable
{
    protected Color hlColor = Color.yellow;

    public override void OnSelectedPre(GameController controller)
    {
        base.OnSelectedPre(controller);
        var hl = GetComponent<Highlightable>();

        hl.Highlight(hlColor);
    }

    public override void OnDeselected(GameController controller)
    {
        base.OnDeselected(controller);
    }
}
