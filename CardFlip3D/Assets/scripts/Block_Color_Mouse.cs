using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block_Color_Mouse : MonoBehaviour
{
    public Color startColor;
    public Color mouseOverColor;
    [SerializeField] private MouseLook1 controller;



    void OnMouseEnter()
    {
        if (controller != null)
        {
            controller.setCursorState(1);
        }

        GetComponent<Renderer>().material.SetColor("_Color", mouseOverColor);
    }

    void OnMouseExit()
    {
        if (controller != null)
        {
            controller.setCursorState(0);
        }
        GetComponent<Renderer>().material.SetColor("_Color", startColor);
    }
}
