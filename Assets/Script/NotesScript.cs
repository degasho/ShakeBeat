using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;

public class NotesScript : MonoBehaviour {
    public int lineNum;                     //レーンの判別

    public float goodTiming;                //判定の猶予
    public float greatTiming;
    public float perfectTiming;
    public float badTiming;

    private static int notesCount = 0;      //現在何番目のノーツまで作られたかが分かる
    private int notesNum;                   //ノーツ番号。n番目のノーツであることが分かる
    private GameManager gameManager;

    // Use this for initialization
    void OnEnable()
    {
        notesNum = notesCount;
        notesCount++;
        //Debug.Log(notesNum);
    }

    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }
	
	// Update is called once per frame
	void Update () {
        this.transform.position += Vector3.down * 10 * Time.deltaTime;
        JudgedNotes();
        AssistVibrate();
        if (this.transform.position.y < -5.0f)
        {
            Debug.Log("false");
            Destroy(this.gameObject);
            gameManager.MissTimingFunc(lineNum);
        }
        if (gameManager.lastNotes <= notesCount)
        {
            initCount();
        }
    }

    void AssistVibrate()
    {
        float timing = gameManager.GetNotesMinTiming(lineNum, notesNum);
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

    void JudgedNotes()
    {
        float timing = gameManager.GetNotesMinTiming(lineNum, notesNum);
        float absTiming = System.Math.Abs(timing);
        bool gyroL = MainGameController.gyroL;
        bool gyroR = MainGameController.gyroR;
        bool isPushingL = MainGameController.isPushingL;
        bool isPushingR = MainGameController.isPushingR;

        /* if ((((Input.GetKeyDown(KeyCode.F) || gyroL || isPushingL) && lineNum == 0) ||
             ((Input.GetKeyDown(KeyCode.J) || gyroR || isPushingR) && lineNum == 1)) &&
             gameManager.JudgeMinNotes(lineNum, notesNum))
             */
        if ((((Input.GetKeyDown(KeyCode.F) || gyroL || isPushingL) && lineNum == 0) ||
            ((Input.GetKeyDown(KeyCode.J) || gyroR || isPushingR) && lineNum == 1)))
        {      
            if (absTiming < perfectTiming)
            {
                gameManager.PerfectTimingFunc(lineNum);
                Destroy(this.gameObject);
                if (gyroL && lineNum == 0)
                {
                    MainGameController.vibrateL = true;
                }
                else if (gyroR && lineNum == 1)
                {
                    MainGameController.vibrateR = true;
                }
            }
            else if (absTiming < greatTiming)
            {
                gameManager.GreatTimingFunc(lineNum);
                Destroy(this.gameObject);
                if (gyroL && lineNum == 0)
                {
                    MainGameController.vibrateL = true;
                }
                else if (gyroR && lineNum == 1)
                {
                    MainGameController.vibrateR = true;
                }
            }
            else if (absTiming < goodTiming)
            {
                gameManager.GoodTimingFunc(lineNum);
                Destroy(this.gameObject);
            }
            else if (absTiming < badTiming)
            {
                gameManager.BadTimingFunc(lineNum);
                Destroy(this.gameObject);
            }
        }
    }
    void initCount()
    {
        notesCount = 0;
    }

}
