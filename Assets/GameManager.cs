using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using TMPro;

public class GameManager : MonoBehaviour
{
    public GameObject snake_piece;
    public GameObject Food;
    public GameObject Obs;
    public TextMeshProUGUI Timer;
    public TextMeshProUGUI gameOverText;
    


    public float score;
    public TextMeshProUGUI scoreText;

    public float timeRemaining = 5 ;
    


    List<Vector3> positions = new List<Vector3>();
    List<GameObject> snake = new List<GameObject>();
    List<Vector3> extensions = new List<Vector3>();


    Vector3 direction = new Vector3(0, 0, .15f);

    int level_width =28;
    int level_height =38;

    public bool game_over = false;

    bool is_locked = false;

    int starting_count = 30;


    void Start()
    {

        


        UpdateScore(0);

        UpdateTime(timeRemaining);
        
        gameOverText.gameObject.SetActive(false);

        

        for (int i = 0; i < starting_count; i++)
        {
            positions.Add(new Vector3(0, 0, (i - starting_count)*0.10f));

            GameObject new_snake_piece = Instantiate(snake_piece);
            new_snake_piece.transform.position = positions[i];

            if (i == starting_count - 1)
            {

                new_snake_piece.AddComponent<SnakePiece>();

               
            }
            else if (i > starting_count - 20)
            {
                new_snake_piece.tag = "Untagged";
            }

            snake.Add(new_snake_piece);
        }





        StartCoroutine(MoveSnake());
        StartCoroutine(CreateFood());

    }


    void Update()
    {


        

        if(timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
            UpdateTime(timeRemaining);

        }
        else
        {
            UpdateTime(0);
            GameObject.FindObjectOfType<GameManager>().game_over = true;
            
        }





        if (Input.GetKey(KeyCode.UpArrow))
        {
            snake[snake.Count - 1].transform.Rotate(new Vector3(0, -Time.deltaTime * 260, 0));
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            snake[snake.Count - 1].transform.Rotate(new Vector3(0, Time.deltaTime * 260, 0));
        }


        if (Input.GetKeyDown(KeyCode.Backspace)) SceneManager.LoadScene("SampleScene");
    }

    IEnumerator MoveSnake()
    {
        yield return new WaitForSeconds(0.02f);

        if (game_over) yield break;
        


        bool grow_snake = false;
        if (extensions.Count > 0 && extensions[0] == positions[0])
        {
            grow_snake = true;
        }

        positions.RemoveAt(0);
        positions.Add(positions[positions.Count - 1] + snake[snake.Count - 1].transform.forward * .10f);

        for (int i = 0; i < positions.Count; i++)
        {
            snake[i].transform.position = positions[i];
        }

        if (grow_snake)
        {
            positions.Insert(0, extensions[0]);

            GameObject new_snake_piece = Instantiate(snake_piece);
            new_snake_piece.transform.position = positions[0];

            snake.Insert(0, new_snake_piece);

            extensions.RemoveAt(0);

        }

        is_locked = false;


        StartCoroutine(MoveSnake());
        

    }

    IEnumerator CreateFood()
    {
        yield return new WaitForSeconds(2.5f);


        int x, z;
        x = Random.Range(-level_width / 2, level_width / 2);
        z = Random.Range(-level_height / 2, level_height / 2);
        
        

        GameObject new_food = Instantiate(Food);
        new_food.transform.position = new Vector3(x, 0, z);

        if(!game_over) StartCoroutine(CreateFood());

    }

    public void EatFood(Vector3 p)
    {

        extensions.Add(p);
        UpdateScore(1);

    }

    public void UpdateTime(float timeleft)
    {
        Timer.text = "Time Remaining: " + timeleft;
    }

    public void GameOver()
    {
        gameOverText.gameObject.SetActive(true);
    }

    
    public void UpdateScore(int scoreToAdd)
    {
        score += scoreToAdd;
        scoreText.text = "Score: " + score;
    }




}
