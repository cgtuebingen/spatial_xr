using UnityEngine;
using TMPro;
using System;

public class ClockController : MonoBehaviour
{
    public TextMeshProUGUI yearTextFirst;
    public TextMeshProUGUI yearTextSecond;
    public TextMeshProUGUI monthTextFirst;
    public TextMeshProUGUI monthTextSecond;
    public TextMeshProUGUI dayTextFirst;
    public TextMeshProUGUI dayTextSecond;
    public TextMeshProUGUI hourTextFirst;
    public TextMeshProUGUI hourTextSecond;
    public TextMeshProUGUI minuteTextFirst;
    public TextMeshProUGUI minuteTextSecond;
    
       void Start()
    {
        UpdateClock();
        InvokeRepeating("UpdateClock", 1f, 60f);
    
    }

    void UpdateClock()
    {
        DateTime now = DateTime.Now;

        yearTextFirst.text = (now.Year / 10 % 10).ToString();
        yearTextSecond.text = ((now.Year % 1000) % 10).ToString();
        monthTextFirst.text = (now.Month / 10).ToString();
        monthTextSecond.text = (now.Month % 10).ToString();
        dayTextFirst.text = (now.Day / 10).ToString();
        dayTextSecond.text = (now.Day % 10).ToString();
        hourTextFirst.text = (now.Hour / 10).ToString();
        hourTextSecond.text = (now.Hour % 10).ToString();
        minuteTextFirst.text = (now.Minute / 10).ToString();
        minuteTextSecond.text = (now.Minute % 10).ToString();
       
    }
}
