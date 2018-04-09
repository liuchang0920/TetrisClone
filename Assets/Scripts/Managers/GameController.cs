using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

    Board m_gameBoard;
    Spawner m_spawner; // try to reference other game objects

	// Use this for initialization
	void Start () {
        m_gameBoard = GameObject.FindWithTag("Board").GetComponent<Board>();
        m_spawner = GameObject.FindWithTag("Spawner").GetComponent<Spawner>();

        if(m_spawner){
            m_spawner.transform.position = Vectorf.Round(m_spawner.transform.position);

        }

        if(!m_gameBoard) {
            Debug.LogWarning("There is no game board defined!");
        }
        if(!m_spawner) {
            Debug.LogWarning("Warning there is no spawner defined!");
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
