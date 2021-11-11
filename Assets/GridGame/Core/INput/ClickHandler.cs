using UnityEngine;

public abstract class ClickHandler : MonoBehaviour
{
    public abstract void OnLeftClick(GameController controller, GameObject clicked);

    public abstract void OnRightClick(GameController controller, GameObject clicked);
}