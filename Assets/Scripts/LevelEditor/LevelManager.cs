using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    private Camera _camera;
    public static LevelManager Instance;
    [SerializeField] public List<GameObject> types;
    public GameObject holdingObject;
    public int[] thresholds;
    public int activeNode=1;
    public int activeID;
    private NodeData node1;
    private NodeData node2;
    private NodeData node3;
    private LevelData level;
    private bool allowedToPlace;
    private MeshRenderer holdingObjectRenderer;
    [SerializeField] private Material forbiddenMat;
    [SerializeField] private Material allowedMat;
    [SerializeField] private Material defMat;
    private EditorCollectable holdingScript;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    void Start()
    {
        thresholds = new int[3];
        _camera = FindObjectOfType<Camera>();
        node1 = new NodeData();
        node2 = new NodeData();
        node3 = new NodeData();
        level = new LevelData();
        level.nodeList.Add(node1);
        level.nodeList.Add(node2);
        level.nodeList.Add(node3);
    }

    void Update()
    {
        HoldObject();
        PlaceObject();
        CancelObject();
    }

    void HoldObject()
    {
        if (holdingObject != null)
        {
            var vec = Input.mousePosition;
            vec.z = 35;
            vec = Camera.main.ScreenToWorldPoint(vec);
            vec.y = holdingScript.YPos();
            vec.x = Mathf.Round(vec.x * 1f) / 1f;
            vec.z = Mathf.Round(vec.z * 1f) / 1f;
            if (vec.x >= -4.5f && vec.x <= 4.5f && vec.z <= 30+((activeNode-1)*40) && vec.z >= 0+(activeNode-1)*40)
            {
                holdingObject.transform.position = vec;
            }
            MousePosValidCheck(vec);
        }
    }

    void MousePosValidCheck(Vector3 vec)
    {
        if (vec.x >= -4.6f && vec.x <= 4.6f && vec.z <= 30+((activeNode-1)*40) && vec.z >= 0+(activeNode-1)*40 && !level.nodeList[activeNode-1].positions.Contains(holdingObject.transform.position))
        {
            allowedToPlace = true;
            holdingObjectRenderer.material = allowedMat;
        }
        else
        {
            allowedToPlace = false;
            holdingObjectRenderer.material = forbiddenMat;
        }
    }
    

    public void CreateObject(int ID, Vector3 scale)
    {
        if (holdingObject != null)
        {
            Destroy(holdingObject.gameObject);
            holdingObject = null;
        }
        activeID = ID;
        holdingObject = Instantiate(types[activeID], transform.position, Quaternion.identity);
        holdingScript = holdingObject.GetComponent<EditorCollectable>();
        holdingObjectRenderer = holdingObject.GetComponent<MeshRenderer>();
        holdingObject.transform.localScale = scale;
    }

    public void PlaceObject()
    {
        if (holdingObject != null)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0) && allowedToPlace)
            {
                var placedObject = Instantiate(holdingObject, holdingObject.transform.position, Quaternion.identity);
                placedObject.GetComponent<MeshRenderer>().material = defMat;
                level.nodeList[activeNode-1].positions.Add(placedObject.transform.position);
                level.nodeList[activeNode-1].scales.Add(placedObject.transform.localScale);
                level.nodeList[activeNode-1].objectTypes.Add(activeID);
            }
            
        }
    }

    public void CancelObject()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Destroy(holdingObject.gameObject);
            holdingObject = null;
        }
    }
    public void EditThreshold(int value)
    {
        thresholds[activeNode-1] = value;
        level.nodeList[activeNode - 1].threshold = value;
    }

    public void GoNextNode()
    {
        _camera.transform.position += Vector3.forward * 40;
        activeNode += 1;
    }
    
    //save related

    public class LevelData
    {
        public List<NodeData> nodeList;

        public LevelData()
        {
            nodeList = new List<NodeData>();
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
    }

    public void SaveLevel()
    {
        int levelNumber;
        DirectoryInfo dir = new DirectoryInfo(Application.dataPath+"/Levels/");
        if (dir.GetDirectories().Length == 0)
        {
            levelNumber = 1;
        }
        else
        {
            levelNumber = dir.GetDirectories().Length + 1;
        }
        Directory.CreateDirectory(Application.dataPath + "/Levels/Level"+levelNumber);
        for (int i = 0; i < 3; i++)
        {
            string json = JsonUtility.ToJson(level.nodeList[i], true);
            File.WriteAllText(Application.dataPath + "/Levels/Level"+ levelNumber + "/node"+(i+1) +".json" , json);
        }
    }
}
