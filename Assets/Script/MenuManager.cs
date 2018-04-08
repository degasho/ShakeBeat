using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;
using System;
using System.Linq;

public class MenuManager : MonoBehaviour {
    private int selectmusic = 0;
    private int selectLevel = 0;//0:easy 1:nomal 2:hard
    public Text musicText;
    public Text subUpMusicText1;
    public Text subUpMusicText2;
    public Text subDownMusicText1;
    public Text subDownMusicText2;
    //public Text levelText;
    public Image levelSelectImage;
    public string infoCsvPass;
    List<Musics> musicsList = new List<Musics>();
    private List<AudioSource> _audioSource = new List<AudioSource>();

    public Image musicImage;
    private List<Texture2D> musicImageList = new List<Texture2D>();

    private MainGameController mgc = new MainGameController();

    // Use this for initialization
    void Start () {
        LoadCSV();
        foreach (Musics music in musicsList ) {
            _audioSource.Add(GameObject.Find(music.audioSourceName).GetComponent<AudioSource>());
            musicImageList.Add(Resources.Load(music.musicImage) as Texture2D);
        }
        _audioSource[selectmusic].Play();
    }
	
	// Update is called once per frame
	void Update () {
        Debug.Log(selectmusic);
        Debug.Log(selectLevel);
        MainGameController.musicPass = musicsList[selectmusic].audioSourceName;
        Debug.Log(MainGameController.musicPass);
        //if (Input.GetKeyDown(KeyCode.Space) || MainGameController.isPushingA)
        if (Input.GetKeyDown(KeyCode.Space) || mgc.GetisPushingA())
        {
            _audioSource[selectmusic].Stop();
            SceneManager.LoadScene("Scene/MainGame");
        }
        //if (Input.GetKeyDown(KeyCode.B) || MainGameController.isPushingB)
        if (Input.GetKeyDown(KeyCode.B) || mgc.GetisPushingB())

        {
            _audioSource[selectmusic].Stop();
            SceneManager.LoadScene("Scene/Title");
        }
        if (Input.GetKeyDown(KeyCode.UpArrow) || mgc.GetisPushingUp())
        //if (Input.GetKeyDown(KeyCode.UpArrow) || MainGameController.isPushingUp)
        {
            _audioSource[selectmusic].Stop();
            selectmusic--;
            if (selectmusic < 0)
            {
                selectmusic = musicsList.Count - 1;
            }

            _audioSource[selectmusic].Play();
            musicImage.sprite = Sprite.Create(musicImageList[selectmusic], musicImage.sprite.rect, musicImage.sprite.pivot);
        }
        if (Input.GetKeyDown(KeyCode.DownArrow) || mgc.GetisPushingDown())
        //if (Input.GetKeyDown(KeyCode.DownArrow) || MainGameController.isPushingDown)
        {
            _audioSource[selectmusic].Stop();
            selectmusic++;
            if (musicsList.Count <= selectmusic)
            {
                selectmusic = 0;
            }

            _audioSource[selectmusic].Play();
            musicImage.sprite = Sprite.Create(musicImageList[selectmusic], musicImage.sprite.rect, musicImage.sprite.pivot);
            
        }
        //if (Input.GetKeyDown(KeyCode.RightArrow) || MainGameController.isPushingRight)
        if (Input.GetKeyDown(KeyCode.RightArrow) || mgc.GetisPushingRight())
        {
            selectLevel++;
            if (selectLevel > 2)
            {
                selectLevel = 2;
            }
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow) || mgc.GetisPushingLeft())
        //if (Input.GetKeyDown(KeyCode.LeftArrow) || MainGameController.isPushingLeft)
        {
            selectLevel--;
            if (selectLevel < 0)
            {
                selectLevel = 0;
            }
        }
        if (selectLevel == 0)
        {
            MainGameController.csvFilePass = musicsList[selectmusic].easyScore;
            //musicText.text = musicsList[selectmusic].musicTitle + ": Easy";
            musicText.text = musicsList[selectmusic].musicTitle;
            levelSelectImage.rectTransform.localPosition = new Vector3(60,-130);
            //levelText.text = "Easy";
        }
        else if (selectLevel == 1)
        {
            MainGameController.csvFilePass = musicsList[selectmusic].nomalScore;
            //musicText.text = musicsList[selectmusic].musicTitle + ": Nomal";
            musicText.text = musicsList[selectmusic].musicTitle;
            levelSelectImage.rectTransform.localPosition = new Vector3(190, -130);
            
            //levelText.text = "Nomal";
        }
        else if (selectLevel == 2)
        {
            MainGameController.csvFilePass = musicsList[selectmusic].hardScore;
            //musicText.text = musicsList[selectmusic].musicTitle + ": Hard";
            musicText.text = musicsList[selectmusic].musicTitle;
            levelSelectImage.rectTransform.localPosition = new Vector3(330, -130);
            //levelText.text = "Hard";
        }

        if (selectmusic - 1 >= 0)
        {
            subUpMusicText1.text = musicsList[selectmusic - 1].musicTitle;
        }
        else
        {
            subUpMusicText1.text = musicsList[musicsList.Count()-1].musicTitle;
        }
        if (selectmusic - 2 >= 0)
        {
            subUpMusicText2.text = musicsList[selectmusic - 2].musicTitle;
        }
        else
        {
            subUpMusicText2.text = musicsList[musicsList.Count() - 2].musicTitle;
        }

        if (selectmusic + 1 < musicsList.Count())
        {
            subDownMusicText1.text = musicsList[selectmusic + 1].musicTitle;
        }
        else
        {
            subDownMusicText1.text = musicsList[0].musicTitle;
        }
        if (selectmusic + 2 < musicsList.Count())
        {
            subDownMusicText2.text = musicsList[selectmusic + 2].musicTitle;
        }
        else
        {
            subDownMusicText2.text = musicsList[1].musicTitle;
        }
    }



    void LoadCSV()
    {
        int i = 0;
        TextAsset csv = Resources.Load(infoCsvPass) as TextAsset;
        StringReader reader = new StringReader(csv.text);
        while (reader.Peek() > -1)
        {
            Musics music = new Musics();
            string line = reader.ReadLine();
            string[] values = line.Split(',');
            music.audioSourceName = values[0];
            music.musicTitle = values[1];
            music.easyScore = values[2];
            music.nomalScore = values[3];
            music.hardScore = values[4];
            music.musicImage = values[5];
            //CSVファイルから曲名と譜面データのcsvファイル名を獲得。
            musicsList.Add(music);
            i++;
        }
    }
}
