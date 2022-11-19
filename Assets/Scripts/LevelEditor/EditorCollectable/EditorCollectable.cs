using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EditorCollectable : MonoBehaviour
{
    public virtual float YPos()
    {
        return transform.localScale.y / 2;
    }
}
