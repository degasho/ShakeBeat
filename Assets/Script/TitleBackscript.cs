using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleBackscript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        this.transform.localPosition += new Vector3(-30, 0);
        if (this.transform.localPosition.x <= -911)
        {
            Destroy(this.gameObject);
        }
    }
}
