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
    
    public TextMeshProUGUI[] texts;
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
            scale = new Vector3(0.25f, 0.25f, 0.25f);
        }
        LevelManager.Instance.CreateObject(objSelection.value,scale);
    }

    public void OnThreshold()
    {
        LevelManager.Instance.EditThreshold(int.Parse(threshold.text));
        texts[activeNode-1].text = "X / " + threshold.text;
    }

    public void GoNextNode()
    {
        activeNode += 1;
        LevelManager.Instance.GoNextNode();
    }

    public void SaveLevel()
    {
        LevelManager.Instance.SaveLevel();
    }
    
}
