using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/**
 * 
 * keep record of
 * 1. score
 * 2. level
 * 3. lines until next level
 **/
public class ScoreManager : MonoBehaviour {

    int m_score = 0;
    int m_lines = 0;
    int m_level = 1;

    public int m_linesPerLevel = 5;

    public Text m_linesText;
    public Text m_levelText;
    public Text m_scoreText;

    const int m_minLines = 1;
    const int m_maxLines = 4;

    public void ScoreLines(int n)
    {
        n = Mathf.Clamp(n, m_minLines, m_maxLines);
        switch(n)
        {
            case 1:
                m_score += 40 * m_level;
                break;
            case 2:
                m_score += 100 * m_level;
                break;
            case 3:
                m_score += 300 * m_level;
                break;
            case 4:
                m_score += 1200 * m_level;
                break;
        }
        UpdateUIText();
    }

    public void Reset() {

        m_level = 1;
        m_lines = m_lines * m_level;
    }

    void UpdateUIText()
    {
        if(m_linesText)
        {
            m_linesText.text = m_lines.ToString();
        }
        if(m_levelText)
        {
            m_levelText.text = m_level.ToString();
        }
        if(m_scoreText)
        {
            m_scoreText.text = PadZero(m_score, 5);
        }
    }

    string PadZero(int n, int padDigits)
    {
        string nStr = n.ToString();
        while(nStr.Length<padDigits)
        {
            nStr = "0" + nStr;
        }
        return nStr;
    }
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
