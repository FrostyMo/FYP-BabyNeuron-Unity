using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
using TMPro;

public class SceneController : MonoBehaviour
{

    public int gridRows;
    public int gridCols;
    public float offsetX = 8f * 30f;
    public float offsetY = -12f * 25f;
    Vector2 start_position;
    [HideInInspector] public List<MainCard> pictureList;

    [SerializeField] public MainCard originalCard;
    [SerializeField] private Sprite[] images;
    public bool updated = false;
    public bool gameover = false;

    private MainCard _firstCard;
    private MainCard _secondCard;

    public static int _score = 0;
    [SerializeField] private TextMesh scoreLabel;
    [SerializeField] private TextMeshProUGUI Levelnumber;

    /////////////////////////////////////////////////////
    public enum EPairNumber
    {
        NotSet = 0,
        Level1 = 4,
        Level2 = 8,
        Level3 = 12,
        Level4 = 16,
        Level5 = 20,
        Level6 = 24,

    }
    public Transform spawnPosition;
    private Vector2 _offset = new Vector2(1.52f, 1.52f);
    //private Vector2 _offset = new Vector2(8f * 30f, -12f * 25f);
    public Vector2 startPos = new Vector2(1.52f, 1.52f);

    public static SceneController instance;
    public static EPairNumber PairNumber;

    private float scaleFactor = 1f;
    public int numberOfcards;


    private int division;
    /////////////////////////////////////////////////////

    private void Start()
    {

        scoreLabel.text = "Score: " + _score;
        Vector3 startPos = originalCard.transform.position;
        start_position = new Vector2(startPos.x, startPos.y);
        //Debug.Log("NUMBERRRR  " + PairNumber);
        if (PairNumber == EPairNumber.Level1)
        {
            //Debug.Log("IN LEVEL 1 ");
            gridRows = 2;
            gridCols = 2;
            numberOfcards = 4;
            // scaleFactor = 1f;
            division = 2;
            spawnPictureMesh(gridRows, gridCols, startPos, _offset, false);
            movePicture(gridRows, gridCols, startPos, _offset);
            Levelnumber.text = "Level 1";
        }

        else if (PairNumber == EPairNumber.Level2)
        {
            gridRows = 2;
            gridCols = 4;
            numberOfcards = 8;
            // scaleFactor = 1f;
            division = 2;
            spawnPictureMesh(gridRows, gridCols, startPos, _offset, false);
            movePicture(gridRows, gridCols, startPos, _offset);
            Levelnumber.text = "Level 2";
        }
        else if (PairNumber == EPairNumber.Level3)
        {
            gridRows = 3;
            gridCols = 4;
            numberOfcards = 12;
            // scaleFactor = 1f;
            division = 4;
            spawnPictureMesh(gridRows, gridCols, startPos, _offset, false);
            movePicture(gridRows, gridCols, startPos, _offset);
            Levelnumber.text = "Level 3";

        }
        else if (PairNumber == EPairNumber.Level4)
        {
            gridRows = 4;
            gridCols = 4;
            numberOfcards = 16;
            scaleFactor = 0.9f;
            division = 4;
            //originalCard.transform.localScale = originalCard.transform.localScale * scaleFactor;
            spawnPictureMesh(gridRows, gridCols, startPos, _offset, false);
            movePicture(gridRows, gridCols, startPos, _offset);
            Levelnumber.text = "Level 4";

        }
        else if (PairNumber == EPairNumber.Level5)
        {
            gridRows = 4;
            gridCols = 5;
            numberOfcards = 20;
            scaleFactor = 0.8f;
            division = 4;
            spawnPictureMesh(gridRows, gridCols, startPos, _offset, false);
            movePicture(gridRows, gridCols, startPos, _offset);
            Levelnumber.text = "Level 5";

        }
        else if (PairNumber == EPairNumber.Level6)
        {
            gridRows = 4;
            gridCols = 6;
            numberOfcards = 24;
            scaleFactor = 0.75f;
            division = 4;
            spawnPictureMesh(gridRows, gridCols, startPos, _offset, false);
            movePicture(gridRows, gridCols, startPos, _offset);
            Levelnumber.text = "Level 6";

        }


    }

