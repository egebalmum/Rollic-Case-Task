using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //Public Declared Values
    [SerializeField] public float horizontalSpeed = 5;
    [SerializeField] public float verticalSpeed = 7;
    
    //Private Declared Values
    private Vector3 _startingPosition;

    //Cached Values
    private Rigidbody _rigidbody;
    
    void Start()
    {
        _startingPosition = transform.position;
        _rigidbody = GetComponent<Rigidbody>();
    }

    
    void FixedUpdate()
    {
        Move();
    }

    void Move()
    {
        var horizontalVector = Input.GetAxisRaw("Horizontal");
        var totalVector = new Vector3(horizontalVector*horizontalSpeed, 0, verticalSpeed);
        _rigidbody.MovePosition(_rigidbody.position + (totalVector * (Time.fixedDeltaTime)));
        if (Mathf.Abs(_rigidbody.position.z - _startingPosition.z) >= 30 && verticalSpeed!= 0)
        {
            _rigidbody.position = new Vector3(_rigidbody.position.x, _rigidbody.position.y, _startingPosition.z+30);
            verticalSpeed = 0;
        }
    }
}
