using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public int currentLevel;

    public static event Action OnMenuState;
    public static event Action OnRunningState;
    public static event Action OnControlState;
    
    private GameState _gameState;
    private List<Level> _levels;
    public class Level
    {
        public int id;
        public int pool1;
        public int pool2;
        public int pool3;
        public GameObject levelObject;

        public Level(int _id, int _pool1, int _pool2, int _pool3, GameObject _levelObject)
        {
            id = _id;
            pool1 = _pool1;
            pool2 = _pool2;
            pool3 = _pool3;
            levelObject = _levelObject;
        }
    }
    public enum GameState
    {
        menu,
        running,
        control
    }

    public void SetGameState(GameState state)
    {
        if (state == GameState.menu)
        {
            OnMenuState?.Invoke();
        }
        else if (state == GameState.running)
        {
            OnRunningState?.Invoke();
        }
        else if (state == GameState.control)
        {
            OnControlState?.Invoke();
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
        SetGameState(GameState.running);
    }

    
    void Update()
    {
        
    }
}
