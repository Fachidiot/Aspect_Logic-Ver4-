﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WordMgr : MonoBehaviour
{
    [Header("GridMake StartPosition")]
    public float StartX;
    public float StartY;
    [Header("GridSize Max Size")]
    public int MinX = -1030;
    public int MinY = -1030;
    public int MaxX = 1030;
    public int MaxY = 1030;
    [Header("Prefab")]
    public GameObject GridPrefab;
    [Header("Grid Scaler")]
    public GameObject Grid;


    private int m_WordCount;

    // 생성될 위치
    private float MakeX;
    private float MakeY;

    // Rect 크기 조절
    private Vector2 RectMax;
    private Vector2 RectMin;

    private List<GameObject> m_GridMakeList;
    private List<Word> m_WordList;
    // 만들어진 단어 목록
    private List<Word> m_MakeList;
    public Word[] GetList()
    {
        var temp = m_MakeList.ToArray();
        return temp;
    }
    // 지금 만들어줄 단어
    private Word m_MakeWord;
    // 다음에 만들어줄 단어
    private Word m_TempWord;
    private bool m_bIsMake;
    public bool IsMake { get { return m_bIsMake; } set { m_bIsMake = value; } }
    // 더이상 단어를 못만들어줄때
    private bool m_bMakeAble;
    private bool m_bVertical;
    private int m_Reset;
    private int m_CurrentWordCount;
    private int safeloop;
    private int m_RootSet;

    private void Awake()
    {
        m_RootSet = 0;
        MakeX = StartX;
        MakeY = StartY;
        m_Reset = 0;
        safeloop = 0;
        m_CurrentWordCount = 0;
        m_bIsMake = false;
        m_bMakeAble = true;
        m_bVertical = true;
        m_GridMakeList = new List<GameObject>();
        m_MakeList = new List<Word>();
        m_WordList = new List<Word>();
    }

    private void Update()
    {
        if (m_WordList.Count <= 0)
        {
            m_WordList = GameObject.Find("CrossMgr").GetComponent<CrossWord>().GetList();
        }

        if (!m_bIsMake && m_WordList.Count > 0)
            Logic();

        if(IsMake)
        {
            Grid.GetComponent<RectTransform>().sizeDelta = new Vector2(110 + RectMax.x - RectMin.x, 110 + RectMax.y - RectMin.y);
            float tempx = (RectMax.x + RectMin.x) / 2;
            float tempy = (RectMax.y + RectMin.y) / 2;
            gameObject.transform.localPosition = new Vector3(-tempx, -tempy, 0);
        }
    }

    void Logic()
    {
        // 비상 탈출문
        if (safeloop > 200)
        {
            Debug.Log("더이상 만들수 있는 단어가 존재하지 않습니다.");
            safeloop = 0;
            m_bMakeAble = false;
            //m_bIsMake = true;
            return;
        }
        // 안전 탈출문
        if (m_WordCount <= m_CurrentWordCount)
        {
            Debug.Log("모든 단어가 완성 되었습니다!");
            Debug.Log(RectMin.x + ", " + RectMax.x);
            Debug.Log(RectMin.y + ", " + RectMax.y);
            m_bIsMake = true;
            return;
        }

        // 일단 루트먼저 만들어준다.
        // 그 다음은 역시 크로싱을 만들어 준다.
        // 계속해서 만들어준다.
        // 한번이라도 막힌다면 첨부터 다시 크로싱 해준다.

        if (m_GridMakeList.Count <= 0 || !m_bMakeAble)
        {
            // 만들어진 리스트가 없다.(처음 생성) or 더이상 만들어줄 단어가 없다.
            // 단어 생성
            RootSet();
        }

        if (m_bMakeAble)
        {
            safeloop++;

            if (!MakeRootCrossWord())
            {
                if (m_Reset < m_MakeList.Count)
                {
                    var temp = GameObject.Find(m_MakeList[m_Reset].Answer + " " + 0.ToString());
                    MakeX = temp.transform.localPosition.x;
                    MakeY = temp.transform.localPosition.y;
                    m_MakeWord = m_MakeList[m_Reset];
                    m_Reset++;
                    return;
                }
                // 모든 단어를 순회 했을때
                m_Reset = 0;
                //m_bMakeAble = false;
            }
        }
    }

    void UseWord(Word _word)
    {
        //Debug.Log(_word.Answer + " 단어 삭제");
        m_WordList.Remove(_word);
    }
    static int count = 0;
    int CheckWord(int index)
    {
        if (count > 10)
        {
            return -1;
        }
        // 랜덤으로 단어를 갖고 온다.
        int temp = Random.Range(0, m_TempWord.IndexInfo[index].Count);

        string val = m_TempWord.IndexInfo[index][temp];
        // 0 : Word, 1 : index1, 2 : index2
        var temp_string = val.Split(',');
        if (FindWord(temp_string[0]) == null)
        {
            count++;
            temp = CheckWord(index);
        }

        return temp;
    }

    // 단어 생성
    void HorizontalMake()
    {
        m_MakeList.Add(m_MakeWord);
        for (int i = 0; i < m_MakeWord.Length; i++)
        {
            var temp = Instantiate(GridPrefab, this.gameObject.transform);
            m_GridMakeList.Add(temp);
            temp.transform.localPosition = new Vector3(MakeX, MakeY + 110 * i, 0);
            if (RectMin.x > MakeX)
            {
                RectMin.x = MakeX;
            }
            if (RectMin.y > MakeY)
            {
                RectMin.y = MakeY;
            }
            if(RectMax.x < MakeX)
            {
                RectMax.x = MakeX;
            }
            if(RectMax.y < MakeY + 110 * i)
            {
                RectMax.y = MakeY + 110 * i;
            }
            temp.name = m_MakeWord.Answer + " " + i;
            temp.tag = "Horizontal";
        }

        m_CurrentWordCount++;
    }
    void VerticalMake()
    {
        m_MakeList.Add(m_MakeWord);
        for (int i = 0; i < m_MakeWord.Length; i++)
        {
            var temp = Instantiate(GridPrefab, this.gameObject.transform);
            m_GridMakeList.Add(temp);
            temp.transform.localPosition = new Vector3(MakeX + 110 * i, MakeY, 0);
            if (RectMin.x > MakeX)
            {
                RectMin.x = MakeX;
            }
            if (RectMin.y > MakeY)
            {
                RectMin.y = MakeY;
            }
            if (RectMax.x < MakeX + 110 * i)
            {
                RectMax.x = MakeX + 110 * i;
            }
            if (RectMax.y < MakeY)
            {
                RectMax.y = MakeY;
            }
            temp.name = m_MakeWord.Answer + " " + i;
            temp.tag = "Vertical";
        }

        m_CurrentWordCount++;
    }

    // 단어가 겹치는지 체크
    void SetRootPos()
    {
        // 생성전에 생성될 위치에 자리가 있는지 확인
        var templist = new List<GameObject>();
        templist.AddRange(GameObject.FindGameObjectsWithTag("Vertical"));
        templist.AddRange(GameObject.FindGameObjectsWithTag("Horizontal"));

#pragma warning disable CS0162 // 접근할 수 없는 코드가 있습니다.
        for (int index = 0; index < templist.Count; index++)
        {
            // 생성된 Grid를 가져온다.
            QuaterSection Section = new QuaterSection();

            for (int j = 0; j < templist.Count; j++)
            {
                // 1분면 ( - , + )
                if (templist[j].transform.localPosition.x < 0 && templist[j].transform.localPosition.y > 0)
                {
                    Section.Section1++;
                }

                // 2분면 ( - , - )
                if (templist[j].transform.localPosition.x < 0 && templist[j].transform.localPosition.y < 0)
                {
                    Section.Section2++;
                }

                // 3분면 ( + , - )
                if (templist[j].transform.localPosition.x > 0 && templist[j].transform.localPosition.y < 0)
                {
                    Section.Section3++;
                }

                // 4분면 ( + , + )
                if (templist[j].transform.localPosition.x > 0 && templist[j].transform.localPosition.y > 0)
                {
                    Section.Section4++;
                }
            }

            // 가장 적게 생성된면을 불러온다.
            var num = Section.DisRank();
            //Debug.Log(num + " 분면에 가장 적은 단어가 생성되었습니다.");

            if (m_RootSet >= MaxX / 100)
            {
                m_bIsMake = true;
                Debug.LogWarning("더 이상 루트를 생성 할 수 없습니다.");
                return;
            }
            // 가장 적게 생성된면의 랜덤으로 좌표를 구한다.
            MakeX = Section.GetX(num, m_RootSet);
            MakeY = Section.GetY(num, m_RootSet);
            m_RootSet++;
            return;
        }
    }
    bool CheckRoot()
    {
        if (!CheckQuater2(true))
        {
            return false;
        }
        // 생성전에 생성될 위치에 자리가 있는지 확인
        for (int i = 0; i < m_GridMakeList.Count; i++)
        {
            for (int j = 0; j < m_MakeWord.Length + 2; j++)
            {
                // y축으로 이동하며 생성할 자리가 비어있는지 확인
                //Debug.Log(m_GridMakeList[i].transform.localPosition + " 와 비교 " + new Vector3(MakeX, MakeY + (j * 110), 0)); -> 원인 : grid생성중 +=로 위치를 변경시켜주었기 때문에 편차가 남
                if (m_GridMakeList[i].transform.localPosition == new Vector3(MakeX + ((j - 1) * 110), MakeY, 0))
                { // 비어있지 않을때
                    //Debug.Log(m_MakeWord.Answer + " 단어 생성실패");
                    return false;
                }

                if (j < m_MakeWord.Length)
                {
                    // y축으로 이동하며 위아래가 비어있는지 확인
                    //Debug.Log(m_GridMakeList[i].transform.localPosition + " 와 비교 " + new Vector3(MakeX + 110, MakeY + (j * 110), 0));
                    if (m_GridMakeList[i].transform.localPosition == new Vector3(MakeX + (j * 110), MakeY + 110, 0))
                    { // 비어있지 않을때
                        //Debug.Log(m_MakeWord.Answer + " 단어 생성실패");
                        return false;
                    }

                    //Debug.Log(m_GridMakeList[i].transform.localPosition + " 와 비교 " + new Vector3(MakeX - 110, MakeY + (j * 110), 0));
                    if (m_GridMakeList[i].transform.localPosition == new Vector3(MakeX + (j * 110), MakeY - 110, 0))
                    { // 비어있지 않을때
                        //Debug.Log(m_MakeWord.Answer + " 단어 생성실패");
                        return false;
                    }
                }
            }
        }

        //Debug.Log("겹치지 않습니다.");
        return true;
    }
    bool CheckHorizontal()
    {
        // 생성전에 생성될 위치에 자리가 있는지 확인
        var templist = new List<GameObject>();
        templist.AddRange(GameObject.FindGameObjectsWithTag("Vertical"));
        templist.AddRange(GameObject.FindGameObjectsWithTag("Horizontal"));
        int tempcount = 0;
        int maxcount = templist.Count;
        for (int i = 0; i < maxcount; i++)
        {
            if (templist[i - tempcount].name.Contains(m_MakeWord.Answer))
            {
                templist.RemoveAt(i - tempcount);
                tempcount++;
            }
        }

        for (int i = 0; i < templist.Count; i++)
        {
            for (int j = 0; j < m_MakeWord.Length; j++)
            {
                // y축으로 이동하며 생성할 자리가 비어있는지 확인
                //Debug.Log(templist[i].transform.localPosition + " 와 비교 " + new Vector3(MakeX, MakeY + (j * 110), 0)); -> 원인 : grid생성중 +=로 위치를 변경시켜주었기 때문에 편차가 남
                if (templist[i].transform.localPosition == new Vector3(MakeX, MakeY + (j * 110), 0))
                { // 비어있지 않을때
                    //Debug.Log(m_MakeWord.Answer + " 단어 생성실패");
                    return false;
                }

                // y축으로 이동하며 위아래가 비어있는지 확인
                //Debug.Log(templist[i].transform.localPosition + " 와 비교 " + new Vector3(MakeX + 110, MakeY + (j * 110), 0));
                if (templist[i].transform.localPosition == new Vector3(MakeX + 1, MakeY + (j * 110), 0))
                { // 비어있지 않을때
                    //Debug.Log(m_MakeWord.Answer + " 단어 생성실패");
                    return false;
                }

                //Debug.Log(templist[i].transform.localPosition + " 와 비교 " + new Vector3(MakeX - 110, MakeY + (j * 110), 0));
                if (templist[i].transform.localPosition == new Vector3(MakeX - 1, MakeY + (j * 110), 0))
                { // 비어있지 않을때
                    //Debug.Log(m_MakeWord.Answer + " 단어 생성실패");
                    return false;
                }
            }
        }

        //Debug.Log("겹치지 않습니다.");
        return true;
    }
    bool CheckCrossHorizontal(bool first = true)
    {
        if (!CheckQuater2(false))
        {
            return false;
        }
        // 생성전에 생성될 위치에 자리가 있는지 확인
        var templist = new List<GameObject>();
        templist.AddRange(GameObject.FindGameObjectsWithTag("Vertical"));
        templist.AddRange(GameObject.FindGameObjectsWithTag("Horizontal"));
        if (first)
        {
            int tempcount = 0;
            int maxcount = templist.Count;
            for (int i = 0; i < maxcount; i++)
            {
                if (templist[i - tempcount].name.Contains(m_MakeWord.Answer))
                {
                    templist.RemoveAt(i - tempcount);
                    tempcount++;
                }
            }
        }

        for (int i = 0; i < templist.Count; i++)
        {
            for (int j = 0; j < m_TempWord.Length + 2; j++)
            {
                // y축으로 이동하며 생성할 자리가 비어있는지 확인
                //Debug.Log(templist[i].transform.localPosition + " 와 비교 " + new Vector3(MakeX, MakeY + (j * 110), 0)); -> 원인 : grid생성중 +=로 위치를 변경시켜주었기 때문에 편차가 남
                if (templist[i].transform.localPosition == new Vector3(MakeX, MakeY + ((j - 1) * 110), 0))
                { // 비어있지 않을때
                    //Debug.Log(m_TempWord.Answer + " 단어 생성실패");
                    return false;
                }

                if (j < m_TempWord.Length)
                {
                    // y축으로 이동하며 위아래가 비어있는지 확인
                    //Debug.Log(templist[i].transform.localPosition + " 와 비교 " + new Vector3(MakeX + 110, MakeY + (j * 110), 0));
                    if (templist[i].transform.localPosition == new Vector3(MakeX + 110, MakeY + (j * 110), 0))
                    { // 비어있지 않을때
                        //Debug.Log(m_TempWord.Answer + " 단어 생성실패");
                        return false;
                    }

                    //Debug.Log(templist[i].transform.localPosition + " 와 비교 " + new Vector3(MakeX - 110, MakeY + (j * 110), 0));
                    if (templist[i].transform.localPosition == new Vector3(MakeX - 110, MakeY + (j * 110), 0))
                    { // 비어있지 않을때
                        //Debug.Log(m_TempWord.Answer + " 단어 생성실패");
                        return false;
                    }
                }
            }
        }

        //Debug.Log("겹치지 않습니다.");
        return true;
    }
    bool CheckVertical()
    {
        // 생성전에 생성될 위치에 자리가 있는지 확인
        var templist = new List<GameObject>();
        templist.AddRange(GameObject.FindGameObjectsWithTag("Horizontal"));
        templist.AddRange(GameObject.FindGameObjectsWithTag("Vertical"));
        int tempcount = 0;
        int maxcount = templist.Count;
        for (int i = 0; i < maxcount; i++)
        {
            if (templist[i - tempcount].name.Contains(m_MakeWord.Answer))
            {
                templist.RemoveAt(i - tempcount);
                tempcount++;
            }
        }

        for (int i = 0; i < templist.Count; i++)
        {
            for (int j = 0; j < m_MakeWord.Length; j++)
            {
                // y축으로 이동하며 생성할 자리가 비어있는지 확인
                //Debug.Log(templist[i].transform.localPosition + " 와 비교 " + new Vector3(MakeX, MakeY + (j * 110), 0)); -> 원인 : grid생성중 +=로 위치를 변경시켜주었기 때문에 편차가 남
                if (templist[i].transform.localPosition == new Vector3(MakeX + (j * 110), MakeY, 0))
                { // 비어있지 않을때
                    //Debug.Log(m_MakeWord.Answer + " 단어 생성실패");
                    return false;
                }

                // y축으로 이동하며 위아래가 비어있는지 확인
                //Debug.Log(templist[i].transform.localPosition + " 와 비교 " + new Vector3(MakeX + 110, MakeY + (j * 110), 0));
                if (templist[i].transform.localPosition == new Vector3(MakeX + (j * 110), MakeY + 1, 0))
                { // 비어있지 않을때
                    //Debug.Log(m_MakeWord.Answer + " 단어 생성실패");
                    return false;
                }

                //Debug.Log(templist[i].transform.localPosition + " 와 비교 " + new Vector3(MakeX - 110, MakeY + (j * 110), 0));
                if (templist[i].transform.localPosition == new Vector3(MakeX + (j * 110), MakeY - 1, 0))
                { // 비어있지 않을때
                    //Debug.Log(m_MakeWord.Answer + " 단어 생성실패");
                    return false;
                }
            }
        }

        //Debug.Log("겹치지 않습니다.");
        return true;
    }
    bool CheckCrossVertical(bool first = true)
    {
        if (!CheckQuater2(true))
        {
            return false;
        }
        // 생성전에 생성될 위치에 자리가 있는지 확인
        var templist = new List<GameObject>();
        templist.AddRange(GameObject.FindGameObjectsWithTag("Horizontal"));
        templist.AddRange(GameObject.FindGameObjectsWithTag("Vertical"));

        if (first)
        {
            int tempcount = 0;
            int maxcount = templist.Count;
            for (int i = 0; i < maxcount; i++)
            {
                if (templist[i - tempcount].name.Contains(m_MakeWord.Answer))
                {
                    templist.RemoveAt(i - tempcount);
                    tempcount++;
                }
            }
        }

        for (int i = 0; i < templist.Count; i++)
        {
            for (int j = 0; j < m_TempWord.Length + 2; j++)
            {
                // y축으로 이동하며 생성할 자리가 비어있는지 확인
                //Debug.Log(templist[i].transform.localPosition + " 와 비교 " + new Vector3(MakeX, MakeY + (j * 110), 0)); -> 원인 : grid생성중 +=로 위치를 변경시켜주었기 때문에 편차가 남
                if (templist[i].transform.localPosition == new Vector3(MakeX + ((j - 1) * 110), MakeY, 0))
                { // 비어있지 않을때
                    //Debug.Log(m_TempWord.Answer + " 단어 생성실패");
                    return false;
                }

                if (j < m_TempWord.Length)
                {
                    // y축으로 이동하며 위아래가 비어있는지 확인
                    //Debug.Log(templist[i].transform.localPosition + " 와 비교 " + new Vector3(MakeX + 110, MakeY + (j * 110), 0));
                    if (templist[i].transform.localPosition == new Vector3(MakeX + (j * 110), MakeY + 110, 0))
                    { // 비어있지 않을때
                        //Debug.Log(m_TempWord.Answer + " 단어 생성실패");
                        return false;
                    }

                    //Debug.Log(templist[i].transform.localPosition + " 와 비교 " + new Vector3(MakeX - 110, MakeY + (j * 110), 0));
                    if (templist[i].transform.localPosition == new Vector3(MakeX + (j * 110), MakeY - 110, 0))
                    { // 비어있지 않을때
                        //Debug.Log(m_TempWord.Answer + " 단어 생성실패");
                        return false;
                    }
                }
            }
        }

        //Debug.Log("겹치지 않습니다.");
        return true;
    }

    // 루트가 될 단어를 생성해줌
    void RootSet()
    {
        if (m_CurrentWordCount >= m_WordCount)
        {
            safeloop = 0;
            m_bMakeAble = false;
            return;
        }

        // 루트가 될 단어를 랜덤으로 뽑는다.
        var index = Random.Range(0, m_WordList.Count);
        m_MakeWord = m_WordList[index];

        int tempcount = 0;
        // 크로스 워드가 있는지
        for (int i = 0; i < m_MakeWord.IndexInfo.Count; i++)
        {
            if (m_MakeWord.IndexInfo[i].Count > 0)
            {
                tempcount += m_MakeWord.IndexInfo[i].Count;
            }
        }

        if (tempcount <= 0)
        {
            Debug.Log("크로스 가능한 단어가 없습니다.");
            return;
        }
        Debug.Log("크로스 가능한 단어의 개수 : " + tempcount);

        //if(!m_bMakeAble)
        //{
        //    var temp = GameObject.Find(m_MakeList[m_MakeList.Count - 1].Answer + " " + 0.ToString());
        //    MakeX = temp.transform.localPosition.x;
        //    MakeY = temp.transform.localPosition.y;
        //}

        if (m_CurrentWordCount == 0)
        {
            Debug.Log("Root Word " + m_MakeWord.Answer + " 단어를 생성합니다.");
            // 생성해준다.
            VerticalMake();
            // 생성해주고 리스트에서 삭제
            UseWord(m_MakeWord);
            // 다음 단어는 Horizontal임을 표시
            m_bMakeAble = true;
            return;
        }
        else if (CheckRoot())
        { // 겹치지 않음
            Debug.Log("Root Word " + m_MakeWord.Answer + " 단어를 생성합니다.");
            // 생성해준다.
            VerticalMake();
            // 생성해주고 리스트에서 삭제
            UseWord(m_MakeWord);
            // 다음 단어는 Horizontal임을 표시
            m_bMakeAble = true;
            m_RootSet = 0;
            return;
        }
        else
        {
            Debug.Log("Root Word 생성 실패");
            m_MakeWord = null;
            safeloop++;
            SetRootPos();
            m_bMakeAble = false;
            return;
        }
    }

    // 루트에 교차되는 단어를 생성
    bool MakeRootCrossWord()
    {
        if(GameObject.Find(m_MakeWord.Answer + " " + 0).tag == "Horizontal")
        {
            m_bVertical = true;
        }
        else
        {
            m_bVertical = false;
        }
        Debug.Log(m_bVertical + " Cross Word 생성 시퀀스");
        // 단어 찾기 전 만들수 있는지 여부
        if (m_CurrentWordCount >= m_WordCount)
        {
            safeloop = 0;
            return false;
        }

        // 크로스 워드가 있는지
        for (int i = 0; i < m_MakeWord.IndexInfo.Count; i++)
        { // 크로스 워드가 존재할때
            if (m_MakeWord.m_bIsOpen[i] == true)
            { // 단어의 인덱스가 열려 있을때
                if (i + 1 != m_MakeWord.Length)
                { // 인덱스의 끝이 아닐때
                    if (m_MakeWord.m_bIsOpen[i + 1] != true)
                    { // 인덱스의 다음 인덱스가 비어있지 않을때
                        // 다시 찾는다.
                        continue;
                    }
                }
                if (i - 1 != -1)
                { // 인덱스의 처음이 아닐때
                    if (m_MakeWord.m_bIsOpen[i - 1] != true)
                    { // 인덱스의 이전 인덱스가 비어있지 않을때
                        // 다시 찾는다.
                        continue;
                    }
                }

                // 인덱스는 생성가능
                //Debug.Log("인덱스 검사 결과 생성 가능");

                for (int j = 0; j < m_MakeWord.IndexInfo[i].Count; j++)
                {
                    string temp = m_MakeWord.IndexInfo[i][j];
                    // 0 : Word, 1 : index1, 2 : index2
                    var temp_string = temp.Split(',');
                    int _1 = int.Parse(temp_string[1]);
                    int _2 = int.Parse(temp_string[2]);
                    m_TempWord = FindWord(temp_string[0]);

                    if (HasWord(temp_string[0]))
                    { // 단어가 있을때
                      //Debug.Log("해당 단어는 생성 가능 합니다.");

                        // 만들어진 리스트에서 교차되는 단어가 있고,
                        // 다른 단어들과 겹치지 말아야함(교차되는 단어는 제외)
                        float tempx = MakeX;
                        float tempy = MakeY;
                        if (m_bVertical)
                        {
                            MakeX = MakeX - (_2 * 110);
                            MakeY = MakeY + (_1 * 110);

                            if (CheckCrossVertical())
                            {
                                Debug.Log("Vertical " + m_TempWord.Answer + " 단어를 생성합니다. with " + m_MakeWord.Answer + " Index : " + _1 + ", " + _2);
                                m_MakeWord.m_bIsOpen[_1] = false;
                                m_TempWord.m_bIsOpen[_2] = false;
                                // 다음 생성될 단어의 위치도 저장해준다.
                                m_MakeWord = m_TempWord;
                                VerticalMake();
                                // 생성해주고 리스트에서 삭제
                                UseWord(m_MakeWord);
                                // 다음 단어는 Horizontal임을 표시
                                return true;
                            }
                            else
                            {
                                MakeX = tempx;
                                MakeY = tempy;
                                safeloop++;
                            }
                        }
                        else
                        {
                            MakeX = MakeX + (_1 * 110);
                            MakeY = MakeY - (_2 * 110);

                            if (CheckCrossHorizontal())
                            {
                                Debug.Log("Horizontal " + m_TempWord.Answer + " 단어를 생성합니다. with " + m_MakeWord.Answer + " Index : " + _1 + ", " + _2);
                                m_MakeWord.m_bIsOpen[_1] = false;
                                m_TempWord.m_bIsOpen[_2] = false;
                                // 다음 생성될 단어의 위치도 저장해준다.
                                m_MakeWord = m_TempWord;
                                HorizontalMake();
                                // 생성해주고 리스트에서 삭제
                                UseWord(m_MakeWord);
                                // 다음 단어는 Horizontal임을 표시
                                return true;
                            }
                            else
                            {
                                MakeX = tempx;
                                MakeY = tempy;
                                safeloop++;
                            }
                        }
                    }
                    else
                    { // 단어가 이미 생성되었을때
                        //Debug.Log(temp_string[0] + "해당 단어는 이미 생성되었습니다. 건너뜀");
                        safeloop++;
                        continue;
                    }
                }
            }
        }

        return false;
    }


    // 4개의 면중에서 가장 많이 생성된 면을 제외한 면에서 생성을 시작해준다.
    private struct QuaterSection
    {
        public int Section1;
        public int Section2;
        public int Section3;
        public int Section4;

        public struct IntQuater
        {
            public int MinX;
            public int MaxX;
            public int MinY;
            public int MaxY;

            public IntQuater(int _1, int _2, int _3, int _4)
            {
                MinX = _1;
                MaxX = _2;
                MinY = _3;
                MaxY = _4;
            }
        }

        //public IntQuater Section1_MinMax;
        //public IntQuater Section2_MinMax;
        //public IntQuater Section3_MinMax;
        //public IntQuater Section4_MinMax;

        public int Rank()
        {
            if (Section2 > Section3)
            {
                if (Section2 > Section1)
                {
                    if (Section2 > Section4)
                    {
                        return 2;
                    }
                    else
                    {
                        return 4;
                    }
                }
                else
                {
                    if (Section1 > Section4)
                    {
                        return 1;
                    }
                    else
                    {
                        return 4;
                    }
                }
            }
            else
            {
                if (Section3 > Section1)
                {
                    if (Section3 > Section4)
                    {
                        return 3;
                    }
                    else
                    {
                        return 4;
                    }
                }
                else
                {
                    if (Section1 > Section4)
                    {
                        return 1;
                    }
                    else
                    {
                        return 4;
                    }
                }
            }
        }
        public int DisRank()
        {
            if (Section2 < Section3)
            {
                if (Section2 < Section1)
                {
                    if (Section2 < Section4)
                    {
                        return 2;
                    }
                    else
                    {
                        return 4;
                    }
                }
                else
                {
                    if (Section1 < Section4)
                    {
                        return 1;
                    }
                    else
                    {
                        return 4;
                    }
                }
            }
            else
            {
                if (Section3 < Section1)
                {
                    if (Section3 < Section4)
                    {
                        return 3;
                    }
                    else
                    {
                        return 4;
                    }
                }
                else
                {
                    if (Section1 < Section4)
                    {
                        return 1;
                    }
                    else
                    {
                        return 4;
                    }
                }
            }
        }
        public int GetX(int sectionum, int randindex)
        {
            switch (sectionum)
            {
                case 1:
                    return randindex * -110;
                case 2:
                    return randindex * -110;
                case 3:
                    return randindex * 110;
                case 4:
                    return randindex * 110;
            }

            return 0;
        }
        public int GetY(int sectionum, int randindex)
        {
            switch (sectionum)
            {
                case 1:
                    return randindex * 110;
                case 2:
                    return randindex * -110;
                case 3:
                    return randindex * -110;
                case 4:
                    return randindex * 110;
            }

            return 0;
        }
    }

    // 여기서 스크롤 렉트 바깥으로 나가는 녀석들을 잡아주지 못함... | 현재까지 확인된 예외 : 양 옆으로 빠져나감
    bool CheckQuater(bool vertical)
    {
        // 생성될때 Rect안에만 생성되도록 체크
        if (vertical)
        {
            if (StartX < 0)
            { // x 음수
                if (StartX < MinX + 110)
                {
                    if (StartY < 0)
                    { // y 음수
                        if (StartY < MinY + 110)
                        {
                            return true;
                        }
                        return false;
                    }
                    else
                    { // y 양수
                        if (StartY < MaxY - 110)
                        {
                            return true;
                        }
                        return false;
                    }
                }
                return false;
            }
            else
            { // x 양수
                if (StartX + m_MakeWord.Length * 110 < MaxX - 110)
                {
                    if (StartY < 0)
                    { // y 음수
                        if (StartY < MinY + 110)
                        {
                            return true;
                        }
                        return false;
                    }
                    else
                    { // y 양수
                        if (StartY < MaxY - 110)
                        {
                            return true;
                        }
                        return false;
                    }
                }
                return false;
            }
        }
        else
        {
            if (StartY < 0)
            { // x 음수
                if (StartY < MinY + 110)
                {
                    if (StartX < 0)
                    { // y 음수
                        if (StartX < MinX + 110)
                        {
                            return true;
                        }
                        return false;
                    }
                    else
                    { // y 양수
                        if (StartX < MaxX - 110)
                        {
                            return true;
                        }
                        return false;
                    }
                }
                return false;
            }
            else
            { // x 양수
                if (StartY + m_MakeWord.Length * 110 < MaxY - 110)
                {
                    if (StartX < 0)
                    { // y 음수
                        if (StartX < MinX + 110)
                        {
                            return true;
                        }
                        return false;
                    }
                    else
                    { // y 양수
                        if (StartX < MaxX - 110)
                        {
                            return true;
                        }
                        return false;
                    }
                }
                return false;
            }
        }
    }
    bool CheckQuater2(bool vertical)
    {
        if (vertical)
        {
            if(MakeX > MinX)
            {
                if(MakeX + m_MakeWord.Length * 110 < MaxX)
                {
                    if(MakeY > MinY)
                    {
                        if (MakeY < MaxY)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        else
        {
            if (MakeX > MinX)
            {
                if (MakeX < MaxX)
                {
                    if (MakeY > MinY)
                    {
                        if (MakeY + m_MakeWord.Length * 110 < MaxY)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
    }

    public void RESET()
    {
        GameObject.Find("CrossMgr").GetComponent<CrossWord>().Reset();
        Resetting();
        Debug.Log("Reset Make");
        MakeX = StartX;
        MakeY = StartY;
        RectMin = new Vector2();
        RectMax = new Vector2();
        m_Reset = 5;
        safeloop = 0;
        m_CurrentWordCount = 0;
        m_bIsMake = false;
        m_bMakeAble = true;
        m_bVertical = true;
        m_MakeList = new List<Word>();
        m_WordList = new List<Word>();
    }
    public IEnumerator Reset()
    {
        Debug.Log("Reset Make");
        Resetting();
        m_bIsMake = false;
        yield return new WaitForSeconds(2f);
    }
    void Resetting()
    {
        var temp = m_GridMakeList.Count;
        for (int i = 0; i < temp; i++)
        {
            Destroy(m_GridMakeList[i]);
        }

        m_GridMakeList.RemoveRange(0, m_GridMakeList.Count);
        m_MakeList.RemoveRange(0, m_MakeList.Count);
    }

    Word FindWord(string _word)
    {
        for (int i = 0; i < m_WordList.Count; i++)
        {
            if (m_WordList[i].Answer.Contains(_word))
                return m_WordList[i];
        }

        return null;
    }

    bool HasWord(string _word)
    {
        for (int i = 0; i < m_WordList.Count; i++)
        {
            if (m_WordList[i].Answer.Contains(_word))
                return true;
        }

        return false;
    }

    public void SetCount(int _count)
    {
        m_WordCount = _count;
    }
}
