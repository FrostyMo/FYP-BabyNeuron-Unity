using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCard : MonoBehaviour
{
    [SerializeField] private AudioSource flipSound;
    [SerializeField] private SceneController controller;
    [SerializeField] private GameObject Card_Back;

    private void OnMouseDown()
    {
        if (Card_Back.activeSelf)
        {
            // Debug.Log("Clicked" + this.name);
            flipSound.Play();
            Card_Back.SetActive(false);
            controller.CardRevealed(this);
        }
    }

    private int _id;
    public int id
    {
        get { return _id; }
    }

    public void ChangeSprite(int id, Sprite image)
    {
        _id = id;
        GetComponent<SpriteRenderer>().sprite = image;
    }

    public void Unreveal()
    {
        Card_Back.SetActive(true);
    }

    public void Disappear()
    {
        Destroy(Card_Back.transform.parent.gameObject);
    }

}
