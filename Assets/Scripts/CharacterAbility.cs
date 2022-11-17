using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharacterAbility : MonoBehaviour
{
    [HideInInspector] public Player _player;
    [HideInInspector] public Rigidbody _rigidbody;
    public GameManager.GameState[] blockedGameStates;
    public GameManager.GameState[] blockedPlayerStates;


    public virtual Vector3 PhysicUpdate()
    {
        return Vector3.zero;
    }

    public virtual void NormalUpdate()
    {
        
    }

    public virtual void Reseter()
    {
        
    }
}
