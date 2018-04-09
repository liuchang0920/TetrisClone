using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

    Board m_gameBoard;
    Spawner m_spawner; // try to reference other game objects

    Shape m_activeShape; // keep track of current shape

    float m_dropInterval = .9f;
    float m_timeToDrop;
    float m_timeToNextkey;

    [Range(0.02f, 1)] // range selector, so that could change the value in the editor 
    public float m_keyRepeatRate = 0.25f;

	// Use this for initialization
	void Start () {
        m_gameBoard = GameObject.FindWithTag("Board").GetComponent<Board>();
        m_spawner = GameObject.FindWithTag("Spawner").GetComponent<Spawner>();

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
	}
	
	// Update is called once per frame
	void Update () {
        if(!m_gameBoard || !m_spawner || !m_activeShape) {
            return;
        }

        PlayerInput();
	}

    void PlayerInput() {
        // handle input
        if (Input.GetButton("MoveRight") && Time.time > m_timeToNextkey || Input.GetButtonDown("MoveRight")){
            m_activeShape.MoveRight();
            m_timeToNextkey = Time.time + m_keyRepeatRate;

            if (!m_gameBoard.IsValidPosition(m_activeShape))
            {
                m_activeShape.MoveLeft();
                Debug.Log("hit the right boundary");
            }
        } else if (Input.GetButton("MoveLeft") && Time.time > m_timeToNextkey || Input.GetButtonDown("MoveLeft")){
            m_activeShape.MoveLeft();
            m_timeToNextkey = Time.time + m_keyRepeatRate;

            if (!m_gameBoard.IsValidPosition(m_activeShape))
            {
                m_activeShape.MoveRight();
                Debug.Log("hit the left boundary");
            }
        }  else if (Input.GetButton("Rotate") && Time.time > m_timeToNextkey){
            m_activeShape.RotateRight();
            m_timeToNextkey = Time.time + m_keyRepeatRate;

            if (!m_gameBoard.IsValidPosition(m_activeShape))
            {
                m_activeShape.RotateLeft();
                Debug.Log("rotate right");
            }
        } else if (Input.GetButton("MoveDown") && Time.time > m_timeToNextkey || (Time.time > m_timeToDrop))
        {
            m_activeShape.MoveDown();
            m_timeToNextkey = Time.time + m_keyRepeatRate;
            m_timeToDrop = Time.time + m_dropInterval;

            if (!m_gameBoard.IsValidPosition(m_activeShape))
            {
                LandShape();
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
        m_timeToNextkey = Time.time; // ??
        m_activeShape.MoveUp();
        m_gameBoard.StoreShapeInGrid(m_activeShape);
        m_activeShape = m_spawner.ShapeSpawner();
    }
}