    public void Update()
    {
        //Updating first card
        if (updated == false)
        {
            if (PairNumber == EPairNumber.Level4 || PairNumber == EPairNumber.Level5 || PairNumber == EPairNumber.Level6)
            {
                originalCard.transform.localScale = originalCard.transform.localScale * scaleFactor;
                updated = true;
            }
        }

        //Goint back to level menu on game completion
        int target = (gridCols * gridRows) / 2;
        if (numberOfcards <= 0 && gameover == false)
        {
            new WaitForSeconds(4);
            //Debug.Log("Game over");
            // SceneController.DisplayDialog("GAME OVER!",
            //     "Score: " + _score
            //     , "Ok");
            if (PairNumber == EPairNumber.Level1)
            {
                PairNumber = EPairNumber.Level2;
                SceneManager.LoadScene("Scene_001");
            }
            else if (PairNumber == EPairNumber.Level2)
            {
                PairNumber = EPairNumber.Level3;
                SceneManager.LoadScene("Scene_001");
            }
            else if (PairNumber == EPairNumber.Level3)
            {
                PairNumber = EPairNumber.Level4;
                SceneManager.LoadScene("Scene_001");
            }
            else if (PairNumber == EPairNumber.Level4)
            {
                PairNumber = EPairNumber.Level5;
                SceneManager.LoadScene("Scene_001");
            }
            else if (PairNumber == EPairNumber.Level5)
            {
                PairNumber = EPairNumber.Level6;
                SceneManager.LoadScene("Scene_001");
            }
            else if (PairNumber == EPairNumber.Level6)
            {
                //PairNumber=EPairNumber.Level3
                SceneManager.LoadScene("MainMenu");
            }

            gameover = true;
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
            //Debug.Log("Array " + i + ": " + newArray[i]);

        }
        return newArray;
    }


    // Check card when revealed



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
    public void CardRevealed(MainCard card)
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
            numberOfcards -= 2;
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

    //Spawning the cards
    private void spawnPictureMesh(int rows, int columns, Vector2 pos, Vector2 offset, bool scaleDown)
    {

        int size = rows * columns; // 3 x 4

        int imageNumber = 0;
        int[] _numbers = new int[size];

        // Assign division number of times the imageNumber
        // to the array _numbers... e.g if size = 4
        // _numbers = {0, 1, 2, 3}... and if size = 8
        // _numbers = {0, 0, 1, 1, 2, 2, 3, 3}
        // 00 11 22 33 44
        for (int i = 0; i < size; i++)
        {
            /*
                0 1 2 3 4 5 6 7 8 9 10 11
                0 0 0 1 1 1 2 2 2 3 03 03
            */
            if (i % 2 == 0 && i != 0)
            {
                imageNumber++;
            }

            if (imageNumber == images.Length)
            {
                imageNumber = 0;
            }
            // Debug.Log("hi" + imageNumber);
            _numbers[i] = imageNumber;
        }

        // Randomize the numbers so they appear in random order
        _numbers = ShuffleArray(_numbers);

        for (int col = 0; col < columns; col++)
        {
            for (int row = 0; row < rows; row++)
            {

                //our code
                MainCard card;
                if (col == 0 && row == 0)
                {
                    card = originalCard;
                }
                else
                {
                    card = Instantiate(originalCard) as MainCard;
                }
                card.name = card.name + 'c' + col + 'r' + row + 'r';

                card.transform.localScale = card.transform.localScale * scaleFactor;
                int index = row * gridCols + col;
                int id = _numbers[index];
                card.ChangeSprite(id, images[id]);
                pictureList.Add(card);
            }
        }

        //applyTextures();
    }

    //Moving the pictures in particular direction after spawning
    private void movePicture(int rows, int columns, Vector2 pos, Vector2 offset)
    {
        Vector3 startPos = originalCard.transform.position;
        var index = 0;
        for (int col = 0; col < columns; col++)
        {
            for (int row = 0; row < rows; row++)
            {
                //var targetPos = new Vector3((pos.x + (offset.x * row)), (pos.y - (offset.y * col)), 0.0f);
                float posX = (offsetX * col * scaleFactor) + startPos.x;
                float posY = (offsetY * row * scaleFactor) + startPos.y;
                var targetPos = new Vector3(posX, posY, startPos.z);
                StartCoroutine(moveToPosition(targetPos, pictureList[index]));
                index++;
            }
        }
    }

    private IEnumerator moveToPosition(Vector3 target, MainCard obj)
    {
        var dist = 300;

        while (obj.transform.position != target)
        {
            obj.transform.position = Vector3.MoveTowards(obj.transform.position, target, dist * Time.deltaTime);
            yield return 0;
        }
    }

}
