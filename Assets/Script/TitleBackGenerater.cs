using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleBackGenerater : MonoBehaviour {
    public GameObject image;
    public GameObject canvas;
    public Image imageprefab;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (Random.Range(0, 31) % 30 == 0)
        {
            imageprefab.color = new Color(Random.Range(0, 256.0f) / 255f, Random.Range(0, 256.0f) / 255f, Random.Range(0, 256.0f) / 255f);
            GameObject prefab = Instantiate(image, new Vector3(911, Random.Range(-300, 300)), Quaternion.identity);
            prefab.transform.SetParent(canvas.transform, false);
        }
    }
}
