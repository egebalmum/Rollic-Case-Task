using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableObject : MonoBehaviour
{
    private Vector3 initialPos;
    private Quaternion initialRot;
    private Rigidbody rigidbody;
    void Start()
    {
        initialPos = transform.position;
        initialRot = transform.rotation;
        rigidbody = GetComponent<Rigidbody>();
    }

    public void Restart()
    {
        rigidbody.velocity = Vector3.zero;
        rigidbody.angularVelocity = Vector3.zero;
        transform.position = initialPos;
        transform.rotation = initialRot;
    }
   
}
