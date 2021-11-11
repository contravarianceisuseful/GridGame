using UnityEngine;

public class SelectionHandler : MonoBehaviour
{
    public Selectable Selected { get; internal set; }

    public void SetSelected(Selectable select)
    {
        var controller = GetComponentInParent<GameController>();

        Selected.OnDeselected(controller);
        select.OnSelectedPre(controller);
        Selected = select;
        select.OnSelectedPost(controller);

    }

}