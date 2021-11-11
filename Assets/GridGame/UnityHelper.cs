using UnityEngine;
using System.Collections;

public static class UnityHelper
{

    public static  bool HasComponent<T>(this GameObject gameObject) where T : Component
    {
        return gameObject.GetComponent<T>() != null;
    }

    public static bool HasComponent<T>(this Component component) where T : Component
    {
        return component.gameObject.GetComponent<T>() != null;
    }

}
