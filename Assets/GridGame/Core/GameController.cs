using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour
{

}

public abstract class ControllerComponent : MonoBehaviour
{
    protected GameController controller
    {
        get { return GetComponent<GameController>(); }
    }
}
