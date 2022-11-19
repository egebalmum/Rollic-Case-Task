using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class InterfaceManager : MonoBehaviour
{
    public static InterfaceManager Instance;
    [SerializeField] public GameObject startingUI;
    [SerializeField] public GameObject gameUI;
    [SerializeField] public TextMeshProUGUI curLevel;
    [SerializeField] public TextMeshProUGUI nextLevel;
    [SerializeField] public Image[] nodeModels;
    [SerializeField] public GameObject winUI;
    [SerializeField] public GameObject loseUI;
    [SerializeField] public GameObject restUI;
    [SerializeField] public GameObject endingUI;
    private int nodeCount;
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    void Start()
    {
        GameManager.StartExit += StartExit;
        GameManager.StartExit += UpdateLevelNumbers;
        GameManager.WinEnter += WinEnter;
        GameManager.WinEnter += GameExit;
        GameManager.WinExit += WinExit;
        GameManager.WinExit += ResetColors;
        GameManager.WinExit += UpdateLevelNumbers;
        
        GameManager.LoseEnter += LoseEnter;
        GameManager.LoseEnter += GameExit;
        GameManager.LoseExit += LoseExit;
        GameManager.LoseExit += ResetColors;
        
        GameManager.RestEnter += RestEnter;
        GameManager.RestExit += RestExit;
        
        GameManager.ControlExit += ColorNodes;
        GameManager.Connecting += GameEnter;

        GameManager.RunningEnter += GameEnter;

        GameManager.Ending += Ending;
    }

    void StartExit()
    {
        startingUI.SetActive(false);
    }

    void WinEnter()
    {
        winUI.SetActive(true);
    }

    void WinExit()
    {
        winUI.SetActive(false);
    }

    void LoseEnter()
    {
        loseUI.SetActive(true);
    }

    void LoseExit()
    {
        loseUI.SetActive(false);
    }

    void ColorNodes()
    {
        nodeCount += 1;
        nodeModels[nodeCount-1].color = Color.yellow;
    }

    void ResetColors()
    {
        nodeCount = 0;
        foreach (var nodeModel in nodeModels)
        {
            nodeModel.color = Color.white;
        }
    }
    void RestEnter()
    {
        restUI.SetActive(true);
    }

    void RestExit()
    {
        restUI.SetActive(false);
    }

    void GameEnter()
    {
        gameUI.SetActive(true);
    }

    void GameExit()
    {
        gameUI.SetActive(false);
    }

    void Ending()
    {
        endingUI.SetActive(true);
    }
    void UpdateLevelNumbers()
    {
        int cur = GameManager.Instance.curLevel.ID;
        curLevel.text = cur.ToString();
        nextLevel.text = (cur + 1).ToString();
    }

    public void Connector()
    {
        GameManager.Instance.SetGameState(GameManager.GameState.Connecting);
    }
}
