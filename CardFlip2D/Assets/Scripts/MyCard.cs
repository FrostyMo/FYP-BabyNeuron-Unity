using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyCard : MonoBehaviour
{
    [SerializeField] private AudioSource flipSound;
    [SerializeField] private MyScene controller;
    [SerializeField] private GameObject Card_Back;
    private GameObject prefab;


    private void OnMouseDown()
    {
        if (Card_Back.activeSelf)
        {
            Debug.Log("Clicked" + this.name);
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

    public void ChangeSprite(int id, GameObject image)
    {
        _id = id;
        Quaternion rot = Quaternion.identity;
        rot = new Quaternion(rot.x, rot.y + 180, rot.z, rot.w);
        prefab = Instantiate(image, this.transform.position, rot);

        Vector3 scale = this.transform.localScale;
        scale = new Vector3(10f, 10f, 10f);
        prefab.transform.localScale = scale;
    }

    public void Unreveal()
    {
        Card_Back.SetActive(true);
    }

    public void Disappear()
    {
        Destroy(this.prefab);
        Destroy(this);
    }

}
