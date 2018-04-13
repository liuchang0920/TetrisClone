using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Holder : MonoBehaviour {

    public Transform m_holderXForm;
    public Shape m_heldShape = null;
    float m_scale = 0.5f;
    public bool m_canRelease = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Catch(Shape shape)
    {
        if (!shape)
        {
            Debug.LogWarning("HOLDER WARNING! " + shape.name + " is invalid!");
            return;
        }

        if (!m_holderXForm)
        {
            Debug.LogWarning("HOLDER WARNING! Missing Holder transform!");
            return;
        }

        if (m_heldShape)
        {
            Debug.LogWarning("HOLDER WARNING!  Release a shape before trying to hold.");
            return;
        }
        else
        {
            shape.transform.position = m_holderXForm.position + shape.m_queueOffset;
            shape.transform.localScale = new Vector3(m_scale, m_scale, m_scale);
            m_heldShape = shape;
        }
    }

    public Shape Release()
    {
       if(m_heldShape)
        {
            m_heldShape.transform.localScale = Vector3.one;
            Shape shape = m_heldShape;
            m_heldShape = null;
            m_canRelease = false;

            return shape;
        }
        Debug.LogWarning("holder contains no shape!");
        return null;
    }

}
