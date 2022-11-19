using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditorNormal : EditorCollectable
{
    public override float YPos()
    {
        return transform.localScale.y / 2;
    }
}
