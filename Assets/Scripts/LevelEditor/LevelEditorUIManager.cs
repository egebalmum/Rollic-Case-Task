using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class LevelEditorUIManager : MonoBehaviour
{
    public static LevelEditorUIManager Instance;
    public Dropdown objSelection;
    public TMP_InputField scaleX;
    public TMP_InputField scaleY;
    public TMP_InputField scaleZ;
    public TMP_InputField threshold;
    public Button nextButton;
    public Button backButton;
    public TextMeshProUGUI[] thresholdTexts;
    public TextMeshProUGUI loadingText;
    public int activeNode=1;
    void Awake()
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
    
    
    
    public void OnCreate()
    {
        Vector3 scale;
        try
        {
            var newX = scaleX.text.Replace(".", ",");
            var newY = scaleY.text.Replace(".", ",");
            var newZ = scaleZ.text.Replace(".", ",");
            scale = new Vector3(float.Parse(newX), float.Parse(newY), float.Parse(newZ));
        }
        catch
        {
            scale = new Vector3(0.35f, 0.35f, 0.35f);
        }
        LevelManager.Instance.CreateObject(objSelection.value,scale);
    }

    public void OnThreshold()
    {
        LevelManager.Instance.EditThreshold(int.Parse(threshold.text));
        thresholdTexts[activeNode-1].text = "X / " + threshold.text;
    }

    public void OnThreshold(int node, int value)
    {
        thresholdTexts[node-1].text = "X / " + value;
    }

    public void GoNextNode()
    {
        activeNode += 1;
        if (activeNode == 3)
        {
            nextButton.interactable = false;
        }
        else if (activeNode == 2)
        {
            backButton.interactable = true;
        }
        LevelManager.Instance.GoNextNode();
    }

    public void GoBackNode()
    {
        activeNode -= 1;
        if (activeNode == 2)
        {
            nextButton.interactable = true;
        }
        else if (activeNode == 1)
        {
            backButton.interactable = false;
        }
        LevelManager.Instance.GoBackNode();
    }
    public void SaveLevel()
    {
        loadingText.text = "Loading...";
        LevelManager.Instance.SaveLevel();
        StartCoroutine(WaitForSave(2));
    }

    IEnumerator WaitForSave(float time)
    {
        yield return new WaitForSeconds(time);
        loadingText.text = "Saved!";
    }

    public void ResetUIManager()
    {
        loadingText.text = String.Empty;
        scaleX.text = String.Empty;
        scaleY.text = String.Empty;
        scaleZ.text = String.Empty;
        threshold.text = String.Empty;
        foreach (var thresholdObj in thresholdTexts)
        {
            thresholdObj.text = "X / X";
        }
    }
}
