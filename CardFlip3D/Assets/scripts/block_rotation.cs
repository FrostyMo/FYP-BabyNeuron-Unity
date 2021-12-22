using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class block_rotation : MonoBehaviour
{
    public float roatation_speed = 10f;// tells the rotation speed of block
    public bool rotate, reverse;// used to tell if the animal of the block is rotated
    public static bool first = false; // used for checking if the clicked block is first or not
    public GameObject score_object; // it is gameobject which is used to show score. 
    public GameObject screen; // reference to the interface that apears after each level finish
    public static GameObject block; // used to point to first clicked block
    private Quaternion start_angle, end_angle, current_angle;
    public static int score = 0; // it is used for the score of current level
    private static int n_pairs; // used to show no of pairs of the current level
    private static int rotated_blocks = 0;


    // Start is called before the first frame update
    void Start()
    {
        
        n_pairs = GameSettings.instance.getPairNumber(); // gets the total pairs present in the level
        start_angle = Quaternion.Euler(0, 0, 0); // the start position of a block is being set
        current_angle = start_angle; 
        end_angle = Quaternion.Euler(0, 180, 0); // the rotated angle

    }

    // Update is called once per frame
    void Update()
    {
        // if the block has been clicked then rotate is true and if the block has not been rotated 180 degrees then reverse is false
        if (rotate == true && reverse == false)
        {
            //rotate the block
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, end_angle, roatation_speed * Time.deltaTime);

            if (block != this.gameObject)// check if the clicked block is not being clicked the second time
            {
                //check if the clicked second block animal == clicked first block animal
                if (this.gameObject.tag == block.tag)
                {

                    first = false;
                    if (this.transform.rotation == Quaternion.Euler(0, -180, 0))
                    {
                        // remove the two clicked blocks from the screen
                        this.gameObject.SetActive(false); 
                        block.SetActive(false);
                        rotated_blocks = 0;
                        n_pairs--;
                        score++;
                    }


                }
                //this condition is run when second block animal != clicked first block animal
                else
                {
                    first = false;
                    block_rotation script = (block_rotation)block.GetComponent(typeof(block_rotation));
                    script.change_reverse();

                    if (this.transform.rotation == Quaternion.Euler(0, -180, 0))
                    {
                        rotate = false;
                        // score--;

                        StartCoroutine(go_back()); // used to set the bool variables for the rotation of the block
                    }
                }

            }


        }
        // condition check to see if all the pairs have been successsfully matched
        if (n_pairs == 0)
        {

            //Debug.Log("setUIbefore");
            setUI();
            n_pairs--;

        }
        // rotating the block back to original positions
        if (reverse == true && rotate == false)
        {

            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, start_angle, roatation_speed * Time.deltaTime);
            if (this.transform.rotation == Quaternion.Euler(0, 0, 0)) // this condition is ture when the animal is visible on the block
            {
                reverse = false;


            }
        }

    }

    public void OnMouseDown()
    {
        // check if the block is not rotated already
        if (rotate == false && rotated_blocks <= 2 )
        {
            rotate = true;
            rotated_blocks++;
            if (first == false)
            {
                // shows that first block has been clicked
                first = true;
                // reference to the first block for comparison when second block is clicked   
                block = this.gameObject;
            }
            //Debug.Log("rotated_blocks = "+ rotated_blocks);
        }
        //else
        //{
            //rotate = false;

        //}

    }

    public void setUI()
    {
        //Debug.Log("setUI");
        GameSettings.instance.levels_info[GameSettings.instance.getLevel() - 1]._score = 0;
        //Debug.Log("level score : "+ GameSettings.instance.levels_info[GameSettings.instance.getLevel()-1]._score);
        GameSettings.instance.setTotalScore();
        //Debug.Log("total score:"+GameSettings.instance.getTotalScore());
        //score_object = GameObject.Find("levelCompletion_screen/score");

        // show_score script = (show_score)score_object.GetComponent(typeof(show_score));
        // script.setScore(score);

        // activateUI();
        ButtonClick.setLevel(GameSettings.instance.getLevel() + 1); // level is incremented and its values are updated
        SceneManager.LoadScene("memory_game_scene");// load the scene after values have been updated

    }

    public void activateUI()
    {
        //screen = GameObject.Find("levelCompletion_screen");
        screen.SetActive(true);
    }


    IEnumerator go_back()
    {
        yield return new WaitForSeconds(0.000001f);
        reverse = true;
        rotated_blocks = 0;
        
    }

    // used when the second clicked block is not equal to the first clicked block
    public void change_reverse()
    {
        reverse = true;
        rotate = false;
        
    }




}
