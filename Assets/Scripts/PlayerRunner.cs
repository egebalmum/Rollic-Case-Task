using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRunner : PlayerAbility
{
    [SerializeField] public float speed = 7;

    public override Vector3 PhysicUpdate()
    {
        if (GameManager.Instance.GetGameState() == GameManager.GameState.Running)
        {
            return Move();
        }
        else
        {
            return Vector3.zero;
        }
    }
    
    Vector3 Move()
    {
        var displacement = (Vector3.forward * (speed*Time.fixedDeltaTime));
        return displacement;
    }
}
