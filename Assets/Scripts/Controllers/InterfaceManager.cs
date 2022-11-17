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
    private int nodeCount;
    [SerializeField] public GameObject winUI;
    [SerializeField] public Button winButton;
    [SerializeField] public GameObject loseUI;
    [SerializeField] public Button loseButton;
    [SerializeField] public GameObject restUI;
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
        
        GameManager.WinEnter += WinEnter;
        GameManager.WinEnter += GameExit;
        GameManager.WinExit += WinExit;
        GameManager.WinExit += ResetColors;
        
        GameManager.LoseEnter += LoseEnter;
        GameManager.LoseEnter += GameExit;
        GameManager.LoseExit += LoseExit;
        GameManager.LoseExit += ResetColors;
        
        GameManager.RestEnter += RestEnter;
        GameManager.RestExit += RestExit;
        
        GameManager.ControlExit += ColorNodes;
        GameManager.Connecting += GameEnter;

        GameManager.RunningEnter += GameEnter;
        

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




}
