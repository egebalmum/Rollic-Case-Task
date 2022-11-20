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
    public static event Action Ending;
    
    [SerializeField] private GameObject[] collectableTypes;
    public GameState _gameState;
    public GameObject levelTemp;
    public List<Level> levels;
    public List<Data.LevelData> levelDatas;
    public Level curLevel;
    public LevelNode curNode;
    public int savedLevelID;
    public Player _player;
    public enum GameState
    {
        Start,
        Running,
        Control,
        Rest,
        Lose,
        Win,
        Connecting,
        Ending
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
            case GameState.Connecting:
                Connecting?.Invoke();
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
            case GameState.Ending:
                Ending?.Invoke();
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
        levelDatas = new List<Data.LevelData>();
        savedLevelID = PlayerPrefs.GetInt("level", 1);
        LoadLevels();
        curLevel = levels[0];
        curNode = curLevel.nodes[0];
        curNode.EnableNode();
        DeactivateAllLevels();
        ActivateFirstTwoLevel();
        SetGameState(GameState.Start);
    }


    void DeactivateAllLevels()
    {
        foreach (var level in levels)
        {
            level.Deactivate();
        }
    }
    void ActivateFirstTwoLevel()
    {
        levels[0].Activate();
        if (levels.Count >= 2)
        {
            levels[1].Activate();
        }
    }

    void ActivateUpComingLevel()
    {
        if (levels.Count >= curIndex()+ 1 + 1)
        {
            levels[curIndex()+1].Activate();
        }
    }

    void DeactivateOldLevel()
    {
        if (curIndex() - 3 >= 0 && levels[curIndex()-3].isActive)
        {
            levels[curIndex() - 3].RestartLevel();
            levels[curIndex() - 3].Deactivate();
        }
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


    public void ContinueAfterCheck()
    {
        if (curNode.ID != 3)
        {
            GoNextNode();
        }
        else
        {
            GoNextLevel();
        }
    }
    private void GoNextNode()
    {
        curNode.DisableNode();
        curNode = curLevel.nodes[curNode.ID];
        curNode.EnableNode();
        SetGameState(GameState.Running);
    }
    
    private void GoNextLevel()
    {
        if (levels.Count == curIndex()+1)
        {
            SetGameState(GameState.Ending);
        }
        else
        {
            curLevel = levels[curIndex()+1];
            PlayerPrefs.SetInt("level",curLevel.ID);
            curNode.DisableNode();
            curNode = curLevel.nodes[0];
            curNode.EnableNode();
            SetGameState(GameState.Win);
            ActivateUpComingLevel();
            DeactivateOldLevel();
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
        for (int i = savedLevelID; i <= levelCount; i++)
        {
            string json =  File.ReadAllText(Application.dataPath + "/Levels/Level" + (i) + "/" + "node1.json");
            Data.NodeData nodeData1 = JsonUtility.FromJson<Data.NodeData>(json);
            
            json =  File.ReadAllText(Application.dataPath + "/Levels/Level" + (i) + "/" + "node2.json");
            Data.NodeData nodeData2 = JsonUtility.FromJson<Data.NodeData>(json);
            
            json =  File.ReadAllText(Application.dataPath + "/Levels/Level" + (i) + "/" + "node3.json");
            Data.NodeData nodeData3 = JsonUtility.FromJson<Data.NodeData>(json);
           
            var levelobj = Instantiate(levelTemp, transform.position + Vector3.forward * (150 * (i - 1) - 150 * (savedLevelID-1)), Quaternion.identity);
            var levelScript = levelobj.GetComponent<Level>();
            levelScript.ID = i;
            levelScript.gameField = levelobj;
            levels.Add(levelScript);
            var levelData = new Data.LevelData();
            levelData.nodeList.Add(nodeData1);
            levelData.nodeList.Add(nodeData2);
            levelData.nodeList.Add(nodeData3);
            levelDatas.Add(levelData);
            
            for (int j = 0; j < 3; j++)
            {
                levelScript.nodes[j].poolThreshold = levelDatas[(i-1)-(savedLevelID-1)].nodeList[j].threshold;
                int objCount = levelDatas[(i-1)-(savedLevelID-1)].nodeList[j].positions.Count;
                for (int k = 0; k < objCount; k++)
                {
                    var collectable = Instantiate(collectableTypes[levelData.nodeList[j].objectTypes[k]],
                        levelData.nodeList[j].positions[k] + Vector3.forward*(150 * (i - 1) - 150 * (savedLevelID-1)), Quaternion.identity);
                    collectable.transform.localScale = levelData.nodeList[j].scales[k];
                    levelScript.nodes[j].collectables.Add(collectable);
                }
                levelScript.nodes[j].UpdateScore();
            }
        }
    }
    
    public void RestartLevel()
    {
        _player.transform.position = curLevel.transform.position;
        curLevel.RestartLevel();
        curNode = curLevel.nodes[0];
        curNode.EnableNode();
        SetGameState(GameState.Rest);
    }
    void Update()
    {
        ControlPoint();
    }

    int curIndex()
    {
        return curLevel.ID - savedLevelID;
    }
    public void ExitGame()
    {
        Application.Quit();
    }
}
