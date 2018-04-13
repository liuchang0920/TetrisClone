using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 *  1. Keep libary of all shapes
 *  2. spawn shape upon game controller request
 * */
public class Spawner : MonoBehaviour {

    public Shape[] m_allShapes;
    public Transform[] m_queuedXForms = new Transform[3];
    Shape[] m_queuedShapes = new Shape[3];

    float m_queueScale = 0.5f;

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
        // shape = Instantiate(GetRandomShape(), transform.position, Quaternion.identity) as Shape;
        shape = GetQueuedShape();
        Debug.LogWarning("getting shape info: ");
        Debug.LogWarning(shape.ToString());
        shape.transform.position = transform.position;
        // scale to 1 so that on the board shows correctly
        shape.transform.localScale = Vector3.one;

        if (shape) {
            return shape;
        } else {
            Debug.Log("warning, Invalid shape");
            return null;  
        }
    }

    // awke starts before start, where queue shape init properly
    private void Awake()
    {
        InitQueue();
    }
    // Use this for initialization
    void Start () {
        // init queue
        

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void InitQueue()
    {
        for(int i=0;i<m_queuedShapes.Length;i++)
        {
            m_queuedShapes[i] = null;
        }
        FillQueue();
    }

    void FillQueue()
    {
        for(int i=0;i<m_queuedShapes.Length; i++)
        {
            if(!m_queuedShapes[i])
            {
                m_queuedShapes[i] = Instantiate(GetRandomShape(), transform.position, Quaternion.identity) as Shape;
                m_queuedShapes[i].transform.position = m_queuedXForms[i].position + m_queuedShapes[i].m_queueOffset;
                // scale
                m_queuedShapes[i].transform.localScale = new Vector3(m_queueScale, m_queueScale, m_queueScale);
            }
        }
    }

    Shape GetQueuedShape()
    {
        Shape firstShape = null;
        if(m_queuedShapes[0])
        {
            firstShape = m_queuedShapes[0];
        }
        for(int i=1;i<m_queuedShapes.Length; i++)
        {
            m_queuedShapes[i - 1] = m_queuedShapes[i];
            m_queuedShapes[i - 1].transform.position = m_queuedXForms[i - 1].position + m_queuedShapes[i].m_queueOffset;
        }

        m_queuedShapes[m_queuedShapes.Length - 1] = null;
        FillQueue();

        return firstShape;
    }
}
