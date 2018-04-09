using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 *  1. Keep libary of all shapes
 *  2. spawn shape upon game controller request
 * */
public class Spawner : MonoBehaviour {

    public Shape[] m_allShapes;

    Shape GetRandomShape() {
        int i = Random.Range(0, m_allShapes.Length);
        if (m_allShapes[i]) {
            return m_allShapes[i];
        } else {
            Debug.Log("warning, Invalid shape");
            return null;
        }
    }

    public Shape ShapeSpawner() {
        Shape shape = null;
        shape = Instantiate(GetRandomShape(), transform.position, Quaternion.identity) as Shape;
        if (shape) {
            return shape;
        } else {
            Debug.Log("warning, Invalid shape");
            return null;  
        }
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
