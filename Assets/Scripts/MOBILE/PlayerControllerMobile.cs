using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerMobile : PlayerAbility
{
    [SerializeField] public float speed = 5;
    private Touch touch;
    public override Vector3 PhysicUpdate()
    {
        return Move();
    }

    Vector3 Move()
    {
        int rawInput= 0;
        if (Input.touchCount > 0)
        {
            touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Moved)
            {
                if (touch.deltaPosition.x > 0)
                {
                    rawInput = 1;
                }
                else
                {
                    rawInput = -1;
                }
                
            }
        }
        var moveVector = new Vector3(rawInput, 0, 0);
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