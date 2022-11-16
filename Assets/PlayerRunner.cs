using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRunner : CharacterAbility
{
    [SerializeField] public float speed = 7;

    public override Vector3 PhysicUpdate()
    {
        if (GameManager.Instance.GetGameState() == GameManager.GameState.running)
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
        if (Mathf.Abs(displacement.z - _player._basePosition.z + _rigidbody.position.z) >= 30)
        {
            var overShoot = Mathf.Abs(displacement.z - _player._basePosition.z + _rigidbody.position.z)-30;
            displacement -= Vector3.forward*overShoot;
            GameManager.Instance.SetGameState(GameManager.GameState.control);
        }
        return displacement;
    }
}
