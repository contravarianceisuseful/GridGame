using UnityEngine;
using System.Collections;

public abstract class UnitComponent : MonoBehaviour
{

    public Unit ThisUnit
    {
        get { return GetComponent<Unit>(); }
    }

}
