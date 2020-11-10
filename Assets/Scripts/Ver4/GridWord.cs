using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridWord : MonoBehaviour
{
    public Text Answer;

    [HideInInspector]
    public bool m_IsDouble;

    private GameObject Mgr;
    private string m_Answer;
    private bool m_bCorrect = false;
    public bool IsCorrect { get { return m_bCorrect; } set { m_bCorrect = value; } }
    private int m_Index;

    void Start()
    {
        try
        {
            m_IsDouble = false;
            Answer.text = "";
            Mgr = GameObject.Find("InputMgr");
            
            Answer.text = this.gameObject.name;
        }
        catch (System.Exception ex)
        {
            Debug.LogWarning(ex.Message);
        }
    }

    public void ClickButton()
    {
        Mgr.GetComponent<InputMgr_2>().QuestionClick(m_Answer);
        m_IsDouble = true;
    }

    public void Show()
    {
        Answer.text = m_Answer[m_Index].ToString();
    }

    public void Input(string _input)
    {
        Answer.text = _input;
    }

    public string GetInput()
    {
        return Answer.text;
    }

    public void Wrong()
    {
        Answer.text = "";
    }

    public void SetUp(string _word, int index)
    {
        m_Answer = _word;
        m_Index = index;
    }
}
