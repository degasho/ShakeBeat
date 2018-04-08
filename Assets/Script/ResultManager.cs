using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ResultManager : MonoBehaviour {
    public static int score;
    public static int perfectNotes;
    public static int greatNotes;
    public static int goodNotes;
    public static int badNotes;
    public static int missNotes;
    public static int maxCombo;
    public Text scoreText;
    public Text perfectText;
    public Text greatText;
    public Text goodText;
    public Text badText;
    public Text missText;
    public Text comboText;
    public Text rankText;
    // Use this for initialization
    void Start () {
        scoreText.text = score.ToString();
        perfectText.text = perfectNotes.ToString();
        greatText.text = greatNotes.ToString();
        goodText.text = goodNotes.ToString();
        badText.text = badNotes.ToString();
        missText.text = missNotes.ToString();
        comboText.text = maxCombo.ToString();
        JudgedRank();
}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Space) || MainGameController.isPushingA)
        {
            initScores();
            SceneManager.LoadScene("Scene/Musicmenu");
        }
	}

    public void initScores()
    {
        score = 0;
        perfectNotes = 0;
        greatNotes = 0;
        goodNotes = 0;
        badNotes = 0;
        missNotes = 0;
        maxCombo = 0;
    }

    public void JudgedRank()
    {
        if(score > 1000000)
        {
            rankText.text = "S+";
        }
        else if(score > 900000)
        {
            rankText.text = "S";
        }
        else if (score > 850000)
        {
            rankText.text = "A";
        }
        else if (score > 700000)
        {
            rankText.text = "B";
        }
        else if (score > 500000)
        {
            rankText.text = "C";
        }
        else
        {
            rankText.text = "D";
        }
    }
}
