using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwingImageScript : MonoBehaviour {
    public Image leftImage;
    public Image rightImage;
    private double angleL = 0;
    private double angleR = 0;
    private float totalaAngleL = 0;
    private float totalaAngleR = 0;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if ((Input.GetKeyDown(KeyCode.F) || MainGameController.gyroL || MainGameController.isPushingL))
        {
            angleL = 0.1;
            leftImage.rectTransform.Rotate(0, 0, -totalaAngleL);
            totalaAngleL = 0;
        }
        if (angleL > 0)
        {
            angleL += 20.0;
            leftImage.rectTransform.Rotate(0,0,8.0f * (float)System.Math.Sin(angleL * System.Math.PI/180));
            totalaAngleL += 8.0f * (float)System.Math.Sin(angleL * System.Math.PI / 180);
        }
        if (angleL > 360)
        {
            Debug.Log("swingEnd L");
            angleL = 0;
            leftImage.rectTransform.Rotate(0, 0, -totalaAngleL);
            totalaAngleL = 0;
        }

        if ((Input.GetKeyDown(KeyCode.J) || MainGameController.gyroR || MainGameController.isPushingR))
        {
            angleR = -0.1;
            rightImage.rectTransform.Rotate(0, 0, -totalaAngleR);
            totalaAngleR = 0;
        }
        if (angleR < 0)
        {
            angleR -= 20.0;
            rightImage.rectTransform.Rotate(0, 0, 8.0f * (float)System.Math.Sin(angleR * System.Math.PI / 180));
            totalaAngleR += 8.0f * (float)System.Math.Sin(angleR * System.Math.PI / 180);
        }
        if(angleR < -360)
        {
            angleR = 0;
            rightImage.rectTransform.Rotate(0, 0, -totalaAngleR);
            totalaAngleR = 0;
        }
    }
}
