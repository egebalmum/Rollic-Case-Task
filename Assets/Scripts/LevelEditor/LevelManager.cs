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
    private Data.NodeData node1;
    private Data.NodeData node2;
    private Data.NodeData node3;
    private Data.LevelData level;
    private bool allowedToPlace;
    private MeshRenderer holdingObjectRenderer;
    [SerializeField] private Material forbiddenMat;
    [SerializeField] private Material allowedMat;
    [SerializeField] private Material defMat;
    private EditorCollectable holdingScript;
    private List<List<GameObject>> placedObjects;
    public Dictionary<int, Data.LevelData> dataDict;
    public bool isBuildingModeOn;
    public Mode mode;
    private int levelNumber;
    private bool isSaved;
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
        placedObjects = new List<List<GameObject>>();
        placedObjects.Add(new List<GameObject>());
        placedObjects.Add(new List<GameObject>());
        placedObjects.Add(new List<GameObject>());
        InitiateLevelObjects();
        dataDict = new Dictionary<int, Data.LevelData>();
    }


    public enum Mode
    {
        editOld,
        createNew
    }
    void Update()
    {
        BuildingFunctions();
    }


    void BuildingFunctions()
    {
        if (isBuildingModeOn)
        {
            HoldObject();
            PlaceObject();
            CancelObject();
            DeleteObject();
        }
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

    private void PlaceObject()
    {
        if (holdingObject != null)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0) && allowedToPlace)
            {
                var placedObject = Instantiate(holdingObject, holdingObject.transform.position, Quaternion.identity);
                placedObject.GetComponent<MeshRenderer>().material = defMat;
                placedObjects[activeNode-1].Add(placedObject);
                level.nodeList[activeNode-1].positions.Add(placedObject.transform.position);
                level.nodeList[activeNode-1].scales.Add(placedObject.transform.localScale);
                level.nodeList[activeNode-1].objectTypes.Add(activeID);
            }
        }
    }

    private void DeleteObject()
    {
        if (holdingObject != null)
        {
            if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                if (level.nodeList[activeNode - 1].positions.Contains(holdingObject.transform.position))
                {
                    var index = level.nodeList[activeNode - 1].positions.IndexOf(holdingObject.transform.position);
                    Destroy(placedObjects[activeNode-1][index].gameObject);
                    placedObjects[activeNode-1].RemoveAt(index);
                    level.nodeList[activeNode-1].positions.RemoveAt(index);
                    level.nodeList[activeNode-1].scales.RemoveAt(index);
                    level.nodeList[activeNode-1].objectTypes.RemoveAt(index);
                }
            }
        }
    }

    public void CancelObject()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && holdingObject != null)
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

    public void GoBackNode()
    {
        _camera.transform.position -= Vector3.forward * 40;
        activeNode -= 1;
    }
    
    public void SaveLevel()
    {
        isSaved = true;
        
        if (mode ==  Mode.createNew)
        {
            Directory.CreateDirectory(Application.dataPath + "/Levels/Level"+levelNumber);
            EditorMainMenu.Instance.AddNewSelection();
            EditorMainMenu.Instance.levelDatas.Add(dataDict[levelNumber]);
        }
        else
        {
            EditorMainMenu.Instance.levelDatas[levelNumber - 1] = dataDict[levelNumber];
        }
        for (int i = 0; i < 3; i++)
        {
            string json = JsonUtility.ToJson(level.nodeList[i], true);
            File.WriteAllText(Application.dataPath + "/Levels/Level"+ levelNumber + "/node"+(i+1) +".json" , json);
        }
        
    }

    public void LoadLevel(Data.LevelData data, int selectionIndex)
    {
        levelNumber = selectionIndex+1;
        if (dataDict.ContainsKey(levelNumber))
        {
            level = dataDict[levelNumber];
        }
        else
        {
            dataDict.Add(levelNumber, new Data.LevelData());
            level = dataDict[levelNumber];
            level.Copy(data);
        }
        for (int i = 0; i < 3; i++)
        {
            var node = level.nodeList[i];
            for (int j = 0; j < level.nodeList[i].positions.Count; j++)
            {
                var placedObject = Instantiate(types[node.objectTypes[j]], node.positions[j], Quaternion.identity);
                placedObjects[i].Add(placedObject);
                placedObject.transform.localScale = node.scales[j];
            }
        }
        LoadThresholds();
    }


    public void LoadThresholds()
    {
        LevelEditorUIManager.Instance.OnThreshold(1,level.nodeList[0].threshold);
        LevelEditorUIManager.Instance.OnThreshold(2,level.nodeList[1].threshold);
        LevelEditorUIManager.Instance.OnThreshold(3,level.nodeList[2].threshold);
    }
    public void ResetLevel()
    {
        if (holdingObject != null)
        {
            Destroy(holdingObject.gameObject);
            holdingObject = null;
        }
        
        for (int i = 0; i < 3; i++)
        {
            foreach (var obj in placedObjects[i])
            {
                Destroy(obj.gameObject);
            }
            placedObjects[i].Clear();
        }
        
        node1 = null;
        node2 = null;
        node3 = null;
        level = null;
        dataDict.Remove(levelNumber);
        levelNumber = 0;
        isSaved = false;
        InitiateLevelObjects();
    }

    public void InitiateLevelObjects()
    {
        node1 = new Data.NodeData();
        node2 = new Data.NodeData();
        node3 = new Data.NodeData();
        level = new Data.LevelData();
        level.nodeList.Add(node1);
        level.nodeList.Add(node2);
        level.nodeList.Add(node3);
    }
    public void CreateLevel(int selectionCount)
    {
        InitiateLevelObjects();
        levelNumber = selectionCount + 1;
        dataDict.Add(levelNumber,new Data.LevelData());
        dataDict[levelNumber].nodeList.Add(node1);
        dataDict[levelNumber].nodeList.Add(node2);
        dataDict[levelNumber].nodeList.Add(node3);
        level = dataDict[levelNumber];
    }

    public void CheckSave()
    {
        if (!isSaved)
        {
            dataDict.Remove(levelNumber);
        }
    }
}
