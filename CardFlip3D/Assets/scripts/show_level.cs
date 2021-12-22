using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
public class show_level : MonoBehaviour
{
    public TMP_Text level;
    // Start is called before the first frame update
    void Start()
    {
        //level.text = "Level";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // used for setting the levelnumber heading in eachlevel
    public void setLevelNumber(int l){
        string str= "Level" + l.ToString();
        //Debug.Log(str);
        level.text = str;
    }
}
