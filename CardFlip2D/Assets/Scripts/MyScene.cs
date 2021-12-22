using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyScene : MonoBehaviour
{
    [SerializeField] public int gridRows;
    public const int gridCols = 4;
    public const float offsetX = 7f * 30f;
    public const float offsetY = -10f * 25f;

    [SerializeField] private MyCard originalCard;
    [SerializeField] private GameObject[] prefabs;

    private void Start()
    {
        // position of the first card
        Vector3 startPos = originalCard.transform.position;

        int size = gridRows * gridCols;
        int division;

        // Choose the smaller of the two
        // for the number of each card item <=> (division)
        if (gridRows <= gridCols)
        {
            division = gridRows;
        }
        else
        {
            division = gridCols;
        }

        int imageNumber = 0;
        int[] _numbers = new int[size];

        // Assign division number of times the imageNumber
        // to the array _numbers... e.g if size = 4
        // _numbers = {0, 1, 2, 3}... and if size = 8
        // _numbers = {0, 0, 1, 1, 2, 2, 3, 3}
        for (int i = 0; i < size; i++)
        {
            /*
                0 1 2 3 4 5 6 7 8 9 10 11
                0 0 0 1 1 1 2 2 2 3 03 03
            */
            if (i % division == 0 && i != 0)
                imageNumber++;
            // Debug.Log("hi" + imageNumber);
            _numbers[i] = imageNumber;
        }

        // Randomize the numbers so they appear in random order
        _numbers = ShuffleArray(_numbers);

        // For each item in the grid
        // if it is the first object, call it originalCard
        // else make it an instance of MainCard
        for (int i = 0; i < gridCols; i++)
        {
            for (int j = 0; j < gridRows; j++)
            {
                MyCard card;
                if (i == 0 && j == 0)
                {
                    card = originalCard;
                }
                else
                {
                    card = Instantiate(originalCard) as MyCard;
                }
                int index = j * gridCols + i;
                int id = _numbers[index];

                float posX = (offsetX * i) + startPos.x;
                float posY = (offsetY * j) + startPos.y;

                // Set the image of the main card using index
                // Instantiate(prefabs[id], new Vector3(posX, posY, startPos.z), Quaternion.identity);




                // Set the position of the card
                card.transform.position = new Vector3(posX, posY, startPos.z);
                card.ChangeSprite(id, prefabs[id]);
            }
        }
    }

    // Shuffle the array using random from i to length of array
    private int[] ShuffleArray(int[] numbers)
    {
        int[] newArray = numbers.Clone() as int[];

        for (int i = 0; i < newArray.Length; i++)
        {
            int temp = newArray[i];
            int random = Random.Range(i, newArray.Length);
            newArray[i] = newArray[random];
            newArray[random] = temp;

        }
        for (int i = 0; i < newArray.Length; i++)
        {
            Debug.Log("Array " + i + ": " + newArray[i]);

        }
        return newArray;
    }


    // Check card when revealed

    private MyCard _firstCard;
    private MyCard _secondCard;

    private int _score = 0;
    [SerializeField] private TextMesh scoreLabel;

    public bool canReveal
    {
        get
        {
            return _secondCard == null;
        }
    }

    // If card(s) revealed, check whether
    // two cards revealed or one
    // If two, start a co routine
    public void CardRevealed(MyCard card)
    {
        if (_firstCard == null)
        {
            _firstCard = card;
        }
        else
        {
            _secondCard = card;
            StartCoroutine(CheckMatch());
        }
    }

    // If both cards' ids match, make them disappear
    private IEnumerator CheckMatch()
    {
        if (_firstCard.id == _secondCard.id)
        {
            _score++;
            scoreLabel.text = "Score: " + _score;
            yield return new WaitForSeconds(0.3f);
            _firstCard.Disappear();
            _secondCard.Disappear();
        }
        else
        {
            yield return new WaitForSeconds(0.5f);
            _firstCard.Unreveal();
            _secondCard.Unreveal();
        }
        _firstCard = null;
        _secondCard = null;
    }

}
