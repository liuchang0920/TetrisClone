using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour {

    public Transform m_emptySprite;
    public int m_height = 30;
    public int m_width = 10;
    public int m_header = 8;

    public int m_compoletedRows = 0;

    Transform[,] m_grid;

    void Awake() // prestart
    {
        m_grid = new Transform[m_width, m_height];
    }
	// Use this for initialization
	void Start () {
        DrawEmptyCells();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    bool IsWitinBoard(int x, int y) {
        return (x >= 0 && x < m_width && y >= 0);
    }

    public bool IsValidPosition(Shape shape) {
        foreach(Transform child in shape.transform) {
            Vector2 pos = Vectorf.Round(child.position);
            if(!IsWitinBoard((int)pos.x, (int)pos.y)) { // float to int cast here
                return false;
            }
            if(IsOccupied((int)pos.x, (int)pos.y, shape)) {
                return false;
            }
        }
        return true;
    }

    void DrawEmptyCells() {
        if(m_emptySprite != null)
        {
            for (int y = 0; y < m_height - m_header; y++)
            {
                for (int x = 0; x < m_width; x++)
                {
                    Transform clone;
                    clone = Instantiate(m_emptySprite, new Vector3(x, y, 0), Quaternion.identity) as Transform; //  Quaternion.identity rotation zero...
                    clone.name = "Board Space: x=" + x.ToString() + ", y=" + y.ToString();
                    clone.transform.parent = transform;
                }
            }
        } else
        {
            Debug.Log("init sprite");

        }
    }

    public void StoreShapeInGrid(Shape shape) {
        if (shape == null) return;
        foreach(Transform child in shape.transform) {
            Vector2 pos = Vectorf.Round(child.position);
            m_grid[(int)pos.x, (int)pos.y] = child;
        }
    }

    bool IsOccupied(int x, int y, Shape shape) {
        return (m_grid[x, y] != null && m_grid[x, y].parent != shape.transform);// differet object    
    }
    
    bool IsCompleted(int y) {
        for(int x=0;x<m_width;x++)
        {
            if(m_grid[x, y] == null)
            {
                return false;
            }
        }
        return true;
    }
    
    void ClearRow(int y) {
        for(int x=0;x<m_width;x++)
        {
            if(m_grid[x, y] != null) {
                Destroy(m_grid[x, y].gameObject);
            }
            m_grid[x, y] = null;
        }
    }

    void ShiftOneRowDown(int y)
    {
        for(int x=0;x<m_width;x++)
        {
            if(m_grid[x, y] != null)
            {
                m_grid[x, y - 1] = m_grid[x, y];
                m_grid[x, y] = null;
                m_grid[x, y - 1].position += new Vector3(0, -1, 0); //move physical space,  move position ??
            }
        }
    }

    void ShiftRowsDown(int startY) 
    {
        for(int i=startY;i<m_height;i++)
        {
            ShiftOneRowDown(i);
        }
    }

    public void ClearAllRows()
    {
        m_compoletedRows = 0;
        for (int y=0;y<m_height; y++)
        {
            if(IsCompleted(y))
            {
                m_compoletedRows++; 
                ClearRow(y);
                ShiftRowsDown(y + 1);
                y--; // after shifted, retest current row
            }
        }
    }

    public bool IsOverLimit(Shape shape)
    {
        foreach(Transform child in shape.transform)
        {
            if(child.transform.position.y >= (m_height - m_header -1))
            {
                return true;
            }
        }
        return false;
    }
}
