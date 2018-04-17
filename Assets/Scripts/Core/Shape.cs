using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shape : MonoBehaviour {

    public bool m_canRotate = true;
    public Vector3 m_queueOffset;

    GameObject[] m_glowSquareFX;
    public string glowSquareTag = "LandShapeFX";

    void Move(Vector3 moveDirection) {
        transform.position += moveDirection;
    }

    public void MoveLeft() {
        Move(new Vector3(-1, 0, 0));
    }

    public void MoveRight()
    {
        Move(new Vector3(1, 0, 0));
    }

    public void MoveDown()
    {
        Move(new Vector3(0, -1, 0));
    }

    public void MoveUp()
    {
        Move(new Vector3(0, 1, 0));
    }

    public void RotateRight() {
        if(m_canRotate) {
            transform.Rotate(0, 0, -90);
        }    
    }

    public void RotateLeft() {
        if(m_canRotate) {
            transform.Rotate(0, 0, 90);
        }
    }

	// Use this for initialization
	void Start () {
        // InvokeRepeating("MoveDown", 0, 0.5f);
        // InvokeRepeating("RotateRight", 0, 0.5f);
        
        //  init fx gameobject
        if(glowSquareTag != "")
        {
            m_glowSquareFX = GameObject.FindGameObjectsWithTag(glowSquareTag);
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void RotateClockwise(bool clockwise)
    {
        if(clockwise)
        {
            RotateRight();
        } else
        {
            RotateLeft();
        }
    }

    public void LandShapeFX()
    {
        int i = 0;
        foreach(Transform child in gameObject.transform)
        {
            if(m_glowSquareFX[i])
            {
                m_glowSquareFX[i].transform.position = new Vector3(child.position.x, child.position.y, -2f);
                ParticlePlayer particlePlayer = m_glowSquareFX[i].GetComponent<ParticlePlayer>();

                if(particlePlayer)
                {
                    particlePlayer.Play();
                }
            }
            i++;
        }
    }
}
