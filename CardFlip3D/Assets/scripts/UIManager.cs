using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    //Screen object variables
    public GameObject loginUI;
    public GameObject registerUI;
    public GameObject registerCorsiScoreUI;
    public GameObject registerPostCorsiScoreUI;
    public GameObject thankYouUI;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != null)
        {
            Debug.Log("Instance already exists, destroying object!");
            Destroy(this);
        }
    }

    //Functions to change the login screen UI

    public void ClearScreen() //Turn off all screens
    {
        loginUI.SetActive(false);
        registerUI.SetActive(false);

        registerPostCorsiScoreUI.SetActive(false);
        registerCorsiScoreUI.SetActive(false);
        thankYouUI.SetActive(false);
    }

    public void LoginScreen() //Back button
    {
        ClearScreen();
        loginUI.SetActive(true);
    }
    public void RegisterScreen() // Regester button
    {
        ClearScreen();
        registerUI.SetActive(true);
    }

    public void RegisterCorsiScoreScreen() //Corsi UI
    {
        ClearScreen();
        registerCorsiScoreUI.SetActive(true);
    }
    public void RegisterPostCorsiScoreScreen() //Corsi UI
    {
        ClearScreen();
        registerPostCorsiScoreUI.SetActive(true);
    }
    public void ThankYouScreen() //Back button
    {
        ClearScreen();
        thankYouUI.SetActive(true);
    }
}
