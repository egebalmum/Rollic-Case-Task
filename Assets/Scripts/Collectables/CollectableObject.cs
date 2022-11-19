using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CollectableObject : MonoBehaviour
{
    public Vector3 initialPos;
    public Quaternion initialRot;
    public Rigidbody rigidbody;
    [SerializeField] public ParticleSystem popParticle;
    public virtual void Start()
    {
        initialPos = transform.position;
        initialRot = transform.rotation;
        rigidbody = GetComponent<Rigidbody>();
    }

    public virtual void Restart()
    {
        rigidbody.velocity = Vector3.zero;
        rigidbody.angularVelocity = Vector3.zero;
        transform.position = initialPos;
        transform.rotation = initialRot;
    }

    public virtual void PopParticleEffect()
    {
        Instantiate(popParticle, transform.position, Quaternion.identity);
    }

    public virtual float YPosOnStart()
    {
        return transform.localScale.y/2;
    }
}
