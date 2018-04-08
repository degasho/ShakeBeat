using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (MainGameController.isPushingHome || Input.GetKeyDown(KeyCode.Escape))
        {
            #if UNITY_EDITOR
                    UnityEditor.EditorApplication.isPlaying = false;
            #else
		            Application.Quit();
            #endif
        }
        else if (MainGameController.isPushingAny || Input.anyKeyDown)
        {
            SceneManager.LoadScene("Scene/Musicmenu");
        }
    }
}
