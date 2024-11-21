using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeController : MonoBehaviour
{
    public bool isCountDown = true; //trueの時は計測をする
    public float gameTime = 0;
    public bool isTimeOver = false; //trueの時はタイマー停止
    public float displayTime = 0;   //表示時間

    float times = 0;

    // Start is called before the first frame update
    void Start()
    {
        if (isCountDown)
        {
            displayTime = gameTime;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isTimeOver == false)
        {
            times += Time.deltaTime;
            if (isCountDown) 
            { 
                displayTime = gameTime - times;
                if (displayTime <= 0.0f)
                {
                    displayTime = 0.0f;
                    isTimeOver = true;
                }
            }
            else
            {
                displayTime = times;
                if (displayTime >= gameTime)
                {
                    displayTime = gameTime;
                    isTimeOver = true;
                }
            }
            //Debug.Log("TIMES:"+displayTime);
        }

    }
}
