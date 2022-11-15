using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    private Vector3 startingPoint;

    private Rigidbody _rigidbody;
    
    // Start is called before the first frame update
    void Start()
    {
        startingPoint = transform.position;
        _rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _rigidbody.velocity = Vector3.zero;
            transform.position = startingPoint;
        }
    }
}
