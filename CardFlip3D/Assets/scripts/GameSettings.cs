using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSettings : MonoBehaviour
{
    public int level_no; // used to save the current level no
    private int total_score;// used to score the combined score of all levels

    
    public enum pairNumber
    {
        NotSet = 0,
        level1 = 2,
        level2 = 3,
        level3 = 4,
        level4 = 6,
        level5 = 8,
        level6 = 10,
        level7 = 12,
        level8 = 14,
        level9 = 16,
        level10 = 18,

    }

    // used to for storing the position, size , and score of the level_no
    [System.Serializable]
    public class level_info
    {
        public int level_no;
        public Vector2 offset;
        public Vector3 start_position;
        public Vector3 scale;
        public int rows;
        public int columns;
        public int _score;
    }

    //list for saving the level info of all the levels
    public List<level_info> levels_info;
    public struct Settings
    {
        public pairNumber number_of_pairs;
    };

    private Settings _gameSettings;

    public static GameSettings instance;

    void Awake()
    {
        if (instance == null)
        {
            DontDestroyOnLoad(target: this);
            instance = this;    // used to create a singleton script
        }
        else
        {
            Destroy(obj: this);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        total_score = 0; // total score set to zero when new game starts
        _gameSettings = new Settings();
        resetGameSettings();
    }
    public void setTotalScore()
    {
        total_score = 0;
        // for (int i = 0; i < levels_info.Count; i++)
        // {
        //     total_score += levels_info[i]._score;
        // }
        total_score = block_rotation.score;
    }

    public int getTotalScore()
    {
        return block_rotation.score;
    }



    public void setPairNumber(pairNumber number)
    {
        _gameSettings.number_of_pairs = number;
    }
    public void setLevel(int l)
    {
        level_no = l;
    }
    public int getLevel()
    {
        return level_no;
    }
    public int getPairNumber()
    {
        return (int)_gameSettings.number_of_pairs;
    }

    public void resetGameSettings()
    {
        _gameSettings.number_of_pairs = pairNumber.NotSet;
    }

    public void restartGame()
    {
        total_score = 0;
        for (int i = 0; i < levels_info.Count; i++)
        {
            levels_info[i]._score = 0;
        }
        block_rotation.score = 0;
        Debug.Log("total score = " + total_score);
    }
}
