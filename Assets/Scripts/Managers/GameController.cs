using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

    Board m_gameBoard;
    Spawner m_spawner; // try to reference other game objects

    Shape m_activeShape; // keep track of current shape

    float m_dropInterval = .9f;
    public  float m_timeToDrop;

    float m_timeToNextkey;

    [Range(0.02f, 1f)] // range selector, so that could change the value in the editor 
    public float m_keyRepeatRate = 0.15f;

    float m_timeToNextKeyLeftRight;

    [Range(0.02f, 1f)]
    public float m_keyRepeatRateLeftRight = 0.25f;

    float m_timeToNextKeyDown;

    [Range(0.02f, 1f)]
    public float m_keyRepeatRateDown = 0.01f;

    float m_timeToNextKeyRotate;

    [Range(0.02f, 1f)]
    public float m_keyRepeatRateRotate = 0.25f;

    // game over flag
    bool m_gameOver = false;

    // game over panel
    public GameObject m_gameOverPanel;

    // Use this for initialization
    void Start () {
        m_gameBoard = GameObject.FindWithTag("Board").GetComponent<Board>();
        m_spawner = GameObject.FindWithTag("Spawner").GetComponent<Spawner>();

        // init time values
        m_timeToNextKeyLeftRight = Time.time + m_keyRepeatRateLeftRight;
        m_timeToNextKeyDown = Time.time + m_keyRepeatRateDown;
        m_timeToNextKeyRotate = Time.time + m_keyRepeatRateRotate;


        //if(m_spawner){
        //    if(m_activeShape == null) {
        //        m_activeShape = m_spawner.ShapeSpawner();    
        //    }

        //    m_spawner.transform.position = Vectorf.Round(m_spawner.transform.position);

        //}

        if(!m_gameBoard) {
            Debug.LogWarning("There is no game board defined!");
        }
        if(!m_spawner) {
            Debug.LogWarning("Warning there is no spawner defined!");
        } else {
            m_spawner.transform.position = Vectorf.Round(m_spawner.transform.position); // ???
            if(!m_activeShape) {
                m_activeShape = m_spawner.ShapeSpawner();
            }
        }

        if(m_gameOverPanel)
        {
            m_gameOverPanel.SetActive(false);
        }
    }
	
	// Update is called once per frame
	void Update () {

        if(!m_gameBoard || !m_spawner || !m_activeShape || m_gameOver) {
            return;
        }

        PlayerInput();
	}

    void PlayerInput() {
        // handle input
        if (Input.GetButton("MoveRight") && Time.time > m_timeToNextKeyLeftRight || Input.GetButtonDown("MoveRight")){
            m_activeShape.MoveRight();
            m_timeToNextKeyLeftRight = Time.time + m_keyRepeatRateLeftRight;

            if (!m_gameBoard.IsValidPosition(m_activeShape))
            {
                m_activeShape.MoveLeft();
                Debug.Log("hit the right boundary");
            }
        } else if (Input.GetButton("MoveLeft") && Time.time > m_timeToNextKeyLeftRight || Input.GetButtonDown("MoveLeft")){
            m_activeShape.MoveLeft();
            m_timeToNextKeyLeftRight = Time.time + m_keyRepeatRateLeftRight;

            if (!m_gameBoard.IsValidPosition(m_activeShape))
            {
                m_activeShape.MoveRight();
                Debug.Log("hit the left boundary");
            }
        }  else if (Input.GetButton("Rotate") && Time.time > m_timeToNextKeyRotate){
            m_activeShape.RotateRight();
            m_timeToNextKeyRotate = Time.time + m_keyRepeatRateRotate;

            if (!m_gameBoard.IsValidPosition(m_activeShape))
            {
                m_activeShape.RotateLeft();
                Debug.Log("rotate right");
            }
        } else if (Input.GetButton("MoveDown") && Time.time > m_timeToNextKeyDown || (Time.time > m_timeToDrop))
        {
            m_activeShape.MoveDown();
            m_timeToNextKeyDown = Time.time + m_keyRepeatRateDown;
            m_timeToDrop = Time.time + m_dropInterval;

            if (!m_gameBoard.IsValidPosition(m_activeShape))
            {
                if(m_gameBoard.IsOverLimit(m_activeShape))
                {
                    GameOver();
                } else
                {
                    LandShape();
                }
            }
        }

        //if (Time.time > m_timeToDrop)
        //{
        //    m_timeToDrop = Time.time + m_dropInterval;
        //    if (m_activeShape)
        //    {
        //        m_activeShape.MoveDown();
        //        if (!m_gameBoard.IsValidPosition(m_activeShape))
        //        {
        //            m_activeShape.MoveUp();
        //            m_gameBoard.StoreShapeInGrid(m_activeShape);
        //            if (m_spawner)
        //            {
        //                m_activeShape = m_spawner.ShapeSpawner(); // get new shape
        //            }
        //        }
        //    }
        //}

    }

    void LandShape() {
        //  m_timeToNextkey = Time.time; // ??
        // 下落到底端以后可以立即触发生成新的方块
        m_timeToNextKeyLeftRight = Time.time;
        m_timeToNextKeyDown = Time.time;
        m_timeToNextKeyRotate = Time.time;

        m_activeShape.MoveUp();
        m_gameBoard.StoreShapeInGrid(m_activeShape);
        m_activeShape = m_spawner.ShapeSpawner();

        m_gameBoard.ClearAllRows();
    }

    public void Restart()
    {
        Debug.Log("Restarted");
        Application.LoadLevel(Application.loadedLevel);

    }

    void GameOver()
    {
        m_gameOver = true;
        m_activeShape.MoveUp();
        Debug.LogWarning(m_activeShape.name + " is over the limit!");
        if (m_gameOverPanel)
        {
            m_gameOverPanel.SetActive(true);
        }
    }
}
 