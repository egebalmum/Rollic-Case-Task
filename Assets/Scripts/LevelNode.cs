using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelNode : MonoBehaviour
{
    [SerializeField] public int poolThreshold;
    [SerializeField] public Animation gateAnimation;
    [HideInInspector] public Transform controlPoint;
    [HideInInspector] public Transform endPoint;
    [HideInInspector] public TextMeshProUGUI text;
    [HideInInspector] public List<GameObject> collectables;
    [HideInInspector] public int ID;
    public int objectInPoolCount;
    void Awake()
    {
        InitializeLevelNode();
    }

    void InitializeLevelNode()
    {
        foreach (Transform tr in transform)
        {
            switch (tr.tag)
            {
                case "ControlPoint":
                    controlPoint = tr;
                    break;
                case "EndPoint":
                    endPoint = tr;
                    break;
                case "PoolText":
                    text = tr.GetChild(0).GetComponent<TextMeshProUGUI>();
                    break;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Object"))
        {
            StartCoroutine(PopCollectable(other.gameObject));
            objectInPoolCount += 1;
            text.text = objectInPoolCount + " / " + poolThreshold;
        }
    }

    IEnumerator PopCollectable(GameObject other)
    {
        yield return new WaitForSeconds(1.5f);
        other.SetActive(false);
        //effects
    }
    
    private void ControlPoolTimed()
    {
        Invoke(nameof(ControlPool),2);
    }
    private void ControlPool()
    {
        if (objectInPoolCount >= poolThreshold)
        {
            gateAnimation.Play("GateOpen");
            GameManager.Instance.Invoke(nameof(GameManager.Instance.GoNextNode),0.7f);
        }
        else
        {
            GameManager.Instance.LoseLevel();
        }
    }

    public void EnableNode()
    {
        GameManager.ControlEnter += ControlPoolTimed;
    }

    public void DisableNode()
    {
        GameManager.ControlEnter -= ControlPoolTimed;
    }

    public void RestartGateAnimation()
    {
        gateAnimation.Play("GateClose");
    }
    public void UpdateScore()
    {
        text.text = objectInPoolCount+" / " + poolThreshold;
    }
}
