using UnityEngine;
using System.Collections;

public class ClickDetector : MonoBehaviour
{
    #region click detection

    public GameObject DetectClickedObject(int buttonID)
    {
        if (DetectClick(buttonID))
        {
            Ray2D ray = new Ray2D(Camera.main.ScreenToWorldPoint(new Vector2(Input.mousePosition.x, Input.mousePosition.y)), Vector2.zero);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);
            if (hit.collider != null)
            {
                return hit.transform.gameObject;
            }
        }
        return null;
    }

    public bool DetectClick(int buttonID)
    {
        return Input.GetMouseButtonDown(buttonID);
    }

    #endregion
}