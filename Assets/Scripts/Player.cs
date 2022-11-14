using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] public float speed = 5;
    
    private Rigidbody _rigidbody;
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    
    void Update()
    {
        Move();
    }

    void Move()
    {
        var horizontalVector = Input.GetAxisRaw("Horizontal");
        var verticalVector = Input.GetAxisRaw("Vertical");
        var totalVector = new Vector3(horizontalVector, 0, verticalVector);
        _rigidbody.position += totalVector * (speed * Time.deltaTime);
    }
}
