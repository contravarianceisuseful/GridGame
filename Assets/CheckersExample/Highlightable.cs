using UnityEngine;
using System.Collections;

public class Highlightable : MonoBehaviour
{
    public Color OriginalColor { get; internal set; }

    protected SpriteRenderer sr
    {
        get
        {
            return GetComponent<SpriteRenderer>();
        }
    }

    public virtual void Highlight(Color color)
    {
        OriginalColor = sr.color;
        sr.color = color;
    }

    public virtual void Dehighlight()
    {
        sr.color = OriginalColor;
    }
}
