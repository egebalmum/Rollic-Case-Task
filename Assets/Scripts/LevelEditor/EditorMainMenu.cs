using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;
using UnityEngine.SceneManagement;
public class EditorMainMenu : MonoBehaviour
{
    public List<Data.LevelData> levelDatas;
    [SerializeField] private TMP_Dropdown selection;
    public static EditorMainMenu Instance;

    [SerializeField] private GameObject UISelection;

    [SerializeField] private GameObject UIBuilder;
    // Start is called before the first frame update
    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
        levelDatas = new List<Data.LevelData>();
        LoadLevelsIntoLevelData();
        PrepareSelection();
    }

    void PrepareSelection()
    {
        for (int i = 0; i < levelDatas.Count; i++)
        {
            var option = new TMP_Dropdown.OptionData();
            option.text = "Level " + (i + 1).ToString();
            selection.options.Add(option);
        }
    }
    void LoadLevelsIntoLevelData()
    {
        DirectoryInfo dir = new DirectoryInfo(Application.dataPath + "/Levels/");
        int levelCount = (dir.GetDirectories().Length);
        for (int i = 1; i <= levelCount; i++)
        {
            string json = File.ReadAllText(Application.dataPath + "/Levels/Level" + (i) + "/" + "node1.json");
            Data.NodeData nodeData1 = JsonUtility.FromJson<Data.NodeData>(json);

            json = File.ReadAllText(Application.dataPath + "/Levels/Level" + (i) + "/" + "node2.json");
            Data.NodeData nodeData2 = JsonUtility.FromJson<Data.NodeData>(json);

            json = File.ReadAllText(Application.dataPath + "/Levels/Level" + (i) + "/" + "node3.json");
            Data.NodeData nodeData3 = JsonUtility.FromJson<Data.NodeData>(json);
            var levelData = new Data.LevelData();
            levelData.nodeList.Add(nodeData1);
            levelData.nodeList.Add(nodeData2);
            levelData.nodeList.Add(nodeData3);
            levelDatas.Add(levelData);
        }
    }

    public void OpenLevel()
    {
        LevelManager.Instance.mode = LevelManager.Mode.editOld;
        LevelManager.Instance.LoadLevel(levelDatas[selection.value],selection.value);
    }

    public void CreateLevel()
    {
        LevelManager.Instance.mode = LevelManager.Mode.createNew;
        LevelManager.Instance.CreateLevel(selection.options.Count);
    }

    public void AddNewSelection()
    {
        var newOption = new TMP_Dropdown.OptionData();
        newOption.text = "Level " + (selection.options.Count+1); 
        selection.options.Add(newOption);
    }

    public void GoBuildMode()
    {
        LevelManager.Instance.isBuildingModeOn = true;
        UIBuilder.SetActive(true);
        UISelection.SetActive(false);
    }

    public void GoSelectionMode()
    {
        LevelManager.Instance.isBuildingModeOn = false;
        UIBuilder.SetActive(false);
        UISelection.SetActive(true);
        LevelManager.Instance.CheckSave();
        LevelManager.Instance.ResetLevel();
        LevelEditorUIManager.Instance.ResetUIManager();
    }
}
