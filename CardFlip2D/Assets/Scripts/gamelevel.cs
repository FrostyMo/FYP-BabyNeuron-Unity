using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class gamelevel : MonoBehaviour
{

    [SerializeField] public SceneController.EPairNumber Levelnumber = SceneController.EPairNumber.NotSet;
    [SerializeField] public string gameScene;
    public Button yourButton;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("SettingPair number");
        // Button btn1 = GameObject.FindGameObjectWithTag("Level1Button").GetComponent<Button>();
        // btn1.onClick.AddListener(TaskOnClick);

        Button btn2 = yourButton.GetComponent<Button>();
        btn2.onClick.AddListener(TaskOnClick);
        //setGameOption();
    }

    public void setGameOption()
    {
        var comp = gameObject.GetComponent<gamelevel>();
        SceneController.PairNumber = comp.Levelnumber;
        Debug.Log("SettingPair number22 " + comp.Levelnumber);


        //SceneManager.LoadScene(gameScene);

    }

    public void TaskOnClick()
    {
        var comp = gameObject.GetComponent<gamelevel>();
        SceneController.PairNumber = comp.Levelnumber;
        Debug.Log("SettingPair number33 " + comp.Levelnumber);
        // LevelLoader.instance.LoadNextLevel(gameScene);
    }
    public void TaskOnClick1()
    {
        var comp = gameObject.GetComponent<gamelevel>();
        SceneController.PairNumber = comp.Levelnumber;
        Debug.Log("SettingPair number53 " + comp.Levelnumber);

    }

}
