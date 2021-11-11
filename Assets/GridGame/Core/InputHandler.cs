using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GridGame;

public class InputHandler : MonoBehaviour {


    public enum InputState
    {
        Default
    }

    public GameObject Selected;

    public Grid Grid;

    protected InputState inputState;
    #region click detection

    void Update()
    {
        HandleLeftClick();
        HandleRightClick();
    }

    private GameObject DetectClickedObject(int buttonID)
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

    protected bool DetectClick(int buttonID)
    {
        return Input.GetMouseButtonDown(buttonID);
    }

    #endregion

    #region click handling
    protected virtual void HandleLeftClick()
    {
        var clicked = DetectClickedObject(0);
        if (clicked != null)
        {
            Debug.Log("Object clicked: " + clicked.name);
            OnLeftClick(clicked);
        }
    }

    protected virtual void OnLeftClick(GameObject clicked)
    {
        Selected = clicked;
    }

    protected virtual void HandleRightClick()
    {
        if (DetectClick(1))
        {
            var clicked = DetectClickedObject(1);
            if (clicked != null)
            {
                Debug.Log("Object clicked: " + clicked.name);
                OnRightClick(clicked);
            }
        }
        
    }

    protected virtual void OnRightClick(GameObject clicked)
    {
        foreach (var tile in Grid.GetAllTiles())
        {
            tile.GetComponent<SpriteRenderer>().color = Color.white;
        }
        var selectedTile = Selected.GetComponent<Tile>();
        if (selectedTile != null)
        {
            var target = clicked.GetComponent<Tile>();
            foreach (var t in Grid.FindPathTo(selectedTile, target, tile => 1))
            {
                t.GetComponent<SpriteRenderer>().color = Color.black;
            }
        }
    }
#endregion
}