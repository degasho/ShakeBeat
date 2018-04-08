using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public GameObject[] notes;
    //private float[] _timing;
    //private int[] _lineNum;

    private List<float> _timing;
    private List<int> _lineNum ;

    //public string filePass;
    private string filePass;
    private int _notesCount = 0;
    private int destroyNotesNum = -1;

    private int disappearedNotesCount = 0;

    private AudioSource _audioSource;
    private float _startTime = 0;

    public float timeOffset = -1;

    private bool _isPlaying = true;
    //public GameObject startButton;

    public Text scoreText;
    public float _score = 0;

    public Text comboText1;
    public Text comboText2;
    public int comboCount = 0;
    public int maxCombocount = 0;
    private bool comboflag = false;

    public int lastNotes = 0;

    public int goodnotes = 0;
    public int greatnotes = 0;
    public int perfectnotes = 0;
    public int badnotes = 0;
    public int missnotes = 0;

    public float goodTiming;                //判定の猶予
    public float greatTiming;
    public float perfectTiming;
    public float badTiming;

    public Text[] judgeText = new Text[2];
    public float judgeTextDestroyTime;
    private float[] judgeTextTime = new float[2];

    public Text[] judgeCountText = new Text[5];

    public GameObject[] effect = new GameObject[2];
    public Material[] effectMaterial = new Material[2];
    public int[] effectFlag = new int[2];

    private float pauseTime = 0;
    public bool isPause = false;

    public GameObject pauseMenu;
    public Text[] pauseText = new Text[3];
    private int selectMenu = 2;

    public static bool resetNotes = false;

    private float[] notesPoint = new float[4];
    private MainGameController mgc = new MainGameController();

    // Use this for initialization
    void Start () {
        Debug.Log(MainGameController.musicPass);
        _audioSource = GameObject.Find(MainGameController.musicPass).GetComponent<AudioSource>();
        
        filePass = MainGameController.csvFilePass;
        Debug.Log(filePass);
        _timing = new List<float>();
        _lineNum = new List<int>();
        LoadCSV();
        lastNotes = _timing.Count;
        comboText1.text = "";
        comboText2.text = "";
        judgeText[0].text = "";
        judgeText[1].text = "";
        NotesPoint();
        StartGame();
        resetNotes = false;
    }
	

	// Update is called once per frame
	void Update () {
        if (_isPlaying)
        {
            CheckNextNotes();
            scoreText.text = ((int)_score).ToString();
            judgeCountText[0].text = perfectnotes.ToString();
            judgeCountText[1].text = greatnotes.ToString();
            judgeCountText[2].text = goodnotes.ToString();
            judgeCountText[3].text = badnotes.ToString();
            judgeCountText[4].text = missnotes.ToString();
            EndGame();
            for (int i = 0; i < 2; i++)
            {
                JudgedNotes(i);
                //AssistVibrate(i);
                JudgeTextMove(i);
                Effect(i);
            }
        }
        PauseGame();
	}


    public void StartGame()
    {
        //startButton.SetActive(false);
        _startTime = Time.time;
        _audioSource.Play();
        _isPlaying = true;
    }

    void CheckNextNotes()
    {
        if (_timing.Count <= _notesCount)
        {
            return;
        }
        while (_timing[_notesCount] + timeOffset < GetMusicTime() && _timing[_notesCount] != 0)
        {
            spawnNotes(_lineNum[_notesCount]);
            _notesCount++;
            if(_timing.Count <= _notesCount)
            {
                break;
            }
        }
    }

    void spawnNotes(int num)
    {
        Debug.Log("spawnNotes");
        Instantiate(notes[num], new Vector3(-1.0f + (2.0f * num), 10.0f, 0.0f), Quaternion.identity);
    }

    void LoadCSV()
    {
        int i = 0;
        TextAsset csv = Resources.Load(filePass) as TextAsset;
        Debug.Log(filePass);
        StringReader reader = new StringReader(csv.text);
        while (reader.Peek() > -1)
        {
            string line = reader.ReadLine();
            string[] values = line.Split(',');
            float time = float.Parse(values[0]);
            int num = int.Parse(values[1]);
            _timing.Add(time);
            _lineNum.Add(num);
            Debug.Log(time);
            i++;
        }
    }

    float GetMusicTime()
    {
        return Time.time - _startTime;
    }

    public void GoodTimingFunc(int num)
    {
        Debug.Log("Line:" + num + " good!");
        //Debug.Log(GetMusicTime());
        goodnotes++;
        _score += notesPoint[2];
        comboflag = true;
        ComboJudge();
        IndicateJudgeText(num, "GOOD");
        judgeText[num].color = new Color(85f/255f, 255f/255f, 0f/255f);
        effectFlag[num] = 2;
        EffectColor(num,"good");
    }
    public void GreatTimingFunc(int num)
    {
        Debug.Log("Line:" + num + " great!");
        //Debug.Log(GetMusicTime());
        greatnotes++;
        _score += notesPoint[1];
        comboflag = true;
        ComboJudge();
        IndicateJudgeText(num, "GREAT");
        judgeText[num].color = new Color(255f / 255f, 120f / 255f, 0f / 255f);
        effectFlag[num] = 2;
        EffectColor(num, "great");
    }
    public void PerfectTimingFunc(int num)
    {
        Debug.Log("Line:" + num + " perfect!");
        //Debug.Log(GetMusicTime());
        perfectnotes++;
        _score += notesPoint[0];
        comboflag = true;
        ComboJudge();
        IndicateJudgeText(num, "PERFECT");
        judgeText[num].color = new Color(255f / 255f, 255f / 255f, 0 / 255f);
        effectFlag[num] = 2;
        EffectColor(num, "perfect");
    }
    public void BadTimingFunc(int num)
    {
        Debug.Log("Line:" + num + " bad!");
        badnotes++;
        _score += notesPoint[3];
        //Debug.Log(GetMusicTime());
        comboflag = false;
        ComboJudge();
        IndicateJudgeText(num, "BAD");
        judgeText[num].color = new Color(0 / 255f, 210f / 255f, 255f / 255f);
        effectFlag[num] = 2;
        EffectColor(num, "bad");
    }
    public void MissTimingFunc(int num)
    {
        Debug.Log(" miss!");
        missnotes++;
        //Debug.Log(GetMusicTime());
        comboflag = false;
        ComboJudge();
        disappearedNotesCount++;
        IndicateJudgeText(num, "MISS");
        judgeText[num].color = new Color(50f / 255f, 50f / 255f, 50f / 255f);
    }

    void IndicateJudgeText(int num, string judge)
    {
        judgeText[num].text = judge;
        if (num == 0)
        {
            judgeText[num].rectTransform.localPosition = new Vector3(-30, -80);
            judgeTextTime[num] = Time.time;
        }
        if(num == 1)
        {
            judgeText[num].rectTransform.localPosition = new Vector3(30, -80);
            judgeTextTime[num] = Time.time;
        }
    }

    void JudgeTextMove(int num)
    {
        if (judgeText[num].rectTransform.localPosition.y <= -70)
        {
            judgeText[num].rectTransform.localPosition += new Vector3(0, 2);
        }
        if(Time.time - judgeTextTime[num] > judgeTextDestroyTime)
        {
            judgeText[num].text = "";
        }
    }

    public int JudgeMinNotes(int line)
    {
        float notesMinTiming = 999.9f;
        int minNotesNum = -1;
        int startnotes = 0;
        startnotes = disappearedNotesCount;
        for (int i = startnotes; i < _notesCount; i++)
        {
            if (_lineNum[i] == line && Math.Abs(this.GetMusicTime() - _timing[i]) <= notesMinTiming)
            {
                notesMinTiming = Math.Abs(this.GetMusicTime() - _timing[i]);
                minNotesNum = i;
            }
        }

        return minNotesNum;
    }

    public float MaxNotesTiming()
    {
        float notesMaxTiming = 0;
        int startnotes = 0;
        for (int i = startnotes; i < _notesCount; i++)
        {
            if (_timing[i] > notesMaxTiming)
            {
                notesMaxTiming = _timing[i];
            }
        }
        return notesMaxTiming;
    }

    public float GetNotesMinTiming(int line, int notesNum)  //ノーツの判定タイミングと現在時間の差
    {
        //Debug.Log(notesNum);
        if (notesNum == -1)
        {
            return 999.9f;
        }
        if (_lineNum[notesNum] == line)
        {
        return this.GetMusicTime() - _timing[notesNum];
        }
        else
        {
            return 999.9f;
        }
    }

    void ComboJudge()
    {
        if (comboflag)
        {
            comboCount++;
            if (maxCombocount < comboCount)
            {
                maxCombocount = comboCount;
            }
            comboText1.text = comboCount.ToString();
            comboText2.text = "COMBO";
        }
        if (comboflag == false)
        {
            comboCount = 0;
            comboText1.text = "";
            comboText2.text = "";
        }
    }

    void JudgedNotes(int lineNum)
    {
        int notesNum = JudgeMinNotes(lineNum);
        float timing = GetNotesMinTiming(lineNum, notesNum);
        float absTiming = System.Math.Abs(timing);
        bool gyroL = mgc.GetgyroL();
        bool gyroR = mgc.GetgyroR();
        bool isPushingL = mgc.GetisPushingL();
        bool isPushingR = mgc.GetisPushingR();

        if(notesNum == -1)
        {
            return;
        }

        if ((((Input.GetKeyDown(KeyCode.F) || gyroL || isPushingL) && _lineNum[notesNum] == 0) ||
            ((Input.GetKeyDown(KeyCode.J) || gyroR || isPushingR) && _lineNum[notesNum] == 1)))
        {
            if (absTiming < perfectTiming)
            {
                PerfectTimingFunc(lineNum);
                destroyNotesNum=notesNum;
                disappearedNotesCount++;
                if (gyroL && lineNum == 0)
                {
                    mgc.SetvibrateL(true);
                }
                else if (gyroR && lineNum == 1)
                {
                    mgc.SetvibrateR(true);
                }
            }
            else if (absTiming < greatTiming)
            {
                GreatTimingFunc(lineNum);
                destroyNotesNum = notesNum;
                disappearedNotesCount++;
                if (gyroL && lineNum == 0)
                {
                    mgc.SetvibrateL(true);
                }
                else if (gyroR && lineNum == 1)
                {
                    mgc.SetvibrateR(true);
                }
            }
            else if (absTiming < goodTiming)
            {
                GoodTimingFunc(lineNum);
                destroyNotesNum = notesNum;
                disappearedNotesCount++;
            }
            else if (absTiming < badTiming)
            {
                BadTimingFunc(lineNum);
                destroyNotesNum = notesNum;
                disappearedNotesCount++;
            }
        }
    }

    void AssistVibrate(int lineNum)
    {
        int notesNum = JudgeMinNotes(lineNum);
        float timing = GetNotesMinTiming(lineNum, notesNum);
        if (Math.Abs(timing) < goodTiming && timing < 0)
        {
            if (lineNum == 0)
            {
                MainGameController.minVibrateL = true;
            }
            else if (lineNum == 1)
            {
                MainGameController.minVibrateR = true;
            }
        }
    }

    public int getDestroyNotesNum()
    {
        return destroyNotesNum;
    }

    void EffectColor(int lane, string judge)
    {
        if(judge == "perfect")
        {
            effectMaterial[lane].color = new Color(255f / 255f, 255f / 255f, 0f / 255f, 255f / 255f);
        }
        else if(judge == "great")
        {
            effectMaterial[lane].color = new Color(255f / 255f, 120f / 255f, 0f / 255f, 255f / 255f);
        }
        else if (judge == "good")
        {
            effectMaterial[lane].color = new Color(0f / 255f, 255f / 255f, 0f / 255f, 255f / 255f);
        }
        else if (judge == "bad")
        {
            effectMaterial[lane].color = new Color(0f / 255f, 0f / 255f, 255f / 255f, 255f / 255f);
        }
    }

    void Effect(int lane)
    {
        if (effectFlag[lane] >= 1)
        {
            if(effectFlag[lane] == 2)
            {
                effect[lane].transform.localScale = new Vector3(0.3f, 1f, 0.1f);
                effectFlag[lane] = 1;
            }
            effect[lane].transform.localScale += new Vector3(0.1f, 0, 0);
            effectMaterial[lane].color -= new Color(0/255f, 0/255f, 0/255f, 30f/255f);
        }
        if(effect[lane].transform.localScale.x > 1.0f)
        {
            effect[lane].transform.localScale = new Vector3(0.3f, 1f, 0.1f);
            effectMaterial[lane].color = new Color(255f / 255f, 120f / 255f, 0 / 255f, 0f / 255f);
            effectFlag[lane] = 0;
        }
    }

    void PauseGame()
    {
        if ((Input.GetKeyDown(KeyCode.Escape) || mgc.GetisPushingPlus()) && isPause == false)
        //if ((Input.GetKeyDown(KeyCode.Escape) ||  MainGameController.isPushingPlus) && isPause == false)
        {
            isPause = true;
            pauseTime = Time.time;
            _isPlaying = false;
            _audioSource.Pause();
            Debug.Log("Pause");
            pauseMenu.SetActive(true);
        }
        //else if((Input.GetKeyDown(KeyCode.Escape) || MainGameController.isPushingPlus) && isPause == true)
        else if ((Input.GetKeyDown(KeyCode.Escape) || mgc.GetisPushingPlus()) && isPause == true)
        {
            isPause = false;
            pauseTime = Time.time - pauseTime;
            _startTime += pauseTime;
            _audioSource.UnPause();
            _isPlaying = true;
            Debug.Log("Restart");
            pauseMenu.SetActive(false);
            selectMenu = 2;
        }

        if (isPause)
        {
            if (selectMenu == 2)
            {
                pauseText[0].color = new Color(0f / 255f, 0f / 255f, 0f / 255f);
                pauseText[1].color = new Color(0f / 255f, 0f / 255f, 0f / 255f);
                pauseText[2].color = new Color(255f / 255f, 0f / 255f, 0f / 255f);
            }
            else if (selectMenu == 1)
            {
                pauseText[0].color = new Color(0f / 255f, 0f / 255f, 0f / 255f);
                pauseText[1].color = new Color(255f / 255f, 0f / 255f, 0f / 255f);
                pauseText[2].color = new Color(0f / 255f, 0f / 255f, 0f / 255f);
            }
            else if (selectMenu == 0)
            {
                pauseText[0].color = new Color(255f / 255f, 0f / 255f, 0f / 255f);
                pauseText[1].color = new Color(0f / 255f, 0f / 255f, 0f / 255f);
                pauseText[2].color = new Color(0f / 255f, 0f / 255f, 0f / 255f);
            }
            //if (( Input.GetKeyDown(KeyCode.UpArrow) || MainGameController.isPushingUp) && selectMenu > 0){
            if (( Input.GetKeyDown(KeyCode.UpArrow) || mgc.GetisPushingUp()) && selectMenu > 0){
                selectMenu--;
            }
            //if ((Input.GetKeyDown(KeyCode.DownArrow) || MainGameController.isPushingDown) && selectMenu < 2)
            if ((Input.GetKeyDown(KeyCode.DownArrow) || mgc.GetisPushingDown()) && selectMenu < 2)
            {
                selectMenu++;
            }
            //if ((Input.GetKeyDown(KeyCode.Space) || MainGameController.isPushingA))
            if ((Input.GetKeyDown(KeyCode.Space) || mgc.GetisPushingA()))
            {
                if(selectMenu == 2)
                {
                    isPause = false;
                    pauseTime = Time.time - pauseTime;
                    _startTime += pauseTime;
                    _audioSource.UnPause();
                    _isPlaying = true;
                    Debug.Log("UnPause");
                    pauseMenu.SetActive(false);
                }
                else if(selectMenu == 1)
                {
                    resetNotes = true;
                    Debug.Log("return start");
                    SceneManager.LoadScene("Scene/MainGame");
                }
                else if(selectMenu == 0)
                {
                    resetNotes = true;
                    isPause = false;
                    _audioSource.Stop();
                    _isPlaying = true;
                    Debug.Log("return MusicMenu");
                    pauseMenu.SetActive(false);
                    SceneManager.LoadScene("Scene/Musicmenu");
                }
            }
        }
    }

    void NotesPoint()
    {
        notesPoint[0] = 1000000f / _timing.Count;
        notesPoint[1] = notesPoint[0] * 0.8f;
        notesPoint[2] = notesPoint[0] * 0.6f;
        notesPoint[3] = notesPoint[0] * 0.2f;
    }

    void EndGame()
    {
        float maxNotesTime = MaxNotesTiming(); 
        if(_timing.Count <= _notesCount && this.GetMusicTime() > maxNotesTime + 5.0f)
        {
            _isPlaying = false;
            ResultManager.score = (int)_score;
            ResultManager.missNotes = missnotes;
            ResultManager.badNotes = badnotes;
            ResultManager.goodNotes = goodnotes;
            ResultManager.greatNotes = greatnotes;
            ResultManager.perfectNotes = perfectnotes;
            ResultManager.maxCombo = maxCombocount;
            SceneManager.LoadScene("Scene/Result");

        }
    }
}
