using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    [SerializeField] public int ID;
    
    private LevelNode node1;
    
    private LevelNode node2;

    private LevelNode node3;
    public GameObject gameField;
    public LevelNode[] nodes;
    public bool isActive;
    void Awake()
    {
        InitializeLevel();
    }
    
    void InitializeLevel()
    {
        nodes = new LevelNode[3];
        foreach (Transform tr in transform)
        {
            switch (tr.tag)
            {
                case "Node1":
                    node1 = tr.GetComponent<LevelNode>();
                    node1.collectables = new List<GameObject>();
                    node1.ID = 1;
                    nodes[0] = node1;
                    break;
                case "Node2":
                    node2 = tr.GetComponent<LevelNode>();
                    node1.collectables = new List<GameObject>();
                    node2.ID = 2;
                    nodes[1] = node2;
                    break;
                case "Node3":
                    node3 = tr.GetComponent<LevelNode>();
                    node1.collectables = new List<GameObject>();
                    node3.ID = 3;
                    nodes[2] = node3;
                    break;
            }
        }
    }

    public void RestartLevel()
    {
        foreach (var node in nodes)
        {
            node.objectInPoolCount = 0;
            node.RestartGateAnimation();
            node.DisableNode();
            node.StopAllCoroutines();
            foreach (var collectable in node.collectables)
            {
                collectable.SetActive(true);
                collectable.GetComponent<CollectableObject>().Restart();
            }
            node.UpdateScore();
        }
    }

    public void Activate()
    {
        gameField.SetActive(true);
        for (int i = 0; i < 3; i++)
        {
            foreach (var collectable in nodes[i].collectables)
            {
                collectable.SetActive(true);
            }
        }

        isActive = true;
    }

    public void Deactivate()
    {
        gameField.SetActive(false);
        for (int i = 0; i < 3; i++)
        {
            foreach (var collectable in nodes[i].collectables)
            {
                collectable.SetActive(false);
            }
        }

        isActive = false;
    }
    
}
