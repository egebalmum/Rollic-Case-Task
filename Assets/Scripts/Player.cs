using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Player : MonoBehaviour
{
    //Public Declared Values
    public Vector3 _basePosition;
    private PlayerAbility[] _abilities;
    private Rigidbody _rigidbody;
    public float leftBound;
    public float rightBound;
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        InitializeAbilities();
    }

    private void InitializeAbilities()
    {
        _abilities = GetComponents<PlayerAbility>();
        foreach (var ability in _abilities)
        {
            ability._player = this;
            ability._rigidbody = _rigidbody;
        }
    }

    private Vector3 AbilityDisplacement()
    {
        var totalVector = Vector3.zero;
        foreach (var ability in _abilities)
        {
            if (!ability.blockedGameStates.Contains(GameManager.Instance.GetGameState()))
            {
                 totalVector += ability.PhysicUpdate();
            }
        }

        return totalVector;
    }

    private void AbilityUpdates()
    {
        foreach (var ability in _abilities)
        {
            if (!ability.blockedGameStates.Contains(GameManager.Instance.GetGameState()))
            {
                ability.NormalUpdate();
            }
        }
    }

    void Start()
    {
        _basePosition = transform.position;
    }
    
    private void FixedUpdate()
    {
        _rigidbody.MovePosition(_rigidbody.position + AbilityDisplacement());
    }

    private void Update()
    {
        AbilityUpdates();
    }
}
