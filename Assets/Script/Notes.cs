using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;

public class Notes : MonoBehaviour {
    public int lineNum;                     //レーンの判別
    private static int notesCount = 0;      //現在何番目のノーツまで作られたかが分かる
    private int notesNum;                   //ノーツ番号。n番目のノーツであることが分かる
    private GameManager gameManager;

    // Use this for initialization
    void OnEnable()
    {

        //Debug.Log(notesNum);
    }

    void Start()
    {
        notesNum = notesCount;
        notesCount++;
        Debug.Log(notesCount);
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }
	
	// Update is called once per frame
	void Update () {
        if (gameManager.isPause == false)
        {
            this.transform.position += Vector3.down * 10 * Time.deltaTime;
        }
        if (this.transform.position.y < -5.0f)
        {
            Debug.Log("false");
            Destroy(this.gameObject);
            gameManager.MissTimingFunc(lineNum);
        }
        if (gameManager.lastNotes <= notesCount || GameManager.resetNotes )
        {
            initCount();
        }
        DestroyNotes();
    }

    void DestroyNotes()
    {
        if(gameManager.getDestroyNotesNum() >= notesNum)
        {
            Destroy(this.gameObject);
        }
        
    }

    void initCount()
    {
        notesCount = 0;
    }

}
