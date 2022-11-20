using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Data 
{
    
    public class LevelData
    {
        public List<NodeData> nodeList;

        public LevelData()
        {
            nodeList = new List<NodeData>();
        }

        public void Copy(LevelData other)
        {
            nodeList.Clear();
            for (int i = 0; i < other.nodeList.Count; i++)
            {
                nodeList.Add(new NodeData());
                nodeList[i].Copy(other.nodeList[i]);
            }
            
        }
    }

    public class NodeData
    {
        public List<Vector3> positions;
        public List<Vector3> scales;
        public List<int> objectTypes;
        public int threshold;
        public NodeData()
        {
            positions = new List<Vector3>();
            scales = new List<Vector3>();
            objectTypes = new List<int>();
        }

        public void Copy(NodeData other)
        {
            positions.Clear();
            scales.Clear();
            objectTypes.Clear();
            threshold = 0;

            for (int i = 0; i < other.positions.Count; i++)
            {
                positions.Add(other.positions[i]);
                scales.Add(other.scales[i]);
                objectTypes.Add(other.objectTypes[i]);
            }
            threshold = other.threshold;
        }
    }
}
