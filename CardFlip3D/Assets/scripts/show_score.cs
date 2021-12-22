using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class show_score : MonoBehaviour
{
    public TMP_Text score;
    public static int tscore; // used to store the total score at the time
    public int val; // used for telling where the script is being used

    // Start is called before the first frame update
    void Start()
    {
        // val == 1 when script is being used in showLevel_scene
        if (val == 1)
        {
            tscore = GameSettings.instance.getTotalScore();
            string str = "Total Score : " + tscore.ToString();
            score.text = str;
            // Debug.Log("tscore" + tscore);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    // used for setting and updating the score
    public void setScore(int l)
    {
        block_rotation.score = 0;
        string str = "Total Score : " + l.ToString();
        //Debug.Log(str);
        score.text = str;
    }
}
