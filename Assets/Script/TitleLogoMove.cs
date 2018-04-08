using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class TitleLogoMove : MonoBehaviour {
    public bool rightSpin = false;
    public bool leftSpin = false;
    public bool ballmove = false;
    public float r = 40.0f;
    public float theta = 0;

    // Use this for initialization
    void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
        if (ballmove)
        {
            theta %= 360;
            float x = r * (float)System.Math.Sin(theta);
            float y = 10 + r * (float)System.Math.Cos(theta);
            this.transform.position = new Vector3(x, y, -3);
            theta+=0.01f;
        }
        else if (rightSpin)
        {
                this.transform.Rotate(new Vector3(0, 0, -1));
        }
        else if(leftSpin)
        {
                this.transform.Rotate(new Vector3(0, 0, 1));
        }
    }

}
