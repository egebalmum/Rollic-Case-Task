using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PlayerPush : PlayerAbility
{
    [SerializeField] private float pushForce;
    private List<CollectableObject> collectables;
    void Start()
    {

        collectables = new List<CollectableObject>();
        GameManager.ControlEnter += PushCollectables;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<CollectableObject>() != null)
        {
            collectables.Add(other.gameObject.GetComponent<CollectableObject>());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<CollectableObject>() != null)
        {
            collectables.Remove(other.gameObject.GetComponent<CollectableObject>());
        }
    }

    private void PushCollectables()
    {
        foreach (var collectable in collectables)
        {
            var direction = collectable.rigidbody.velocity;
            direction.y = 0;
            collectable.rigidbody.AddForce(direction*pushForce);
        }
        collectables.Clear();
    }
}
