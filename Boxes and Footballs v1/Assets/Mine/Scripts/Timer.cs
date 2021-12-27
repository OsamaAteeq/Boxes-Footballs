using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    public float minutes = 5;

    public int fake_seconds = 0;
    public int fake_minutes = 90;
    public string display = "90:00";

    public bool reducing = false;
    private bool closing = false;
    private bool should_reduce = true;
    private int fake2;

    void Start()
    {
        fake2 = fake_minutes;
        GetComponent<TextMeshProUGUI>().text = display;
    }
    void Update()
    {
        if (!reducing && fake_minutes + fake_seconds != 0 && should_reduce)
        {
            StartCoroutine(StopWatch());
        }
        else if (!closing && (fake_minutes + fake_seconds == 0)) 
        {
            closing = true;
            StartCoroutine(waitBeforeEnd());
        }
    }

    public string stop()
    {
        should_reduce = false;
        return display;
    }

    public void resume(int minute, int seconds) 
    {
        this.fake_minutes = minute;
        this.fake_seconds = seconds;

        should_reduce = true;
    }

    private IEnumerator waitBeforeEnd()
    {
        GetComponent<TextMeshProUGUI>().GetComponent<DisplayScore>().display();
        yield return new WaitForSeconds(15);
        Application.Quit();
        
    }

    IEnumerator StopWatch() 
    {
        //calculations
        float ratio = (float)minutes / (float)fake2;
        reducing = true;
        
        yield return new WaitForSeconds(ratio);
        if (fake_seconds == 0 && fake_minutes == 0)
        {
            reducing = false;
        }
        else if (fake_seconds == 0)
        {
            fake_minutes--;
            fake_seconds = 60;
        }
        else 
        {
            fake_seconds--;
        }
        //calculations

        //display
        if (fake_minutes < 10 && fake_seconds<10)
        {
            display = "0" + fake_minutes + ":0" + fake_seconds;
        }
        else if (fake_minutes < 10)
        {
            display = "0" + fake_minutes + ":" + fake_seconds;
        }
        else if (fake_seconds < 10)
        {
            display = "" + fake_minutes + ":0" + fake_seconds;
        }
        else 
        {
            display = "" + fake_minutes + ":" + fake_seconds;
        }
        TextMeshProUGUI tmp = GetComponent<TextMeshProUGUI>();
        tmp.text = display;
        reducing = false;
        //display
    }
}
