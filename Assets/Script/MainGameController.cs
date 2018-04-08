using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class MainGameController : MonoBehaviour {
    public static string csvFilePass;
    public static string musicPass;
    private static bool created = false;

    public float vibrateNum = 8.0f;

    private float gyroLPre = 0;
    private float gyroRPre = 0;

    private List<Joycon> m_joycons;
    private Joycon m_joyconL;
    private Joycon m_joyconR;
    public static bool gyroL = false;
    public static bool gyroR = false;
    public static bool isPushingL = false;
    public static bool isPushingR = false;
    public static bool isPushingLZ = false;
    public static bool isPushingRZ = false;

    public static bool isPushingA = false;
    public static bool isPushingB = false;
    public static bool isPushingX = false;
    public static bool isPushingY = false;
    public static bool isPushingRight = false;
    public static bool isPushingLeft = false;
    public static bool isPushingUp = false;
    public static bool isPushingDown = false;

    public static bool isPushingMinus = false;
    public static bool isPushingPlus = false;

    public static bool isPushingHome = false;

    public static bool isPushingAny = false;

    public static bool vibrateL = false;
    public static bool vibrateR = false;

    public static bool minVibrateL = false;
    public static bool minVibrateR = false;
    // Use this for initialization
    void Start () {
        if (!created)
        {
            DontDestroyOnLoad(this.gameObject);
            created = true;
        }else
        {
            Destroy(this.gameObject);
        }
        if (m_joycons == null)
        {
            m_joycons = JoyconManager.Instance.j;
        }
        if (m_joycons != null && m_joycons.Count > 0)
        {
            m_joyconL = m_joycons.Find(c => c.isLeft);
            m_joyconR = m_joycons.Find(c => !c.isLeft);
        }
    }

    // Update is called once per frame
    void Update () {

        if (m_joycons != null && m_joycons.Any(c => c.isLeft) && m_joycons.Any(c => !c.isLeft))
        {
            inputJoyconButton();
            gyroJudge();
            VibrateL();
            VibrateR();
        }
    }

    void inputJoyconButton()
    {
        if (m_joycons != null && m_joycons.Count > 0)
        {
            isPushingA = m_joyconR.GetButtonDown(Joycon.Button.DPAD_RIGHT);
            isPushingB = m_joyconR.GetButtonDown(Joycon.Button.DPAD_DOWN);
            isPushingX = m_joyconR.GetButtonDown(Joycon.Button.DPAD_UP);
            isPushingY = m_joyconR.GetButtonDown(Joycon.Button.DPAD_LEFT);
            isPushingUp = m_joyconL.GetButtonDown(Joycon.Button.DPAD_UP);
            isPushingDown = m_joyconL.GetButtonDown(Joycon.Button.DPAD_DOWN);
            isPushingRight = m_joyconL.GetButtonDown(Joycon.Button.DPAD_RIGHT);
            isPushingLeft = m_joyconL.GetButtonDown(Joycon.Button.DPAD_LEFT);
            isPushingLZ = m_joyconL.GetButtonDown(Joycon.Button.SHOULDER_2);
            isPushingRZ = m_joyconR.GetButtonDown(Joycon.Button.SHOULDER_2);
            isPushingL = m_joyconL.GetButtonDown(Joycon.Button.SHOULDER_1);
            isPushingR = m_joyconR.GetButtonDown(Joycon.Button.SHOULDER_1);
            isPushingPlus = m_joyconR.GetButtonDown(Joycon.Button.PLUS);
            isPushingMinus = m_joyconL.GetButtonDown(Joycon.Button.MINUS);
            isPushingHome = m_joyconR.GetButtonDown(Joycon.Button.HOME);

            if (isPushingA || isPushingB || isPushingX || isPushingY
                || isPushingUp || isPushingDown || isPushingRight
                || isPushingLeft || isPushingLZ || isPushingRZ
                || isPushingL || isPushingR || isPushingPlus || isPushingMinus)
            {
                isPushingAny = true;
            }
            else
            {
                isPushingAny = false;
            }
        }
    }

    void gyroJudge()
    {
        if (m_joyconL.GetGyro().z > vibrateNum && !gyroL)
        {
            
            if (gyroLPre < vibrateNum)
            {
                gyroL = true;
            }
            else
            {
                gyroL = false;
            }
            
        }
        else
        {
            gyroL = false;
        }

        if (m_joyconR.GetGyro().z < -vibrateNum && !gyroR)
        {
            if (gyroRPre > -vibrateNum)
            {
                gyroR = true;
            }
            else
            {
                gyroR = false;
            }
        }
        else
        {
            gyroR = false;
        }
        gyroLPre = m_joyconL.GetGyro().z;
        gyroRPre = m_joyconR.GetGyro().z;
    }

    public void SetcsvFilePass(string csvFile)
    {
        csvFilePass = csvFile;
    }
    public void SetmusicPass(string music)
    {
        musicPass = music;
    }

    public string GetcsvFilePass()
    {
        return csvFilePass;
    }
    public string GetmusicPass()
    {
        return musicPass;
    }

    public void VibrateL()
    {
        if (vibrateL)
        {
            m_joyconL.SetRumble(80, 160, 0.6f, 100);
            vibrateL = false;
        }
        else if (minVibrateL)
        {
            m_joyconL.SetRumble(20, 40, 0.6f, 50);
            minVibrateL = false;
        }
    }
    public void VibrateR()
    {
        if (vibrateR)
        {
            m_joyconR.SetRumble(80, 160, 0.6f, 100);
            vibrateR = false;
        }
        else if (minVibrateR)
        {
            m_joyconR.SetRumble(40, 80, 0.6f, 80);
            minVibrateR = false;
        }
    }

    public bool GetisPushingA()
    {
        return isPushingA;
    }
    public bool GetisPushingB()
    {
        return isPushingB;
    }
    public bool GetisPushingX()
    {
        return isPushingX;
    }
    public bool GetisPushingY()
    {
        return isPushingY;
    }
    public bool GetisPushingUp()
    {
        return isPushingUp;
    }
    public bool GetisPushingDown()
    {
        return isPushingDown;
    }
    public bool GetisPushingRight()
    {
        return isPushingRight;
    }
    public bool GetisPushingLeft()
    {
        return isPushingLeft;
    }
    public bool GetisPushingHome()
    {
        return isPushingHome;
    }
    public bool GetisPushingPlus()
    {
        return isPushingPlus;
    }
    public bool GetisPushingminus()
    {
        return isPushingMinus;
    }
    public bool GetisPushingR()
    {
        return isPushingR;
    }
    public bool GetisPushingL()
    {
        return isPushingL;
    }
    public bool GetisPushingLZ()
    {
        return isPushingLZ;
    }
    public bool GetisPushingRZ()
    {
        return isPushingRZ;
    }
    public bool GetgyroL()
    {
        return gyroL;
    }
    public bool GetgyroR()
    {
        return gyroR;
    }

    public void SetvibrateL(bool vibrate)
    {
        vibrateL = vibrate;
    }
    public void SetvibrateR(bool vibrate)
    {
        vibrateR = vibrate;
    }
}
