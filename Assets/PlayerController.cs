using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : CharacterAbility
{
    [SerializeField] public float speed = 5;

    public override Vector3 PhysicUpdate()
    {
        return Move();
    }

    Vector3 Move()
    {
        var horizontalVector = Input.GetAxisRaw("Horizontal");
        var moveVector = new Vector3(horizontalVector, 0, 0);
        return (moveVector * (speed*Time.fixedDeltaTime));
    }
}
