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

    // sound manager reference
    SoundManager m_soundManager;

    // score manager
    ScoreManager m_scoreManager;

    // icon toggle for rotate goes in this class
    public IconToggle m_rotIconToggle;
    bool m_clockwise = true;

    // pause the game
    public bool m_isPaused = false;
    public GameObject m_pausePanel;

    // ghost 
    Ghost m_ghost;

    // holder
    Holder m_holder;

    // game over fx
    public ParticlePlayer m_gameOverFX;

    // Use this for initialization
    void Start () {
        m_gameBoard = GameObject.FindWithTag("Board").GetComponent<Board>();
        m_spawner = GameObject.FindWithTag("Spawner").GetComponent<Spawner>();
        m_soundManager = GameObject.FindObjectOfType<SoundManager>();
        m_scoreManager = GameObject.FindObjectOfType<ScoreManager>();
        m_ghost = GameObject.FindObjectOfType<Ghost>();
        m_holder = GameObject.FindObjectOfType<Holder>();

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

        if(!m_soundManager)
        {
            Debug.LogWarning("sound manager not found!");
        }
        if(!m_scoreManager)
        {
            Debug.LogWarning("There is no score manager defined.");
        }

        if(!m_spawner) {
            Debug.LogWarning("Warning there is no spawner defined!");
        } else {
            m_spawner.transform.position = Vectorf.Round(m_spawner.transform.position); // ???
            if(!m_activeShape) {
                m_activeShape = m_spawner.ShapeSpawner();
                Debug.LogWarning("active shape: " + m_activeShape.ToString());
            }
        }

        if (!m_ghost)
        {
            Debug.LogWarning("no ghost");
        }
        // reset game
        if(m_gameOverPanel)
        {
            m_gameOverPanel.SetActive(false);
        }
        if(m_pausePanel)
        {
            m_pausePanel.SetActive(false);
        }
        //if(m_soundManager.m_fxEnabled && m_soundManager.m_moveSound)
        //{
        //    AudioSource.PlayClipAtPoint(m_soundManager.m_moveSound, Camera.main.transform.position, m_soundManager.m_fxVolume);

        //}
    }
	
	// Update is called once per frame
	void Update () {

        if(!m_gameBoard || !m_spawner || !m_activeShape || m_gameOver || !m_soundManager || !m_scoreManager) {
            return;
        }

        PlayerInput();
	}

    void LateUpdate()
    {
        // Debug.LogWarning("about to draw ghost");
        if (m_ghost)
        {
            // Debug.LogWarning("drawing ghost");
            m_ghost.DrawGhost(m_activeShape, m_gameBoard);
        }
    }

    void PlayerInput() {
        // handle input
        if (Input.GetButton("MoveRight") && Time.time > m_timeToNextKeyLeftRight || Input.GetButtonDown("MoveRight")){
            m_activeShape.MoveRight();
            m_timeToNextKeyLeftRight = Time.time + m_keyRepeatRateLeftRight;

            if (!m_gameBoard.IsValidPosition(m_activeShape))
            {
                PlaySound(m_soundManager.m_errorSound, 1.0f);
                m_activeShape.MoveLeft();
                Debug.Log("hit the right boundary");
            } else
            {
                PlaySound(m_soundManager.m_moveSound, 1.0f);

            }
        } else if (Input.GetButton("MoveLeft") && Time.time > m_timeToNextKeyLeftRight || Input.GetButtonDown("MoveLeft")){
            m_activeShape.MoveLeft();
            m_timeToNextKeyLeftRight = Time.time + m_keyRepeatRateLeftRight;

            if (!m_gameBoard.IsValidPosition(m_activeShape))
            {
                PlaySound(m_soundManager.m_errorSound, 1.0f);
                m_activeShape.MoveRight();
                Debug.Log("hit the left boundary");
            }
            else
            {
                PlaySound(m_soundManager.m_moveSound, 1.0f);

            }
        }  else if (Input.GetButton("Rotate") && Time.time > m_timeToNextKeyRotate){
            // m_activeShape.RotateRight();
            m_activeShape.RotateClockwise(m_clockwise);
            m_timeToNextKeyRotate = Time.time + m_keyRepeatRateRotate;

            if (!m_gameBoard.IsValidPosition(m_activeShape))
            {
                PlaySound(m_soundManager.m_errorSound, 1.0f);
                // m_activeShape.RotateLeft();
                m_activeShape.RotateClockwise(!m_clockwise);

            }
            else
            {
                PlaySound(m_soundManager.m_moveSound, 1.0f);

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
        } else if (Input.GetButtonDown("ToggleRot"))
        {
            ToggleRotDirection();
        } else if(Input.GetButtonDown("Pause"))
        {
            TogglePause();
        } else if(Input.GetButtonDown("Hold"))
        {
            Hold();
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
        // land shape fx
        m_activeShape.LandShapeFX();
        m_activeShape = m_spawner.ShapeSpawner();

        // remove compoeted rows
        // m_gameBoard.ClearAllRows();
        m_gameBoard.StartCoroutine("ClearAllRows");

        // reset ghost
        if(m_ghost)
        {
            m_ghost.Reset();
        }
        // update can hold
        if(m_holder)
        {
            m_holder.m_canRelease = true;

        }
        // add landing sound effects
        PlaySound(m_soundManager.m_dropSound, 0.75f);

        if(m_gameBoard.m_completedRows > 0)
        {
            m_scoreManager.ScoreLines(m_gameBoard.m_completedRows); // 当前这一块落下以后，消除的块数
            if(m_gameBoard.m_completedRows > 1)
            {
                AudioClip randomVocal = m_soundManager.GetRandomClip(m_soundManager.m_vocalClips);
                PlaySound(randomVocal);
            }

            PlaySound(m_soundManager.m_clearRowSound, 1.0f);
        }
    }

    public void Restart()
    {
        Debug.Log("Restarted");
        // set time scale to 1 to solve a bug
        Time.timeScale = 1f;
        Application.LoadLevel(Application.loadedLevel);

    }

    void GameOver()
    {
        m_gameOver = true;
        m_activeShape.MoveUp();
        Debug.LogWarning(m_activeShape.name + " is over the limit!");
        //if (m_gameOverPanel)
        //{
        //    m_gameOverPanel.SetActive(true);
        //}
        //if(m_gameOverFX)
        //{
        //    m_gameOverFX.Play();
        //}

        StartCoroutine(GameOverRoutine());

        PlaySound(m_soundManager.m_gameOverVocalClip, 5f);
        PlaySound(m_soundManager.m_gameOverSound, 2.0f);

    }
    
    void PlaySound(AudioClip clip, float volMultiplier=1.0f)
    {
        if(clip && m_soundManager.m_fxEnabled)
        {
            AudioSource.PlayClipAtPoint(clip, Camera.main.transform.position, Mathf.Clamp(m_soundManager.m_fxVolume * volMultiplier, 0.05f, 1.0f));

        }
    }

    public void ToggleRotDirection()
    {
        m_clockwise = !m_clockwise;
        if(m_rotIconToggle)
        {
            // this only affects ui, real function is handled elsewhere
            m_rotIconToggle.ToggleIcon(m_clockwise); // toggle direction

        }
    }

    public void TogglePause()
    {
        if(m_gameOver)
        {
            return;
        }
        m_isPaused = !m_isPaused;
        
        // toggle pause panel
        if(m_pausePanel) 
        {
            m_pausePanel.SetActive(m_isPaused);
            if(m_soundManager)
            {
                m_soundManager.m_musicSource.volume = (m_isPaused) ? m_soundManager.m_musicVolume * 0.25f : m_soundManager.m_musicVolume;

            }

            Time.timeScale = (m_isPaused) ? 0 : 1;
             
        }
    }

    public void Hold()
    {
        if(!m_holder)
        {
            return;  
        }
        
        if(!m_holder.m_heldShape)
        {
            m_holder.Catch(m_activeShape);
            m_activeShape = m_spawner.ShapeSpawner();// is holder not empty skip
            PlaySound(m_soundManager.m_holdSound);

        }
        else if(m_holder.m_canRelease)
        {
            Shape shape = m_activeShape;
            m_activeShape = m_holder.Release();
            m_activeShape.transform.position = m_spawner.transform.position;
            m_holder.Catch(shape);
            PlaySound(m_soundManager.m_holdSound);

        }
        else
        {
            Debug.LogWarning("wait for cool down");
            PlaySound(m_soundManager.m_errorSound);

        }

        if (m_ghost)
        {
            m_ghost.Reset(); // update ghost .. 
        }
    }

    // coroutine to delay the game over panel
    IEnumerator GameOverRoutine()
    {
        if(m_gameOverFX)
        {
            m_gameOverFX.Play();
        }
        yield return new WaitForSeconds(0.3f);
        
        if(m_gameOverPanel)
        {
            m_gameOverPanel.SetActive(true);
        }
    }
}

 