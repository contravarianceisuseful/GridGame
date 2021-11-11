using UnityEngine;

public class Selectable : MonoBehaviour
{
    public virtual bool CanBeSelected(GameController controller)
    {
        return true;
    }

    public virtual void OnSelectedPre(GameController controller)
    {
        
    }

    public virtual void OnSelectedPost(GameController controller)
    {

    }

    public virtual void OnDeselected(GameController controller)
    {
        
    }
}