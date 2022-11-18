using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class GameManager : MonoBehaviour
{
    //SINGLETON
    public static GameManager Instance;

    //EVENTS
    public static event Action StartEnter;
    public static event Action StartExit;
    public static event Action RunningEnter;
    public static event Action RunningExit;
    public static event Action ControlEnter;
    public static event Action ControlExit;
    public static event Action RestEnter;
    public static event Action RestExit;
    public static event Action WinEnter;
    public static event Action WinExit;
    public static event Action LoseEnter;
    public static event Action LoseExit;
    public static event Action Connecting;
    [SerializeField] private GameObject[] collectableTypes;
    public GameState _gameState;
    public GameObject levelTemp;
    public List<GameObject> levelObjects;
    public List<Level> levels;
    public List<LevelManager.LevelData> levelDatas;
    public Level curLevel;
    public LevelNode curNode;
    public int savedLevelID = 1;

    private Player _player;
    public enum GameState
    {
        Start,
        Running,
        Control,
        Rest,
        Lose,
        Win,
        Connecting
    }

    public void SetGameState(GameState state)
    {
        if (_gameState == state)
        {
            print("Error: Trying to setting Game State with the same state");
            return;
        }
        
        switch (_gameState)
        {
            case GameState.Start:
                StartExit?.Invoke();
                break;
            case GameState.Running:
                RunningExit?.Invoke();
                break;
            case GameState.Control:
                ControlExit?.Invoke();
                break;
            case GameState.Rest:
                RestExit?.Invoke();
                break;
            case GameState.Lose:
                LoseExit?.Invoke();
                break;
            case GameState.Win:
                WinExit?.Invoke();
                break;
            default:
                print("Error in SetGameState switch1");
                break;
        }
        switch (state)
        {
            case GameState.Start:
                StartEnter?.Invoke();
                break;
            case GameState.Running:
                RunningEnter?.Invoke();
                break;
            case GameState.Control:
                ControlEnter?.Invoke();
                break;
            case GameState.Rest:
                RestEnter?.Invoke();
                break;
            case GameState.Lose:
                LoseEnter?.Invoke();
                break;
            case GameState.Win:
                WinEnter?.Invoke();
                break;
            case GameState.Connecting:
                Connecting?.Invoke();
                break;
            default:
                print("Error in SetGameState switch2");
                break;
        }
        _gameState = state;
    }
    
    public GameState GetGameState()
    {
        return _gameState;
    }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    void Start()
    {
        _player = FindObjectOfType<Player>();
        levelObjects = new List<GameObject>();
        levelDatas = new List<LevelManager.LevelData>();
        LoadLevels();
        if (savedLevelID > 0)
        {
            curLevel = levels[savedLevelID - 1];
            curNode = curLevel.nodes[0];
            curNode.EnableNode();
        }
        else
        {
            curLevel = levels[0];
            curNode = curLevel.nodes[0];
            curNode.EnableNode();
        }

        
        
        SetGameState(GameState.Start);
        
    }

    void ControlPoint()
    {
        if (_player.transform.position.z >= curNode.controlPoint.position.z && GetGameState() == GameState.Running)
        {
            Vector3 snapToEndPoint = _player.transform.position;
            snapToEndPoint.z = curNode.controlPoint.position.z;
            _player.transform.position = snapToEndPoint;
            SetGameState(GameState.Control);
        }
    }

    public void GoNextNode()
    {
        if (curNode.ID != 3)
        {
            curNode.DisableNode();
            curNode = curLevel.nodes[curNode.ID];
            curNode.EnableNode();
            SetGameState(GameState.Running);
        }
        else
        {
            curLevel = levels[curLevel.ID];  //GoingToUpgradeThis
            curNode.DisableNode();
            curNode = curLevel.nodes[0];
            curNode.EnableNode();
            SetGameState(GameState.Win);
        }
    }

    public void LoseLevel()
    {
        SetGameState(GameState.Lose);
    }
    
    
    public void LoadLevels()
    {
        DirectoryInfo dir = new DirectoryInfo(Application.dataPath+"/Levels/");
        int levelCount = (dir.GetDirectories().Length);
        for (int i = 1; i <= levelCount; i++)
        {
            string json =  File.ReadAllText(Application.dataPath + "/Levels/Level" + (i) + "/" + "node1.json");
            LevelManager.NodeData nodeData1 = JsonUtility.FromJson<LevelManager.NodeData>(json);
            
            json =  File.ReadAllText(Application.dataPath + "/Levels/Level" + (i) + "/" + "node2.json");
            LevelManager.NodeData nodeData2 = JsonUtility.FromJson<LevelManager.NodeData>(json);
            
            json =  File.ReadAllText(Application.dataPath + "/Levels/Level" + (i) + "/" + "node3.json");
            LevelManager.NodeData nodeData3 = JsonUtility.FromJson<LevelManager.NodeData>(json);
            
            var levelobj = Instantiate(levelTemp, transform.position + Vector3.forward * 135 * (i - 1), Quaternion.identity);
            
            levelObjects.Add(levelobj);
            var levelScript = levelobj.GetComponent<Level>();
            levelScript.ID = i;
            levels.Add(levelScript);
            
            var levelData = new LevelManager.LevelData();
            levelData.nodeList.Add(nodeData1);
            levelData.nodeList.Add(nodeData2);
            levelData.nodeList.Add(nodeData3);
            levelDatas.Add(levelData);
            for (int j = 0; j < 3; j++)
            {
                levelScript.nodes[j].poolThreshold = levelDatas[i-1].nodeList[j].threshold;
                int objCount = levelDatas[i-1].nodeList[j].positions.Count;
                for (int k = 0; k < objCount; k++)
                {
                    var collectable = Instantiate(collectableTypes[levelDatas[i-1].nodeList[j].objectTypes[k]],
                        levelDatas[i-1].nodeList[j].positions[k] + Vector3.forward*(135*(i-1)), Quaternion.identity);
                    collectable.transform.localScale = levelDatas[i - 1].nodeList[j].scales[k];
                }
                levelScript.nodes[j].UpdateScore();
            }
        }
    }
    
    public void RestartLevel()
    {
        //RESTART
    }

    public void GoNextLevel()
    {
        SetGameState(GameState.Connecting);
        
    }
    void Update()
    {
        ControlPoint();
    }
}
