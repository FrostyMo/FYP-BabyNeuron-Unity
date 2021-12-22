using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonClick : MonoBehaviour
{
    // used for changing the scene to the given scene 'scenName'
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);

    }

    // used for setting the number of pairs for each level on the basis of input
    public static void setLevel(int l)
    {

        if (l == 1)
        {
            GameSettings.instance.setPairNumber(GameSettings.pairNumber.level1);
        }
        else if (l == 2)
        {
            GameSettings.instance.setPairNumber(GameSettings.pairNumber.level2);
        }
        else if (l == 3)
        {
            GameSettings.instance.setPairNumber(GameSettings.pairNumber.level3);
        }
        else if (l == 4)
        {
            GameSettings.instance.setPairNumber(GameSettings.pairNumber.level4);
        }
        else if (l == 5)
        {
            GameSettings.instance.setPairNumber(GameSettings.pairNumber.level5);
        }
        else if (l == 6)
        {
            GameSettings.instance.setPairNumber(GameSettings.pairNumber.level6);
        }
        else if (l == 7)
        {
            GameSettings.instance.setPairNumber(GameSettings.pairNumber.level7);
        }
        else if (l == 8)
        {
            GameSettings.instance.setPairNumber(GameSettings.pairNumber.level8);
        }
        else if (l == 9)
        {
            GameSettings.instance.setPairNumber(GameSettings.pairNumber.level9);
        }
        else if (l == 10)
        {
            GameSettings.instance.setPairNumber(GameSettings.pairNumber.level10);
        }

        GameSettings.instance.setLevel(l);

    }

    //used for restarting the score of a level and reloading it
    public void retryLevel()
    {
        GameSettings.instance.setLevel(GameSettings.instance.getLevel()); // to restart the values of the level
        LoadScene("memory_game_scene"); // load the scene after values have been refreshed
    }

    //used for loading the next level
    public void nextLevel()
    {
        // deactivateUI(); // score showing interface is deactivated
        setLevel(GameSettings.instance.getLevel() + 1); // level is incremented and its values are updated
        LoadScene("memory_game_scene");// load the scene after values have been updated
    }

    // used for closing the bar that apears after each level completion
    public void deactivateUI()
    {
        GameObject s = GameObject.Find("levelCompletion_screen");
        s.SetActive(false);
    }
}
