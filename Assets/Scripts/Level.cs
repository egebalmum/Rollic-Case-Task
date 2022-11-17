using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    [SerializeField] public int ID;
    
    private LevelNode node1;
    
    private LevelNode node2;

    private LevelNode node3;
    public LevelNode[] nodes;
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
                    node1.ID = 1;
                    nodes[0] = node1;
                    break;
                case "Node2":
                    node2 = tr.GetComponent<LevelNode>();
                    node2.ID = 2;
                    nodes[1] = node2;
                    break;
                case "Node3":
                    node3 = tr.GetComponent<LevelNode>();
                    node3.ID = 3;
                    nodes[2] = node3;
                    break;
            }
        }
    }
    
}
