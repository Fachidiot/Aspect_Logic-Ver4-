using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GridMgr : MonoBehaviour
{
    public string str_name;
    public GameObject GridPrefab;

    private const string DefaultPath = "/Resource/Map/GridMap1Info.json";
    private GridInfo m_TempGrid;
    private string[,] m_Grid;
    private Word m_TempWord;
    private int m_WordCout;
    private List<Word> m_WordList;
    private List<Word> m_MainList;
    private bool m_bIsMake;
    public bool IsMake { get { return m_bIsMake; } }

    private List<GameObject> m_ObjList = new List<GameObject>();

    // Count Word List
    private List<Word> m_3List = new List<Word>();
    private List<Word> m_4List = new List<Word>();
    private List<Word> m_5List = new List<Word>();
    private List<Word> m_6List = new List<Word>();
    private List<Word> m_7List = new List<Word>();
    private List<Word> m_8List = new List<Word>();
    private List<Word> m_9List = new List<Word>();
    private List<Word> m_10List = new List<Word>();
    private List<Word> m_11List = new List<Word>();
    private List<Word> m_12List = new List<Word>();
    private List<Word> m_13List = new List<Word>();
    private List<Word> m_14List = new List<Word>();

    private void Start()
    {
        LoadMapData();
        PushData();
        //Debug.Log(m_3List.Count);
        //Debug.Log(m_4List.Count);
        //Debug.Log(m_5List.Count);
        //Debug.Log(m_6List.Count);
        //Debug.Log(m_7List.Count);
        //Debug.Log(m_8List.Count);
        //Debug.Log(m_9List.Count);
        //Debug.Log(m_10List.Count);
        //Debug.Log(m_11List.Count);
        //Debug.Log(m_12List.Count);
        //Debug.Log(m_13List.Count);
        //Debug.Log(m_14List.Count);
    }

    public static void ShuffleList<T>(List<T> list)
    {
        int random1;
        int random2;

        T temp;

        for (int index = 0; index < list.Count; ++index)
        {
            random1 = Random.Range(0, list.Count);
            random2 = Random.Range(0, list.Count);

            temp = list[random1];
            list[random1] = list[random2];
            list[random2] = temp;
        }
    }

    void ListSet()
    {
        m_WordList = new List<Word>();
        m_WordList = csvReader_ver2.GetList();
        m_3List = new List<Word>();
        m_4List = new List<Word>();
        m_5List = new List<Word>();
        m_6List = new List<Word>();
        m_7List = new List<Word>();
        m_8List = new List<Word>();
        m_9List = new List<Word>();
        m_10List = new List<Word>();
        m_11List = new List<Word>();
        m_12List = new List<Word>();
        m_13List = new List<Word>();
        m_14List = new List<Word>();
        for (int i = 0; i < m_WordList.Count; i++)
        {
            switch (m_WordList[i].Length)
            {
                case 3:
                    m_3List.Add(m_WordList[i]);
                    break;
                case 4:
                    m_4List.Add(m_WordList[i]);
                    break;
                case 5:
                    m_5List.Add(m_WordList[i]);
                    break;
                case 6:
                    m_6List.Add(m_WordList[i]);
                    break;
                case 7:
                    m_7List.Add(m_WordList[i]);
                    break;
                case 8:
                    m_8List.Add(m_WordList[i]);
                    break;
                case 9:
                    m_9List.Add(m_WordList[i]);
                    break;
                case 10:
                    m_10List.Add(m_WordList[i]);
                    break;
                case 11:
                    m_11List.Add(m_WordList[i]);
                    break;
                case 12:
                    m_12List.Add(m_WordList[i]);
                    break;
                case 13:
                    m_13List.Add(m_WordList[i]);
                    break;
                case 14:
                    m_14List.Add(m_WordList[i]);
                    break;
                default:
                    Debug.LogError("새로운 리스트 생성필요");
                    break;
            }
        }
    }

    GridInfo LoadMapData(string path = DefaultPath)
    {
        ListSet();
        string temp_string = File.ReadAllText(Application.dataPath + path);
        var var_Info = JsonUtility.FromJson<GridInfo>(temp_string);

        if(var_Info.CrossInfo6 == null)
        {
            m_WordCout = 5;
        }
        else if(var_Info.CrossInfo11 == null)
        {
            m_WordCout = 10;
        }
        else
        {
            m_WordCout = 15;
        }
        
        int x = 0;
        int y = 0;
        m_Grid = new string[m_WordCout, m_WordCout];
        for (int i = 0; i < var_Info.Grid.Length; i++)
        {
            if (i % m_WordCout == 0 && i != 0)
            { // \n
                x++;
                y = 0;
            }
            m_Grid[x, y] = var_Info.Grid[i];
            y++;
        }
        m_TempGrid = var_Info;
        Debug.Log("Successfully reading data");
        return m_TempGrid;
    }

    void PushData()
    {
        if(m_TempGrid == null)
        { return; }

        m_MainList = new List<Word>();
        char[] PrevCross = new char[0];
        int[]  PrevIndex = new int[0];

        string[,] SaveMap = new string[m_WordCout, m_WordCout];

        // 맞는 단어만 뽑기
        for (int i = 0; i < m_WordCout; i++)
        {
            switch (i + 1)
            {
                case 1:
                    for (int a = 0; a < m_WordCout; a++)
                    {
                        for (int b = 0; b < m_WordCout; b++)
                        {
                            SaveMap[a, b] = m_Grid[a, b];
                        }
                    }

                    var templist1 = GetList(m_TempGrid.CrossInfo1[2]);

                    for (int j = 0; j < templist1.Count; j++)
                    {
                        int random = Random.Range(0, templist1.Count);
                        m_TempWord = templist1[random];

                        if(m_TempGrid.CrossInfo1[3] == 100)
                        { // 단어가 수평일때
                            int count = 0;
                            for (int k = 0; k < m_TempWord.Length; k++)
                            {
                                if(m_Grid[m_TempGrid.CrossInfo1[0], m_TempGrid.CrossInfo1[1] + k] != "o" &&
                                    m_Grid[m_TempGrid.CrossInfo1[0], m_TempGrid.CrossInfo1[1] + k] != m_TempWord.Answer[k].ToString())
                                { // 이미 존재하는 단어가 다를경우
                                    count++;
                                }
                            }
                            if(count == 0)
                            { // 입력할 단어가 비어있거나 입력되어있는 단어의 낱말과 일치할때
                                for (int z = 0; z < m_TempWord.Length; z++)
                                {
                                    m_Grid[m_TempGrid.CrossInfo1[0], m_TempGrid.CrossInfo1[1] + z] = m_TempWord.Answer[z].ToString();
                                }
                                m_MainList.Add(m_TempWord);
                                templist1.Remove(m_TempWord);
                                break;
                            }
                        }
                        else if (m_TempGrid.CrossInfo1[3] == -100)
                        { // 단어가 수직일때
                            int count = 0;
                            for (int k = 0; k < m_TempWord.Length; k++)
                            {
                                if(m_Grid[m_TempGrid.CrossInfo1[0] + k, m_TempGrid.CrossInfo1[1]] != "o" &&
                                    m_Grid[m_TempGrid.CrossInfo1[0] + k, m_TempGrid.CrossInfo1[1]] != m_TempWord.Answer[k].ToString())
                                {
                                    count++;
                                }
                            }
                            if(count == 0)
                            {
                                for (int z = 0; z < m_TempWord.Length; z++)
                                {
                                    m_Grid[m_TempGrid.CrossInfo1[0] + z, m_TempGrid.CrossInfo1[1]] = m_TempWord.Answer[z].ToString();
                                }
                                m_MainList.Add(m_TempWord);
                                templist1.Remove(m_TempWord);
                                break;
                            }
                        }

                        if(templist1.Count == j + 1)
                        {
                            m_MainList.RemoveRange(0, m_MainList.Count);
                            ListSet();
                            for (int a = 0; a < m_WordCout; a++)
                            {
                                for (int b = 0; b < m_WordCout; b++)
                                {
                                    m_Grid[a, b] = SaveMap[a, b];
                                }
                            }
                            i = -1;
                            break;
                        }
                    }
                    break;
                case 2:
                    var templist2 = GetList(m_TempGrid.CrossInfo2[2]);
                    ShuffleList<Word>(templist2);

                    for (int j = 0; j < templist2.Count; j++)
                    {
                        m_TempWord = templist2[j];

                        if (m_TempGrid.CrossInfo2[3] == 100)
                        { // 단어가 수평일때
                            int count = 0;
                            for (int k = 0; k < m_TempWord.Length; k++)
                            {
                                if (m_Grid[m_TempGrid.CrossInfo2[0], m_TempGrid.CrossInfo2[1] + k] != "o" &&
                                    m_Grid[m_TempGrid.CrossInfo2[0], m_TempGrid.CrossInfo2[1] + k] != m_TempWord.Answer[k].ToString())
                                { // 이미 존재하는 단어가 다를경우
                                    count++;
                                }
                            }
                            if (count == 0)
                            { // 입력할 단어가 비어있거나 입력되어있는 단어의 낱말과 일치할때
                                for (int z = 0; z < m_TempWord.Length; z++)
                                {
                                    m_Grid[m_TempGrid.CrossInfo2[0], m_TempGrid.CrossInfo2[1] + z] = m_TempWord.Answer[z].ToString();
                                }
                                m_MainList.Add(m_TempWord);
                                templist2.Remove(m_TempWord);
                                break;
                            }
                        }
                        else if (m_TempGrid.CrossInfo2[3] == -100)
                        { // 단어가 수직일때
                            int count = 0;
                            for (int k = 0; k < m_TempWord.Length; k++)
                            {
                                if (m_Grid[m_TempGrid.CrossInfo2[0] + k, m_TempGrid.CrossInfo2[1]] != "o" &&
                                    m_Grid[m_TempGrid.CrossInfo2[0] + k, m_TempGrid.CrossInfo2[1]] != m_TempWord.Answer[k].ToString())
                                {
                                    count++;
                                }
                            }
                            if (count == 0)
                            {
                                for (int z = 0; z < m_TempWord.Length; z++)
                                {
                                    m_Grid[m_TempGrid.CrossInfo2[0] + z, m_TempGrid.CrossInfo2[1]] = m_TempWord.Answer[z].ToString();
                                }
                                m_MainList.Add(m_TempWord);
                                templist2.Remove(m_TempWord);
                                break;
                            }
                        }

                        if (templist2.Count == j + 1)
                        {
                            m_MainList.RemoveRange(0, m_MainList.Count);
                            ListSet();
                            for (int a = 0; a < m_WordCout; a++)
                            {
                                for (int b = 0; b < m_WordCout; b++)
                                {
                                    m_Grid[a, b] = SaveMap[a, b];
                                }
                            }
                            i = -1;
                            break;
                        }
                    }
                    break;
                case 3:
                    var templist3 = GetList(m_TempGrid.CrossInfo3[2]);
                    ShuffleList<Word>(templist3);

                    for (int j = 0; j < templist3.Count; j++)
                    {
                        m_TempWord = templist3[j];

                        if (m_TempGrid.CrossInfo3[3] == 100)
                        { // 단어가 수평일때
                            int count = 0;
                            for (int k = 0; k < m_TempWord.Length; k++)
                            {
                                if (m_Grid[m_TempGrid.CrossInfo3[0], m_TempGrid.CrossInfo3[1] + k] != "o" &&
                                    m_Grid[m_TempGrid.CrossInfo3[0], m_TempGrid.CrossInfo3[1] + k] != m_TempWord.Answer[k].ToString())
                                { // 이미 존재하는 단어가 다를경우
                                    count++;
                                }
                            }
                            if (count == 0)
                            { // 입력할 단어가 비어있거나 입력되어있는 단어의 낱말과 일치할때
                                for (int z = 0; z < m_TempWord.Length; z++)
                                {
                                    m_Grid[m_TempGrid.CrossInfo3[0], m_TempGrid.CrossInfo3[1] + z] = m_TempWord.Answer[z].ToString();
                                }
                                m_MainList.Add(m_TempWord);
                                templist3.Remove(m_TempWord);
                                break;
                            }
                        }
                        else if (m_TempGrid.CrossInfo3[3] == -100)
                        { // 단어가 수직일때
                            int count = 0;
                            for (int k = 0; k < m_TempWord.Length; k++)
                            {
                                if (m_Grid[m_TempGrid.CrossInfo3[0] + k, m_TempGrid.CrossInfo3[1]] != "o" &&
                                    m_Grid[m_TempGrid.CrossInfo3[0] + k, m_TempGrid.CrossInfo3[1]] != m_TempWord.Answer[k].ToString())
                                {
                                    count++;
                                }
                            }
                            if (count == 0)
                            {
                                for (int z = 0; z < m_TempWord.Length; z++)
                                {
                                    m_Grid[m_TempGrid.CrossInfo3[0] + z, m_TempGrid.CrossInfo3[1]] = m_TempWord.Answer[z].ToString();
                                }
                                m_MainList.Add(m_TempWord);
                                templist3.Remove(m_TempWord);
                                break;
                            }
                        }

                        if (templist3.Count == j + 1)
                        {
                            m_MainList.RemoveRange(0, m_MainList.Count);
                            ListSet();
                            for (int a = 0; a < m_WordCout; a++)
                            {
                                for (int b = 0; b < m_WordCout; b++)
                                {
                                    m_Grid[a, b] = SaveMap[a, b];
                                }
                            }
                            i = -1;
                            break;
                        }
                    }
                    break;
                case 4:
                    var templist4 = GetList(m_TempGrid.CrossInfo4[2]);
                    ShuffleList<Word>(templist4);

                    for (int j = 0; j < templist4.Count; j++)
                    {
                        m_TempWord = templist4[j];

                        if (m_TempGrid.CrossInfo4[3] == 100)
                        { // 단어가 수평일때
                            int count = 0;
                            for (int k = 0; k < m_TempWord.Length; k++)
                            {
                                if (m_Grid[m_TempGrid.CrossInfo4[0], m_TempGrid.CrossInfo4[1] + k] != "o" &&
                                    m_Grid[m_TempGrid.CrossInfo4[0], m_TempGrid.CrossInfo4[1] + k] != m_TempWord.Answer[k].ToString())
                                { // 이미 존재하는 단어가 다를경우
                                    count++;
                                }
                            }
                            if (count == 0)
                            { // 입력할 단어가 비어있거나 입력되어있는 단어의 낱말과 일치할때
                                for (int z = 0; z < m_TempWord.Length; z++)
                                {
                                    m_Grid[m_TempGrid.CrossInfo4[0], m_TempGrid.CrossInfo4[1] + z] = m_TempWord.Answer[z].ToString();
                                }
                                m_MainList.Add(m_TempWord);
                                templist4.Remove(m_TempWord);
                                break;
                            }
                        }
                        else if (m_TempGrid.CrossInfo4[3] == -100)
                        { // 단어가 수직일때
                            int count = 0;
                            for (int k = 0; k < m_TempWord.Length; k++)
                            {
                                if (m_Grid[m_TempGrid.CrossInfo4[0] + k, m_TempGrid.CrossInfo4[1]] != "o" &&
                                    m_Grid[m_TempGrid.CrossInfo4[0] + k, m_TempGrid.CrossInfo4[1]] != m_TempWord.Answer[k].ToString())
                                {
                                    count++;
                                }
                            }
                            if (count == 0)
                            {
                                for (int z = 0; z < m_TempWord.Length; z++)
                                {
                                    m_Grid[m_TempGrid.CrossInfo4[0] + z, m_TempGrid.CrossInfo4[1]] = m_TempWord.Answer[z].ToString();
                                }
                                m_MainList.Add(m_TempWord);
                                templist4.Remove(m_TempWord);
                                break;
                            }
                        }

                        if (templist4.Count == j + 1)
                        {
                            m_MainList.RemoveRange(0, m_MainList.Count);
                            ListSet();
                            for (int a = 0; a < m_WordCout; a++)
                            {
                                for (int b = 0; b < m_WordCout; b++)
                                {
                                    m_Grid[a, b] = SaveMap[a, b];
                                }
                            }
                            i = -1;
                            break;
                        }
                    }
                    break;
                case 5:
                    var templist5 = GetList(m_TempGrid.CrossInfo5[2]);
                    ShuffleList<Word>(templist5);

                    for (int j = 0; j < templist5.Count; j++)
                    {
                        m_TempWord = templist5[j];

                        if (m_TempGrid.CrossInfo5[3] == 100)
                        { // 단어가 수평일때
                            int count = 0;
                            for (int k = 0; k < m_TempWord.Length; k++)
                            {
                                if (m_Grid[m_TempGrid.CrossInfo5[0], m_TempGrid.CrossInfo5[1] + k] != "o" &&
                                    m_Grid[m_TempGrid.CrossInfo5[0], m_TempGrid.CrossInfo5[1] + k] != m_TempWord.Answer[k].ToString())
                                { // 이미 존재하는 단어가 다를경우
                                    count++;
                                }
                            }
                            if (count == 0)
                            { // 입력할 단어가 비어있거나 입력되어있는 단어의 낱말과 일치할때
                                for (int z = 0; z < m_TempWord.Length; z++)
                                {
                                    m_Grid[m_TempGrid.CrossInfo5[0], m_TempGrid.CrossInfo5[1] + z] = m_TempWord.Answer[z].ToString();
                                }
                                m_MainList.Add(m_TempWord);
                                templist5.Remove(m_TempWord);
                                break;
                            }
                        }
                        else if (m_TempGrid.CrossInfo5[3] == -100)
                        { // 단어가 수직일때
                            int count = 0;
                            for (int k = 0; k < m_TempWord.Length; k++)
                            {
                                if (m_Grid[m_TempGrid.CrossInfo5[0] + k, m_TempGrid.CrossInfo5[1]] != "o" &&
                                    m_Grid[m_TempGrid.CrossInfo5[0] + k, m_TempGrid.CrossInfo5[1]] != m_TempWord.Answer[k].ToString())
                                {
                                    count++;
                                }
                            }
                            if (count == 0)
                            {
                                for (int z = 0; z < m_TempWord.Length; z++)
                                {
                                    m_Grid[m_TempGrid.CrossInfo5[0] + z, m_TempGrid.CrossInfo5[1]] = m_TempWord.Answer[z].ToString();
                                }
                                m_MainList.Add(m_TempWord);
                                templist5.Remove(m_TempWord);
                                break;
                            }
                        }

                        if (templist5.Count == j + 1)
                        {
                            m_MainList.RemoveRange(0, m_MainList.Count);
                            ListSet();
                            for (int a = 0; a < m_WordCout; a++)
                            {
                                for (int b = 0; b < m_WordCout; b++)
                                {
                                    m_Grid[a, b] = SaveMap[a, b];
                                }
                            }
                            i = -1;
                            break;
                        }
                    }
                    break;
                case 6:
                    var templist6 = GetList(m_TempGrid.CrossInfo6[2]);
                    ShuffleList<Word>(templist6);

                    for (int j = 0; j < templist6.Count; j++)
                    {
                        m_TempWord = templist6[j];

                        if (m_TempGrid.CrossInfo6[3] == 100)
                        { // 단어가 수평일때
                            int count = 0;
                            for (int k = 0; k < m_TempWord.Length; k++)
                            {
                                if (m_Grid[m_TempGrid.CrossInfo6[0], m_TempGrid.CrossInfo6[1] + k] != "o" &&
                                    m_Grid[m_TempGrid.CrossInfo6[0], m_TempGrid.CrossInfo6[1] + k] != m_TempWord.Answer[k].ToString())
                                { // 이미 존재하는 단어가 다를경우
                                    count++;
                                }
                            }
                            if (count == 0)
                            { // 입력할 단어가 비어있거나 입력되어있는 단어의 낱말과 일치할때
                                for (int z = 0; z < m_TempWord.Length; z++)
                                {
                                    m_Grid[m_TempGrid.CrossInfo6[0], m_TempGrid.CrossInfo6[1] + z] = m_TempWord.Answer[z].ToString();
                                }
                                m_MainList.Add(m_TempWord);
                                templist6.Remove(m_TempWord);
                                break;
                            }
                        }
                        else if (m_TempGrid.CrossInfo6[3] == -100)
                        { // 단어가 수직일때
                            int count = 0;
                            for (int k = 0; k < m_TempWord.Length; k++)
                            {
                                if (m_Grid[m_TempGrid.CrossInfo6[0] + k, m_TempGrid.CrossInfo6[1]] != "o" &&
                                    m_Grid[m_TempGrid.CrossInfo6[0] + k, m_TempGrid.CrossInfo6[1]] != m_TempWord.Answer[k].ToString())
                                {
                                    count++;
                                }
                            }
                            if (count == 0)
                            {
                                for (int z = 0; z < m_TempWord.Length; z++)
                                {
                                    m_Grid[m_TempGrid.CrossInfo6[0] + z, m_TempGrid.CrossInfo6[1]] = m_TempWord.Answer[z].ToString();
                                }
                                m_MainList.Add(m_TempWord);
                                templist6.Remove(m_TempWord);
                                break;
                            }
                        }

                        if (templist6.Count == j + 1)
                        {
                            m_MainList.RemoveRange(0, m_MainList.Count);
                            ListSet();
                            for (int a = 0; a < m_WordCout; a++)
                            {
                                for (int b = 0; b < m_WordCout; b++)
                                {
                                    m_Grid[a, b] = SaveMap[a, b];
                                }
                            }
                            i = -1;
                            break;
                        }
                    }
                    break;
                case 7:
                    var templist7 = GetList(m_TempGrid.CrossInfo7[2]);
                    ShuffleList<Word>(templist7);

                    for (int j = 0; j < templist7.Count; j++)
                    {
                        m_TempWord = templist7[j];

                        if (m_TempGrid.CrossInfo7[3] == 100)
                        { // 단어가 수평일때
                            int count = 0;
                            for (int k = 0; k < m_TempWord.Length; k++)
                            {
                                if (m_Grid[m_TempGrid.CrossInfo7[0], m_TempGrid.CrossInfo7[1] + k] != "o" &&
                                    m_Grid[m_TempGrid.CrossInfo7[0], m_TempGrid.CrossInfo7[1] + k] != m_TempWord.Answer[k].ToString())
                                { // 이미 존재하는 단어가 다를경우
                                    count++;
                                }
                            }
                            if (count == 0)
                            { // 입력할 단어가 비어있거나 입력되어있는 단어의 낱말과 일치할때
                                for (int z = 0; z < m_TempWord.Length; z++)
                                {
                                    m_Grid[m_TempGrid.CrossInfo7[0], m_TempGrid.CrossInfo7[1] + z] = m_TempWord.Answer[z].ToString();
                                }
                                m_MainList.Add(m_TempWord);
                                templist7.Remove(m_TempWord);
                                break;
                            }
                        }
                        else if (m_TempGrid.CrossInfo7[3] == -100)
                        { // 단어가 수직일때
                            int count = 0;
                            for (int k = 0; k < m_TempWord.Length; k++)
                            {
                                if (m_Grid[m_TempGrid.CrossInfo7[0] + k, m_TempGrid.CrossInfo7[1]] != "o" &&
                                    m_Grid[m_TempGrid.CrossInfo7[0] + k, m_TempGrid.CrossInfo7[1]] != m_TempWord.Answer[k].ToString())
                                {
                                    count++;
                                }
                            }
                            if (count == 0)
                            {
                                for (int z = 0; z < m_TempWord.Length; z++)
                                {
                                    m_Grid[m_TempGrid.CrossInfo7[0] + z, m_TempGrid.CrossInfo7[1]] = m_TempWord.Answer[z].ToString();
                                }
                                m_MainList.Add(m_TempWord);
                                templist7.Remove(m_TempWord);
                                break;
                            }
                        }

                        if (templist7.Count == j + 1)
                        {
                            m_MainList.RemoveRange(0, m_MainList.Count);
                            ListSet();
                            for (int a = 0; a < m_WordCout; a++)
                            {
                                for (int b = 0; b < m_WordCout; b++)
                                {
                                    m_Grid[a, b] = SaveMap[a, b];
                                }
                            }
                            i = -1;
                            break;
                        }
                    }
                    break;
                case 8:
                    var templist8 = GetList(m_TempGrid.CrossInfo8[2]);
                    ShuffleList<Word>(templist8);

                    for (int j = 0; j < templist8.Count; j++)
                    {
                        m_TempWord = templist8[j];

                        if (m_TempGrid.CrossInfo8[3] == 100)
                        { // 단어가 수평일때
                            int count = 0;
                            for (int k = 0; k < m_TempWord.Length; k++)
                            {
                                if (m_Grid[m_TempGrid.CrossInfo8[0], m_TempGrid.CrossInfo8[1] + k] != "o" &&
                                    m_Grid[m_TempGrid.CrossInfo8[0], m_TempGrid.CrossInfo8[1] + k] != m_TempWord.Answer[k].ToString())
                                { // 이미 존재하는 단어가 다를경우
                                    count++;
                                }
                            }
                            if (count == 0)
                            { // 입력할 단어가 비어있거나 입력되어있는 단어의 낱말과 일치할때
                                for (int z = 0; z < m_TempWord.Length; z++)
                                {
                                    m_Grid[m_TempGrid.CrossInfo8[0], m_TempGrid.CrossInfo8[1] + z] = m_TempWord.Answer[z].ToString();
                                }
                                m_MainList.Add(m_TempWord);
                                templist8.Remove(m_TempWord);
                                break;
                            }
                        }
                        else if (m_TempGrid.CrossInfo8[3] == -100)
                        { // 단어가 수직일때
                            int count = 0;
                            for (int k = 0; k < m_TempWord.Length; k++)
                            {
                                if (m_Grid[m_TempGrid.CrossInfo8[0] + k, m_TempGrid.CrossInfo8[1]] != "o" &&
                                    m_Grid[m_TempGrid.CrossInfo8[0] + k, m_TempGrid.CrossInfo8[1]] != m_TempWord.Answer[k].ToString())
                                {
                                    count++;
                                }
                            }
                            if (count == 0)
                            {
                                for (int z = 0; z < m_TempWord.Length; z++)
                                {
                                    m_Grid[m_TempGrid.CrossInfo8[0] + z, m_TempGrid.CrossInfo8[1]] = m_TempWord.Answer[z].ToString();
                                }
                                m_MainList.Add(m_TempWord);
                                templist8.Remove(m_TempWord);
                                break;
                            }
                        }

                        if (templist8.Count == j + 1)
                        {
                            m_MainList.RemoveRange(0, m_MainList.Count);
                            ListSet();
                            for (int a = 0; a < m_WordCout; a++)
                            {
                                for (int b = 0; b < m_WordCout; b++)
                                {
                                    m_Grid[a, b] = SaveMap[a, b];
                                }
                            }
                            i = -1;
                            break;
                        }
                    }
                    break;
                case 9:
                    var templist9 = GetList(m_TempGrid.CrossInfo9[2]);
                    ShuffleList<Word>(templist9);

                    for (int j = 0; j < templist9.Count; j++)
                    {
                        m_TempWord = templist9[j];

                        if (m_TempGrid.CrossInfo9[3] == 100)
                        { // 단어가 수평일때
                            int count = 0;
                            for (int k = 0; k < m_TempWord.Length; k++)
                            {
                                if (m_Grid[m_TempGrid.CrossInfo9[0], m_TempGrid.CrossInfo9[1] + k] != "o" &&
                                    m_Grid[m_TempGrid.CrossInfo9[0], m_TempGrid.CrossInfo9[1] + k] != m_TempWord.Answer[k].ToString())
                                { // 이미 존재하는 단어가 다를경우
                                    count++;
                                }
                            }
                            if (count == 0)
                            { // 입력할 단어가 비어있거나 입력되어있는 단어의 낱말과 일치할때
                                for (int z = 0; z < m_TempWord.Length; z++)
                                {
                                    m_Grid[m_TempGrid.CrossInfo9[0], m_TempGrid.CrossInfo9[1] + z] = m_TempWord.Answer[z].ToString();
                                }
                                m_MainList.Add(m_TempWord);
                                templist9.Remove(m_TempWord);
                                break;
                            }
                        }
                        else if (m_TempGrid.CrossInfo9[3] == -100)
                        { // 단어가 수직일때
                            int count = 0;
                            for (int k = 0; k < m_TempWord.Length; k++)
                            {
                                if (m_Grid[m_TempGrid.CrossInfo9[0] + k, m_TempGrid.CrossInfo9[1]] != "o" &&
                                    m_Grid[m_TempGrid.CrossInfo9[0] + k, m_TempGrid.CrossInfo9[1]] != m_TempWord.Answer[k].ToString())
                                {
                                    count++;
                                }
                            }
                            if (count == 0)
                            {
                                for (int z = 0; z < m_TempWord.Length; z++)
                                {
                                    m_Grid[m_TempGrid.CrossInfo9[0] + z, m_TempGrid.CrossInfo9[1]] = m_TempWord.Answer[z].ToString();
                                }
                                m_MainList.Add(m_TempWord);
                                templist9.Remove(m_TempWord);
                                break;
                            }
                        }

                        if (templist9.Count == j + 1)
                        {
                            m_MainList.RemoveRange(0, m_MainList.Count);
                            ListSet();
                            for (int a = 0; a < m_WordCout; a++)
                            {
                                for (int b = 0; b < m_WordCout; b++)
                                {
                                    m_Grid[a, b] = SaveMap[a, b];
                                }
                            }
                            i = -1;
                            break;
                        }
                    }
                    break;
                case 10:
                    var templist10 = GetList(m_TempGrid.CrossInfo10[2]);
                    ShuffleList<Word>(templist10);

                    for (int j = 0; j < templist10.Count; j++)
                    {
                        m_TempWord = templist10[j];

                        if (m_TempGrid.CrossInfo10[3] == 100)
                        { // 단어가 수평일때
                            int count = 0;
                            for (int k = 0; k < m_TempWord.Length; k++)
                            {
                                if (m_Grid[m_TempGrid.CrossInfo10[0], m_TempGrid.CrossInfo10[1] + k] != "o" &&
                                    m_Grid[m_TempGrid.CrossInfo10[0], m_TempGrid.CrossInfo10[1] + k] != m_TempWord.Answer[k].ToString())
                                { // 이미 존재하는 단어가 다를경우
                                    count++;
                                }
                            }
                            if (count == 0)
                            { // 입력할 단어가 비어있거나 입력되어있는 단어의 낱말과 일치할때
                                for (int z = 0; z < m_TempWord.Length; z++)
                                {
                                    m_Grid[m_TempGrid.CrossInfo10[0], m_TempGrid.CrossInfo10[1] + z] = m_TempWord.Answer[z].ToString();
                                }
                                m_MainList.Add(m_TempWord);
                                templist10.Remove(m_TempWord);
                                break;
                            }
                        }
                        else if (m_TempGrid.CrossInfo10[3] == -100)
                        { // 단어가 수직일때
                            int count = 0;
                            for (int k = 0; k < m_TempWord.Length; k++)
                            {
                                if (m_Grid[m_TempGrid.CrossInfo10[0] + k, m_TempGrid.CrossInfo10[1]] != "o" &&
                                    m_Grid[m_TempGrid.CrossInfo10[0] + k, m_TempGrid.CrossInfo10[1]] != m_TempWord.Answer[k].ToString())
                                {
                                    count++;
                                }
                            }
                            if (count == 0)
                            {
                                for (int z = 0; z < m_TempWord.Length; z++)
                                {
                                    m_Grid[m_TempGrid.CrossInfo10[0] + z, m_TempGrid.CrossInfo10[1]] = m_TempWord.Answer[z].ToString();
                                }
                                m_MainList.Add(m_TempWord);
                                templist10.Remove(m_TempWord);
                                break;
                            }
                        }

                        if (templist10.Count == j + 1)
                        {
                            m_MainList.RemoveRange(0, m_MainList.Count);
                            ListSet();
                            for (int a = 0; a < m_WordCout; a++)
                            {
                                for (int b = 0; b < m_WordCout; b++)
                                {
                                    m_Grid[a, b] = SaveMap[a, b];
                                }
                            }
                            i = -1;
                            break;
                        }
                    }
                    break;
                case 11:
                    var templist11 = GetList(m_TempGrid.CrossInfo11[2]);
                    ShuffleList<Word>(templist11);

                    for (int j = 0; j < templist11.Count; j++)
                    {
                        m_TempWord = templist11[j];

                        if (m_TempGrid.CrossInfo11[3] == 100)
                        { // 단어가 수평일때
                            int count = 0;
                            for (int k = 0; k < m_TempWord.Length; k++)
                            {
                                if (m_Grid[m_TempGrid.CrossInfo11[0], m_TempGrid.CrossInfo11[1] + k] != "o" &&
                                    m_Grid[m_TempGrid.CrossInfo11[0], m_TempGrid.CrossInfo11[1] + k] != m_TempWord.Answer[k].ToString())
                                { // 이미 존재하는 단어가 다를경우
                                    count++;
                                }
                            }
                            if (count == 0)
                            { // 입력할 단어가 비어있거나 입력되어있는 단어의 낱말과 일치할때
                                for (int z = 0; z < m_TempWord.Length; z++)
                                {
                                    m_Grid[m_TempGrid.CrossInfo11[0], m_TempGrid.CrossInfo11[1] + z] = m_TempWord.Answer[z].ToString();
                                }
                                m_MainList.Add(m_TempWord);
                                templist11.Remove(m_TempWord);
                                break;
                            }
                        }
                        else if (m_TempGrid.CrossInfo11[3] == -100)
                        { // 단어가 수직일때
                            int count = 0;
                            for (int k = 0; k < m_TempWord.Length; k++)
                            {
                                if (m_Grid[m_TempGrid.CrossInfo11[0] + k, m_TempGrid.CrossInfo11[1]] != "o" &&
                                    m_Grid[m_TempGrid.CrossInfo11[0] + k, m_TempGrid.CrossInfo11[1]] != m_TempWord.Answer[k].ToString())
                                {
                                    count++;
                                }
                            }
                            if (count == 0)
                            {
                                for (int z = 0; z < m_TempWord.Length; z++)
                                {
                                    m_Grid[m_TempGrid.CrossInfo11[0] + z, m_TempGrid.CrossInfo11[1]] = m_TempWord.Answer[z].ToString();
                                }
                                m_MainList.Add(m_TempWord);
                                templist11.Remove(m_TempWord);
                                break;
                            }
                        }

                        if (templist11.Count == j + 1)
                        {
                            m_MainList.RemoveRange(0, m_MainList.Count);
                            ListSet();
                            for (int a = 0; a < m_WordCout; a++)
                            {
                                for (int b = 0; b < m_WordCout; b++)
                                {
                                    m_Grid[a, b] = SaveMap[a, b];
                                }
                            }
                            i = -1;
                            break;
                        }
                    }
                    break;
                case 12:
                    var templist12 = GetList(m_TempGrid.CrossInfo12[2]);
                    ShuffleList<Word>(templist12);

                    for (int j = 0; j < templist12.Count; j++)
                    {
                        m_TempWord = templist12[j];

                        if (m_TempGrid.CrossInfo12[3] == 100)
                        { // 단어가 수평일때
                            int count = 0;
                            for (int k = 0; k < m_TempWord.Length; k++)
                            {
                                if (m_Grid[m_TempGrid.CrossInfo12[0], m_TempGrid.CrossInfo12[1] + k] != "o" &&
                                    m_Grid[m_TempGrid.CrossInfo12[0], m_TempGrid.CrossInfo12[1] + k] != m_TempWord.Answer[k].ToString())
                                { // 이미 존재하는 단어가 다를경우
                                    count++;
                                }
                            }
                            if (count == 0)
                            { // 입력할 단어가 비어있거나 입력되어있는 단어의 낱말과 일치할때
                                for (int z = 0; z < m_TempWord.Length; z++)
                                {
                                    m_Grid[m_TempGrid.CrossInfo12[0], m_TempGrid.CrossInfo12[1] + z] = m_TempWord.Answer[z].ToString();
                                }
                                m_MainList.Add(m_TempWord);
                                templist12.Remove(m_TempWord);
                                break;
                            }
                        }
                        else if (m_TempGrid.CrossInfo12[3] == -100)
                        { // 단어가 수직일때
                            int count = 0;
                            for (int k = 0; k < m_TempWord.Length; k++)
                            {
                                if (m_Grid[m_TempGrid.CrossInfo12[0] + k, m_TempGrid.CrossInfo12[1]] != "o" &&
                                    m_Grid[m_TempGrid.CrossInfo12[0] + k, m_TempGrid.CrossInfo12[1]] != m_TempWord.Answer[k].ToString())
                                {
                                    count++;
                                }
                            }
                            if (count == 0)
                            {
                                for (int z = 0; z < m_TempWord.Length; z++)
                                {
                                    m_Grid[m_TempGrid.CrossInfo12[0] + z, m_TempGrid.CrossInfo12[1]] = m_TempWord.Answer[z].ToString();
                                }
                                m_MainList.Add(m_TempWord);
                                templist12.Remove(m_TempWord);
                                break;
                            }
                        }

                        if (templist12.Count == j + 1)
                        {
                            m_MainList.RemoveRange(0, m_MainList.Count);
                            ListSet();
                            for (int a = 0; a < m_WordCout; a++)
                            {
                                for (int b = 0; b < m_WordCout; b++)
                                {
                                    m_Grid[a, b] = SaveMap[a, b];
                                }
                            }
                            i = -1;
                            break;
                        }
                    }
                    break;
                case 13:
                    var templist13 = GetList(m_TempGrid.CrossInfo13[2]);
                    ShuffleList<Word>(templist13);

                    for (int j = 0; j < templist13.Count; j++)
                    {
                        m_TempWord = templist13[j];

                        if (m_TempGrid.CrossInfo13[3] == 100)
                        { // 단어가 수평일때
                            int count = 0;
                            for (int k = 0; k < m_TempWord.Length; k++)
                            {
                                if (m_Grid[m_TempGrid.CrossInfo13[0], m_TempGrid.CrossInfo13[1] + k] != "o" &&
                                    m_Grid[m_TempGrid.CrossInfo13[0], m_TempGrid.CrossInfo13[1] + k] != m_TempWord.Answer[k].ToString())
                                { // 이미 존재하는 단어가 다를경우
                                    count++;
                                }
                            }
                            if (count == 0)
                            { // 입력할 단어가 비어있거나 입력되어있는 단어의 낱말과 일치할때
                                for (int z = 0; z < m_TempWord.Length; z++)
                                {
                                    m_Grid[m_TempGrid.CrossInfo13[0], m_TempGrid.CrossInfo13[1] + z] = m_TempWord.Answer[z].ToString();
                                }
                                m_MainList.Add(m_TempWord);
                                templist13.Remove(m_TempWord);
                                break;
                            }
                        }
                        else if (m_TempGrid.CrossInfo13[3] == -100)
                        { // 단어가 수직일때
                            int count = 0;
                            for (int k = 0; k < m_TempWord.Length; k++)
                            {
                                if (m_Grid[m_TempGrid.CrossInfo13[0] + k, m_TempGrid.CrossInfo13[1]] != "o" &&
                                    m_Grid[m_TempGrid.CrossInfo13[0] + k, m_TempGrid.CrossInfo13[1]] != m_TempWord.Answer[k].ToString())
                                {
                                    count++;
                                }
                            }
                            if (count == 0)
                            {
                                for (int z = 0; z < m_TempWord.Length; z++)
                                {
                                    m_Grid[m_TempGrid.CrossInfo13[0] + z, m_TempGrid.CrossInfo1[1]] = m_TempWord.Answer[z].ToString();
                                }
                                m_MainList.Add(m_TempWord);
                                templist13.Remove(m_TempWord);
                                break;
                            }
                        }

                        if (templist13.Count == j + 1)
                        {
                            m_MainList.RemoveRange(0, m_MainList.Count);
                            ListSet();
                            for (int a = 0; a < m_WordCout; a++)
                            {
                                for (int b = 0; b < m_WordCout; b++)
                                {
                                    m_Grid[a, b] = SaveMap[a, b];
                                }
                            }
                            i = -1;
                            break;
                        }
                    }
                    break;
                case 14:
                    var templist14 = GetList(m_TempGrid.CrossInfo14[2]);
                    ShuffleList<Word>(templist14);

                    for (int j = 0; j < templist14.Count; j++)
                    {
                        m_TempWord = templist14[j];

                        if (m_TempGrid.CrossInfo14[3] == 100)
                        { // 단어가 수평일때
                            int count = 0;
                            for (int k = 0; k < m_TempWord.Length; k++)
                            {
                                if (m_Grid[m_TempGrid.CrossInfo14[0], m_TempGrid.CrossInfo14[1] + k] != "o" &&
                                    m_Grid[m_TempGrid.CrossInfo14[0], m_TempGrid.CrossInfo14[1] + k] != m_TempWord.Answer[k].ToString())
                                { // 이미 존재하는 단어가 다를경우
                                    count++;
                                }
                            }
                            if (count == 0)
                            { // 입력할 단어가 비어있거나 입력되어있는 단어의 낱말과 일치할때
                                for (int z = 0; z < m_TempWord.Length; z++)
                                {
                                    m_Grid[m_TempGrid.CrossInfo14[0], m_TempGrid.CrossInfo14[1] + z] = m_TempWord.Answer[z].ToString();
                                }
                                m_MainList.Add(m_TempWord);
                                templist14.Remove(m_TempWord);
                                break;
                            }
                        }
                        else if (m_TempGrid.CrossInfo14[3] == -100)
                        { // 단어가 수직일때
                            int count = 0;
                            for (int k = 0; k < m_TempWord.Length; k++)
                            {
                                if (m_Grid[m_TempGrid.CrossInfo14[0] + k, m_TempGrid.CrossInfo14[1]] != "o" &&
                                    m_Grid[m_TempGrid.CrossInfo14[0] + k, m_TempGrid.CrossInfo14[1]] != m_TempWord.Answer[k].ToString())
                                {
                                    count++;
                                }
                            }
                            if (count == 0)
                            {
                                for (int z = 0; z < m_TempWord.Length; z++)
                                {
                                    m_Grid[m_TempGrid.CrossInfo14[0] + z, m_TempGrid.CrossInfo14[1]] = m_TempWord.Answer[z].ToString();
                                }
                                m_MainList.Add(m_TempWord);
                                templist14.Remove(m_TempWord);
                                break;
                            }
                        }

                        if (templist14.Count == j + 1)
                        {
                            m_MainList.RemoveRange(0, m_MainList.Count);
                            ListSet();
                            for (int a = 0; a < m_WordCout; a++)
                            {
                                for (int b = 0; b < m_WordCout; b++)
                                {
                                    m_Grid[a, b] = SaveMap[a, b];
                                }
                            }
                            i = -1;
                            break;
                        }
                    }
                    break;
                case 15:
                    var templist15 = GetList(m_TempGrid.CrossInfo15[2]);
                    ShuffleList<Word>(templist15);

                    for (int j = 0; j < templist15.Count; j++)
                    {
                        m_TempWord = templist15[j];

                        if (m_TempGrid.CrossInfo15[3] == 100)
                        { // 단어가 수평일때
                            int count = 0;
                            for (int k = 0; k < m_TempWord.Length; k++)
                            {
                                if (m_Grid[m_TempGrid.CrossInfo15[0], m_TempGrid.CrossInfo15[1] + k] != "o" &&
                                    m_Grid[m_TempGrid.CrossInfo15[0], m_TempGrid.CrossInfo15[1] + k] != m_TempWord.Answer[k].ToString())
                                { // 이미 존재하는 단어가 다를경우
                                    count++;
                                }
                            }
                            if (count == 0)
                            { // 입력할 단어가 비어있거나 입력되어있는 단어의 낱말과 일치할때
                                for (int z = 0; z < m_TempWord.Length; z++)
                                {
                                    m_Grid[m_TempGrid.CrossInfo15[0], m_TempGrid.CrossInfo15[1] + z] = m_TempWord.Answer[z].ToString();
                                }
                                m_MainList.Add(m_TempWord);
                                templist15.Remove(m_TempWord);
                                break;
                            }
                        }
                        else if (m_TempGrid.CrossInfo15[3] == -100)
                        { // 단어가 수직일때
                            int count = 0;
                            for (int k = 0; k < m_TempWord.Length; k++)
                            {
                                if (m_Grid[m_TempGrid.CrossInfo15[0] + k, m_TempGrid.CrossInfo15[1]] != "o" &&
                                    m_Grid[m_TempGrid.CrossInfo15[0] + k, m_TempGrid.CrossInfo15[1]] != m_TempWord.Answer[k].ToString())
                                {
                                    count++;
                                }
                            }
                            if (count == 0)
                            {
                                for (int z = 0; z < m_TempWord.Length; z++)
                                {
                                    m_Grid[m_TempGrid.CrossInfo15[0] + z, m_TempGrid.CrossInfo15[1]] = m_TempWord.Answer[z].ToString();
                                }
                                m_MainList.Add(m_TempWord);
                                templist15.Remove(m_TempWord);
                                break;
                            }
                        }

                        if (templist15.Count == j + 1)
                        {
                            m_MainList.RemoveRange(0, m_MainList.Count);
                            ListSet();
                            for (int a = 0; a < m_WordCout; a++)
                            {
                                for (int b = 0; b < m_WordCout; b++)
                                {
                                    m_Grid[a, b] = SaveMap[a, b];
                                }
                            }
                            i = -1;
                            break;
                        }
                    }
                    break;
                default:
                    break;
            }
        }

        // 단어UI 오브젝트 생성
        for (int i = 0; i < m_WordCout; i++)
        {
            for (int j = 0; j < m_WordCout; j++)
            {
                if (m_Grid[i,j] != "x")
                {
                    var temp = Instantiate(GridPrefab, GameObject.Find("GridContainer").gameObject.transform);
                    temp.name = m_Grid[i, j];
                    m_ObjList.Add(temp);
                    temp.transform.localPosition = new Vector3((110 * i) - 250, (110 * j) - 250, 0);
                }
            }
        }

        m_bIsMake = true;
    }

    public List<Word> GetMainList()
    {
        return m_MainList;
    }

    List<Word> GetList(int i)
    {
        switch (i)
        {
            case 3:
                return m_3List;
            case 4:
                return m_4List;
            case 5:
                return m_5List;
            case 6:
                return m_6List;
            case 7:
                return m_7List;
            case 8:
                return m_8List;
            case 9:
                return m_9List;
            case 10:
                return m_10List;
            case 11:
                return m_11List;
            case 12:
                return m_12List;
            case 13:
                return m_13List;
            case 14:
                return m_14List;
            default:
                Debug.LogError("ㄴㅇㄱ");
                return null;
        }
    }

    List<Word> PeekList(int length)
    {
        List<Word> temp = new List<Word>();
        for (int i = 0; i < m_WordList.Count; i++)
        {
            if(m_WordList[i].Length == length)
            {
                temp.Add(m_WordList[i]);
            }
        }

        return temp;
    }

    // 교차단어를 체크
    void CheckCross(int index)
    {
        for (int i = 0; i < m_WordList.Count; i++)
        {
            // 자기 자신을 제외
            if (i == index)
                continue;

            // 해당 단어와 교차하는 단어를 저장
            for (int j = 0; j < m_WordList[i].Length; j++)
            {
                if (m_WordList[index].Answer.Contains(m_WordList[i].Answer[j].ToString()))
                { // 해당 단어와 교차되는 단어일때
                    var temp_int = m_WordList[index].FindAlphabetCount(m_WordList[i].Answer[j].ToString());
                    if (temp_int <= 0)
                    {
                        // 오류
                        continue;
                    }
                    else if (temp_int == 1)
                    { // 맞는 알파벳이 1개일때
                        var temp = m_WordList[index].FindAlphabetIndex(m_WordList[i].Answer[j].ToString());
                        // 해당 리스트인덱스에 string리스트를 추가해준다.
                        // Word, 1, 2, false
                        var temp_string = m_WordList[i].Answer + ", " + temp + ", " + j;
                        m_WordList[index].IndexInfo[temp].Add(temp_string);
                    }
                    else
                    { // 2개 이상일때
                        for (int k = 0; k < temp_int; k++)
                        {
                            var temp = m_WordList[index].FindAlphabetIndexGreater(m_WordList[i].Answer[j].ToString(), k);
                            // 해당 리스트인덱스에 string리스트를 추가해준다.
                            // Word, 1, 2, false
                            var temp_string = m_WordList[i].Answer + ", " + temp + ", " + j;
                            m_WordList[index].IndexInfo[temp].Add(temp_string);
                        }
                    }
                }
            }
        }
    }
    public void DeleteObj()
    {
        if (m_ObjList == null)
            return;
        int temp_int = m_ObjList.Count;
        for (int i = 0; i < temp_int; i++)
        {
            Destroy(m_ObjList[0]);
            m_ObjList.Remove(m_ObjList[0]);
        }
    }

    public void MakeGrid1()
    {
        DeleteObj();
        LoadMapData("/Resource/Map/GridMap1Info.json");
        PushData();
    }
    public void MakeGrid2()
    {
        DeleteObj();
        LoadMapData("/Resource/Map/GridMap2Info.json");
        PushData();
    }
    public void MakeGrid3()
    {
        DeleteObj();
        LoadMapData("/Resource/Map/GridMap3Info.json");
        PushData();
    }
    public void MakeGrid4()
    {
        DeleteObj();
        LoadMapData("/Resource/Map/GridMap4Info.json");
        PushData();
    }
    public void MakeGrid5()
    {
        DeleteObj();
        LoadMapData("/Resource/Map/GridMap5Info.json");
        PushData();
    }
}
