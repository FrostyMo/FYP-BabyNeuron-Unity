using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class pictureManager : MonoBehaviour
{
    public Transform spawnPosition;
    public Canvas canvas;
    public Vector3 startPos;
    public static int blockno;
    public List<GameObject> blockList;

    [HideInInspector]
    public List<GameObject> spawnList;
    private Vector2 _offset;
    private Vector3 _scale;
    private int _level;


    private int _rows, _columns;
    private bool[] checkBlock;
    // Start is called before the first frame update
    void Start()
    {

        refresh();

    }

    public void refresh()
    {

        checkBlock = new bool[blockList.Count];
        _level = GameSettings.instance.getLevel();
        // Debug.Log(_level);
        GameObject t = GameObject.Find("Canvas/level_no");
        show_level script = (show_level)t.GetComponent(typeof(show_level));
        script.setLevelNumber(_level);
        //t.text = "Level"+ _level.ToString();
        //Debug.Log(GameSettings.instance.levels_info[_level-1]);
        startPos = GameSettings.instance.levels_info[_level - 1].start_position;
        _offset = GameSettings.instance.levels_info[_level - 1].offset;
        _scale = GameSettings.instance.levels_info[_level - 1].scale;
        _rows = GameSettings.instance.levels_info[_level - 1].rows;
        _columns = GameSettings.instance.levels_info[_level - 1].columns;
        //Debug.Log(GameSettings.instance.levels_info[_level-1]);
        // Debug.Log("columns =" + _columns);
        // Debug.Log("rows =" + _rows);
        spawnPictureMesh(_rows, _columns, _scale);
        movePicture(_rows, _columns, startPos, _offset);
    }
    // Update is called once per frame
    void Update()
    {

        //checkBlock();
        //if(currentGamestate == gameState.deletingPuzzles){
        //    if(currentPuzzleState == puzzleState.canRotate){
        //        destroyBlock();
        //    }
        //}


    }
    
    private void spawnPictureMesh(int rows, int columns, Vector3 scale)
    {
        //getting the number of pairs required for the current level
        int n_blocks = GameSettings.instance.getPairNumber();

        var randomBlockIndex = Random.Range(0, blockList.Count);
        //creating the required number of pairs
        for (int i = 0; i < n_blocks; i++)
        {
            //creating the first block of the pair
            GameObject obj1 = Instantiate(blockList[randomBlockIndex]);
            spawnList.Add(obj1);

            //creating the second block of the pair
            GameObject obj2 = Instantiate(blockList[randomBlockIndex]);
            spawnList.Add(obj2);

            checkBlock[randomBlockIndex] = true;
            randomBlockIndex = (randomBlockIndex + 1) % blockList.Count;
            if (checkAll() == true)
            {
                randomBlockIndex = Random.Range(0, blockList.Count);
                for (int j = 0; j < blockList.Count; j++)
                    checkBlock[j] = false;
            }
        }
        
        // shuffling all the blocks so that they can be spawned randomly
        spawnList = ShuffleList(spawnList);

        for (int i = 0; i < spawnList.Count; i++)
        {
            // setting the spawn position and the scale or size of the block
            spawnList[i].transform.position = spawnPosition.position;
            spawnList[i].transform.localScale = scale;
        }
    }

    //function to check if all the blocks have been spawned
    private bool checkAll()
    {
        for (int i = 0; i < blockList.Count; i++)
        {
            if (checkBlock[i] == false)
            {
                return false;
            }
        }
        return true;
    }

    //function for shuffling a list
    List<GameObject> ShuffleList(List<GameObject> list)
    {
        GameObject tmp;

        // fisher–yates shuffle
        for (int i = 0; i < list.Count; i++)
        {

            // Pick random Element
            int j = Random.Range(i, list.Count);

            // Swap Elements
            tmp = list[i];
            list[i] = list[j];
            list[j] = tmp;
        }
        return list;
    }

    //function for moving the blocks from spawn-position to start-position
    private void movePicture(int rows, int columns, Vector3 pos, Vector2 offset)
    {

        int index = 0; // used for telling the amount blocks moved to their starting position
        int total_blocks = GameSettings.instance.getPairNumber() * 2;


        for (int col = 0; col < columns; col++)
        {
            for (int row = 0; row < rows; row++)
            {
                //condition to check if the total blocks have been spawned.
                if (index >= total_blocks)
                {
                    break;
                }

                // calculate the start postion of the the each block positionX = pos.x + (offset.x * col)
                // and postionY = pos.y - (offset.y * row)
                var targetPos = new Vector3((pos.x + (offset.x * col)), (pos.y - (offset.y * row)), pos.z);

                StartCoroutine(moveToPosition(targetPos, spawnList[index]));
                index++;

            }
        }
    }

    //used for moving the given block => obj to the position => target
    private IEnumerator moveToPosition(Vector3 target, GameObject obj)
    {
        var dist = 18; // tells the speed of the block when its moving towards the target position

        while (obj.transform.position != target)
        {
            obj.transform.position = Vector3.MoveTowards(obj.transform.position, target, dist * Time.deltaTime);
            yield return 0;
        }
    }


}






