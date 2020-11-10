using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputMgr_2 : MonoBehaviour
{
    [Header("Veiwer")]
    public GameObject RectScroll;
    [Header("Effect Image")]
    public Image TimeOver;
    public Image Congratulations;
    public Image Correct;
    public Image Wrong;
    [Header("Text UI")]
    public Text CurrentHint;
    public Text QuestionCount;
    public Text Ready;
    public Text Start;
    [Header("Timer")]
    public Text TimerText;
    public float MaxTime;
    [Header("Count")]
    public int MaxCount;
    [HideInInspector]
    public bool Assistant;
    [Header("Effect Time")]
    public Text WinTime;
    public Text OverCount;

    private float m_CurrentTime;

    private string m_Difficult;
    private int m_Score = -1;
    public int GetScore(string difficult)
    {
        if (PlayerPrefs.HasKey(difficult))
        {
            return PlayerPrefs.GetInt(difficult);
        }
        else
        {
            return -1;
        }
    }

    [HideInInspector]
    public bool m_Power;
    public void PowerOn()
    {
        m_Power = true;
    }
    public void PowerOff()
    {
        Start.color = new Color(0.196f, 0.196f, 0.196f, 1);
        Ready.GetComponent<Outline>().effectDistance = new Vector2(0, 0);
        Ready.gameObject.SetActive(true);
        Start.gameObject.SetActive(false);
        m_WordList = new List<Word>();
        m_DoneList = new List<Word>();
        m_InputList = new List<string>();
        m_InputIndexList = new List<int>();
        m_IsClicked = false;
        m_CurrentTime = 0;
        m_IsStart = false;
        m_IsList = false;
        m_IsEnd = false;
        m_IsInput = false;
        Correct.gameObject.SetActive(false);
        Wrong.gameObject.SetActive(false);
        m_IsCheck = false;
        Correct.color += new Color(0, 0, 0, 255);
        Wrong.color += new Color(0, 0, 0, 255);

        if (m_ObjectList != null)
        {
            for (int i = 0; i < m_ObjectList.Length; i++)
            {
                if (m_ObjectList[i] != null)
                {
                    m_ObjectList[i].GetComponent<Outline>().enabled = false;
                }
            }
        }

        m_Power = false;
    }

    private bool m_IsStart;
    private bool m_IsList;
    private bool m_IsEnd;
    public bool End { get { return m_IsEnd; } set { m_IsEnd = value; } }
    private bool m_IsClicked;
    private bool m_IsInput;
    private bool m_IsCheck;
    // 정답
    private Word m_Answer;
    // 사용자 입력
    private List<string> m_InputList = new List<string>();
    private List<int> m_InputIndexList = new List<int>();
    private int CurrentIndex = 0;
    private int m_MaxCount = 10;
    private GameObject TempInput;
    private GameObject GridManager;
    private List<Word> m_WordList;
    private List<Word> m_DoneList;
    private GameObject[] m_ObjectList;

    private void Awake()
    {
        Assistant = true;
        m_IsEnd = false;
        m_IsList = false;
        m_IsStart = false;
        m_IsCheck = false;
        m_IsInput = false;
        m_IsClicked = false;
        m_WordList = new List<Word>();
        m_DoneList = new List<Word>();
    }

    GameObject CheckCross()
    {
        bool vertical;
        if (TempInput.tag == "Vertical")
        {
            vertical = true;
        }
        else
        {
            vertical = false;
        }
        var temp = CheckDouble(TempInput.transform.localPosition, vertical);

        return temp;
    }

    public void KeyInput(string _input)
    {
        if (m_IsClicked)
        {

            if (_input == "BK_SPACE")
            {
                if (m_InputIndexList[CurrentIndex] <= 0)
                {
                    m_IsInput = false;
                    return;
                }

                TempInput = GameObject.Find(m_Answer.Answer + " " + (m_InputIndexList[CurrentIndex] - 1));
                var temp1 = CheckCross();

                if (temp1 != null)
                { // 겹치는 부분이 존재함
                    if (temp1.GetComponent<InputWord>().IsCorrect)
                    { // 겹치는 단어가 정답일때
                        string var_string = temp1.GetComponent<InputWord>().GetInput();
                        m_InputList[CurrentIndex] = m_InputList[CurrentIndex].Remove(m_InputList[CurrentIndex].Length - 1);
                        Debug.Log(_input);
                        m_InputIndexList[CurrentIndex]--;
                        return;
                    }
                    m_InputList[CurrentIndex] = m_InputList[CurrentIndex].Remove(m_InputList[CurrentIndex].Length - 1);
                    Debug.Log(_input);
                    TempInput.GetComponent<InputWord>().Wrong();
                    temp1.GetComponent<InputWord>().Wrong();
                    m_InputIndexList[CurrentIndex]--;
                    return;
                }

                m_InputList[CurrentIndex] = m_InputList[CurrentIndex].Remove(m_InputList[CurrentIndex].Length - 1);
                Debug.Log(_input);
                TempInput.GetComponent<InputWord>().Wrong();
                m_InputIndexList[CurrentIndex]--;
                return;
            }

            if (m_InputIndexList[CurrentIndex] >= m_Answer.Answer.Length)
            {
                return;
            }
            TempInput = GameObject.Find(m_Answer.Answer + " " + m_InputIndexList[CurrentIndex]);

            var temp = CheckCross();

            if (temp != null)
            { // 겹치는 부분이 존재함
                if (temp.GetComponent<InputWord>().IsCorrect)
                { // 겹치는 단어가 정답일때
                    string var_string = temp.GetComponent<InputWord>().GetInput();
                    Debug.Log(var_string);
                    TempInput.GetComponent<InputWord>().Input(var_string);
                    temp.GetComponent<InputWord>().Input(var_string);
                    m_InputIndexList[CurrentIndex]++;
                    m_IsInput = true;
                    m_InputList[CurrentIndex] += string.Join("", var_string);
                    return;
                }
                Debug.Log(_input);
                TempInput.GetComponent<InputWord>().Input(_input);
                temp.GetComponent<InputWord>().Input(_input);
                m_InputIndexList[CurrentIndex]++;
                m_IsInput = true;
                m_InputList[CurrentIndex] += string.Join("", _input);
                return;
            }

            Debug.Log(_input);
            TempInput.GetComponent<InputWord>().Input(_input);
            m_InputIndexList[CurrentIndex]++;
            m_IsInput = true;
            m_InputList[CurrentIndex] += string.Join("", _input);
            return;
        }
    }

    GameObject CheckDouble(Vector3 Pos, bool Vertical)
    {
        if (Vertical)
        {
            var temp = GameObject.FindGameObjectsWithTag("Horizontal");
            for (int i = 0; i < temp.Length; i++)
            {
                if (temp[i].transform.localPosition == Pos)
                {
                    var obj = temp[i];
                    return obj;
                }
            }
        }
        else
        {
            var temp = GameObject.FindGameObjectsWithTag("Vertical");
            for (int i = 0; i < temp.Length; i++)
            {
                if (temp[i].transform.localPosition == Pos)
                {
                    var obj = temp[i];
                    return obj;
                }
            }
        }

        return null;
    }
    void DiffuseEffect()
    {
        if (Correct.IsActive())
        { // 정답 이펙트
            while (Correct.color.a > 0f)
            {
                Correct.color -= new Color(0, 0, 0, 0.04f * Time.deltaTime);
            }
            m_IsCheck = false;

            RESET();

            // 문제를 맞혔을때만 다음으로 넘어가거나 뒤로 돌아가게 해줘야함
            if (Assistant)
            {
                Next(true);
            }
        }
        else
        { // 오답 이펙트
            while (Wrong.color.a > 0f)
            {
                Wrong.color -= new Color(0, 0, 0, 0.04f * Time.deltaTime);
            }
            m_IsCheck = false;
            RESET();

            // 문제를 틀렸을때는 다시 해당 문제로 가게 해줘야함
            m_IsClicked = true;
            HighLight();
        }
        return;
    }

    private void FixedUpdate()
    {
        if (m_Power)
        {
            if (!m_IsStart)
            {
                Ready.gameObject.SetActive(false);
                Start.gameObject.SetActive(true);
                m_IsStart = true;
            }
            else if (!m_IsEnd)
            {
                Timer();
                ////////////////////
                if (Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    Prev();
                }
                if (Input.GetKeyDown(KeyCode.RightArrow))
                {
                    Next();
                }
            }
            if (Start.color.a <= 0)
            {
                Start.gameObject.SetActive(false);
            }
            Start.color -= new Color(0, 0, 0, 1f) * Time.deltaTime;

            if (m_IsCheck)
            {
                DiffuseEffect();
            }
        }
    }

    void Timer()
    {
        if (m_DoneList.Count >= m_MaxCount)
        { // 모든 정답을 맞춘 상황
            Debug.Log("게임 종료");
            m_Score = (int)(m_CurrentTime);
            m_IsEnd = true;
            Congratulations.gameObject.SetActive(true);
            WinTime.text = ((int)m_CurrentTime).ToString() + " 초";
            return;
        }
        if (m_CurrentTime >= MaxTime)
        { // 시간 종료
            Debug.Log("시간 종료");
            m_IsEnd = true;
            // 게임 시작 화면으로
            TimeOver.gameObject.SetActive(true);
            OverCount.text = m_DoneList.Count.ToString() + " 개";
            return;
        }
        m_CurrentTime += Time.deltaTime;
        TimerText.text = ((int)(MaxTime - m_CurrentTime)).ToString();
    }

    private void Update()
    {
        if (m_Power)
        {
            // 시작했을때 한번만
            if (!m_IsList)
            {
                GridManager = GameObject.Find("GridInfoMgr");
                m_IsList = true;
            }

            if(m_WordList.Count <= 0 && GridManager.GetComponent<GridMgr>().IsMake)
            {
                var temp = GridManager.GetComponent<GridMgr>().GetMainList();
                for (int i = 0; i < temp.Count; i++)
                {
                    m_WordList.Add(temp[i]);
                    m_InputList.Add("");
                    m_InputIndexList.Add(0);
                }
                m_MaxCount = m_WordList.Count;
            }

            QuestionCount.text = (m_DoneList.Count + " / " + m_MaxCount);

            // 표를 눌렀을 때
            if (!m_IsEnd)
            {
                if (m_IsClicked)
                {
                    if (m_IsInput && m_InputList[CurrentIndex].Length == m_Answer.Answer.Length)
                    {
                        if (m_InputList[CurrentIndex] == m_Answer.Answer)
                        {
                            // 정답
                            Correct.gameObject.SetActive(true);
                            SetCorrect();
                            m_DoneList.Add(m_Answer);
                            int index = FindWord(m_Answer);
                            Word TempWord = new Word();
                            if (index + 1 != m_WordList.Count)
                            {
                                TempWord = m_WordList[index];
                                if (Assistant)
                                {
                                    m_Answer = TempWord;
                                    index = FindWord(m_Answer);
                                    CurrentIndex = index;
                                }
                            }
                            m_WordList.Remove(m_Answer);
                            m_InputList.Remove(m_Answer.Answer);
                            m_InputIndexList.Remove(m_Answer.Length);
                            m_IsCheck = true;
                        }
                        else
                        {
                            // 오답
                            Wrong.gameObject.SetActive(true);
                            m_IsCheck = true;
                            for (int i = 0; i < m_Answer.Answer.Length; i++)
                            {
                                m_InputIndexList[CurrentIndex] = 0;
                                m_InputList[CurrentIndex] = "";
                                TempInput = GameObject.Find(m_Answer.Answer + " " + i);
                                TempInput.GetComponent<InputWord>().Wrong();
                                var temp = CheckCross();
                                if (temp != null)
                                {
                                    if (temp.GetComponent<InputWord>().IsCorrect)
                                    {
                                        TempInput.GetComponent<InputWord>().Input(temp.GetComponent<InputWord>().GetInput());
                                    }
                                    else
                                    {
                                        temp.GetComponent<InputWord>().Wrong();
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    public void Next(bool auto = false)
    {
        if (auto)
        {
            RESET();
            CurrentHint.text = m_Answer.Meaning;
        }
        else
        {
            RESET();
            int index = FindWord(m_Answer);
            if (index <= m_WordList.Count - 1)
            {
                if (index != m_WordList.Count - 1)
                {
                    m_Answer = m_WordList[index + 1];
                    index = FindWord(m_Answer);
                    CurrentHint.text = m_Answer.Meaning;
                    CurrentIndex = index;
                }
            }
            else
            {
                return;
            }
        }
        HighLight();
        if (Assistant)
        {
            var temp = GameObject.Find(m_Answer.Answer + " 0");
            Vector3 Pos = new Vector3(-temp.transform.localPosition.y, temp.transform.localPosition.x, 0);
            RectScroll.transform.localPosition = Pos;
        }
        m_IsClicked = true;
    }

    public void Prev()
    {
        RESET();
        int index = FindWord(m_Answer);
        if (index >= 0)
        {
            if (index != 0)
            {
                m_Answer = m_WordList[index - 1];
                index = FindWord(m_Answer);
                CurrentHint.text = m_Answer.Meaning;
                CurrentIndex = index;
            }
        }
        else
        {
            return;
        }
        HighLight();
        if (Assistant)
        {
            var temp = GameObject.Find(m_Answer.Answer + " 0");
            Vector3 Pos = new Vector3(-temp.transform.localPosition.y, temp.transform.localPosition.x, 0);
            RectScroll.transform.localPosition = Pos;
        }
        m_IsClicked = true;
    }

    public void QuestionClick(string _answer)
    {
        RESET();
        for (int i = 0; i < m_DoneList.Count; i++)
        {
            if (m_DoneList[i].Answer == _answer)
            {
                return;
            }
        }
        for (int i = 0; i < m_WordList.Count; i++)
        {
            if (m_WordList[i].Answer == _answer)
            { // 정답 정보 입력
                m_Answer = m_WordList[i];
                CurrentIndex = i;
            }
        }
        Debug.Log(m_Answer.Answer);
        HighLight();
        CurrentHint.text = m_Answer.Meaning;
        m_IsClicked = true;
    }

    void HighLight()
    { // 클릭시 하이라이팅 처리
        m_ObjectList = new GameObject[m_Answer.Length];
        for (int i = 0; i < m_Answer.Length; i++)
        {
            m_ObjectList[i] = GameObject.Find(m_Answer.Answer + " " + i);
            m_ObjectList[i].GetComponent<Outline>().enabled = true;
        }
    }

    void SetCorrect()
    {
        for (int i = 0; i < m_Answer.Length; i++)
        {
            var temp = GameObject.Find(m_Answer.Answer + " " + i);
            temp.GetComponent<InputWord>().IsCorrect = true;
        }
    }

    void RESET()
    {
        if (m_ObjectList != null)
        {
            for (int i = 0; i < m_ObjectList.Length; i++)
            {
                if (m_ObjectList[i] != null)
                {
                    m_ObjectList[i].GetComponent<Outline>().enabled = false;
                }
            }
        }
        m_IsClicked = false;
        m_IsInput = false;
        Correct.gameObject.SetActive(false);
        Wrong.gameObject.SetActive(false);
        m_IsCheck = false;
        Correct.color += new Color(0, 0, 0, 255);
        Wrong.color += new Color(0, 0, 0, 255);
    }

    int FindWord(Word word)
    {
        for (int i = 0; i < m_WordList.Count; i++)
        {
            if (m_WordList[i] == word)
            {
                return i;
            }
        }

        return -1;
    }

    public void Pause(bool power)
    {
        m_Power = power;
    }
}
