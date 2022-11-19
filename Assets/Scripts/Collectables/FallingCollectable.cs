using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;
using Random = UnityEngine.Random;

public class FallingCollectable : CollectableObject
{
    [SerializeField] private GameObject childCollectable;
    private List<GameObject> spawnedChilds;
    [SerializeField] private int childAmount;
    private Transform playerTransform;
    [SerializeField] private float activationDistance;
    [SerializeField] private bool isActivated;
    private MeshRenderer _renderer;
    private Collider _collider;
    public override void Start()
    {
        base.Start();
        rigidbody.isKinematic = true;
        GetPlayerTransform();
        _renderer = GetComponent<MeshRenderer>();
        _collider = GetComponent<Collider>();
        spawnedChilds = new List<GameObject>();
    }
    
    private void GetPlayerTransform()
    {
        if (GameManager.Instance._player != null)
        {
            playerTransform = GameManager.Instance._player.transform;
        }
        else
        {
            playerTransform = FindObjectOfType<Player>().transform;
        }
    }
    public void Update()
    {
        Drop();
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.transform.CompareTag("Floor"))
        {
            ChangeForm();
        }
    }

    private void ChangeForm()
    {
        _collider.enabled = false;
        _renderer.enabled = false;
        rigidbody.isKinematic = true;
        for (int i = 0; i < childAmount; i++)
        {
            var offset = new Vector3(Random.Range(0f, 0.1f), Random.Range(0f, 0.1f), Random.Range(0f, 0.1f));
            var child = Instantiate(childCollectable, transform.position + offset, Quaternion.identity);
            child.transform.localScale = transform.localScale/3f;
            spawnedChilds.Add(child);
        }
        //PopParticleEffect();
    }
    public override void Restart()
    {
        ResetChilds();
        base.Restart();
        rigidbody.isKinematic = true;
        rigidbody.useGravity = false;
        isActivated = false;
        _collider.enabled = true;
        _renderer.enabled = true;
    }

    private void Drop()
    {
        if (GameManager.Instance.GetGameState() == GameManager.GameState.Running)
        {
            var playerPos2D = playerTransform.position;
            playerPos2D.y = 0;
            var collectablePos2D = transform.position;
            collectablePos2D.y = 0;
            if (Vector3.Distance(collectablePos2D, playerPos2D) <= activationDistance && !isActivated)
            {
                rigidbody.isKinematic = false;
                isActivated = true;
                rigidbody.useGravity = true;
            }
        }
    }
    private void ResetChilds()
    {
        for (int i = spawnedChilds.Count-1; i >= 0; i--)
        {
            Destroy(spawnedChilds[i].gameObject);
        }
        spawnedChilds.Clear();
    }

    public override float YPosOnStart()
    {
        return 5f;
    }
}
