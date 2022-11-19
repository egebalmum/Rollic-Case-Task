using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : PlayerAbility
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
        var displacement = moveVector * (speed * Time.fixedDeltaTime);
        var totalMoveX = displacement.x + _rigidbody.position.x;
        if (totalMoveX > _player.rightBound)
        {
            var overShoot = totalMoveX - _player.rightBound;
            displacement.x = totalMoveX - overShoot - _rigidbody.position.x;
        }
        else if (totalMoveX < _player.leftBound)
        {
            var overShoot = totalMoveX - _player.leftBound;
            displacement.x = totalMoveX - overShoot - _rigidbody.position.x;
        }
        return (displacement);
    }
    
}
