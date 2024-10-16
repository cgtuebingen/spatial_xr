using UnityEngine;
using TMPro;
using System;

public class ClockController : MonoBehaviour
{
    public TextMeshProUGUI yearTextSecond;
    public TextMeshProUGUI monthTextSecond;
    public TextMeshProUGUI dayTextSecond;
    public TextMeshProUGUI hourTextSecond;
    public TextMeshProUGUI minuteTextSecond;

    public ClockInteraction clockInteraction;
    public bool isTimeChangedManually = false;

    private DateTime lastUpdateTime;

    void Start()
    {
        // Setze die letzte Update-Zeit auf den aktuellen Zeitpunkt
        lastUpdateTime = DateTime.Now;
        UpdateClock();
    }

    void Update()
    {
        // Überprüfen, ob eine Minute seit dem letzten Update vergangen ist
        if ((DateTime.Now - lastUpdateTime).TotalMinutes >= 1)
        {
            UpdateClock();
            lastUpdateTime = DateTime.Now;  // Setze die letzte Update-Zeit auf jetzt
        }

        
    }

    void UpdateClock()
    {
        DateTime now = DateTime.Now;

        // Aktualisiere die Textfelder mit dem aktuellen Datum und der Uhrzeit
        yearTextSecond.text = now.Year.ToString();
        monthTextSecond.text = now.Month.ToString();
        dayTextSecond.text = now.Day.ToString();
        hourTextSecond.text = now.Hour.ToString();
        minuteTextSecond.text = now.Minute.ToString();

        Debug.Log("Uhr aktualisiert: " + now.ToString("yyyy-MM-dd HH:mm"));
    }
   
   /*
    void KeepTimeInRange()
    {
        // Keep year values between 0 and 9
        yearTextFirst.text = (int.Parse(yearTextFirst.text) % 10).ToString();
         if (int.Parse(yearTextFirst.text) < 0)
            {
                yearTextFirst.text = "0";
            }
        
        yearTextSecond.text = (int.Parse(yearTextSecond.text) % 10).ToString();

        if (int.Parse(yearTextSecond.text) < 0)
            {
                yearTextSecond.text = "0";
            }

        // Ensure month values are valid (1-12)
        monthTextFirst.text = (int.Parse(monthTextFirst.text) % 2).ToString();
        if (int.Parse(monthTextFirst.text) == 1)
        {
            monthTextSecond.text = (int.Parse(monthTextSecond.text) % 3).ToString();
        }
        else
        {
            monthTextSecond.text = (int.Parse(monthTextSecond.text) % 10).ToString();
            if (int.Parse(monthTextSecond.text) == 0 || int.Parse(monthTextSecond.text) < 0)
            {
                monthTextSecond.text = "1";
            }
        }

        if (int.Parse(monthTextFirst.text) < 0)
            {
                monthTextFirst.text = "1";
            }

        if (int.Parse(monthTextSecond.text) < 0)
            {
                monthTextSecond.text = "0";
            }

        // Ensure day values are valid (1-31, adjusted for month and leap year)
        dayTextFirst.text = (int.Parse(dayTextFirst.text) % 4).ToString();
        int dayFirstDigit = int.Parse(dayTextFirst.text);
        int month = int.Parse(monthTextFirst.text + monthTextSecond.text);
        int year = int.Parse("20" + yearTextFirst.text + yearTextSecond.text);
        
        if (dayFirstDigit == 2 && month == 2)
        {
            bool isLeapYear = (year % 4 == 0 && year % 100 != 0) || (year % 400 == 0);
            dayTextSecond.text = (int.Parse(dayTextSecond.text) % (isLeapYear ? 10 : 9)).ToString();
        }
        else if (dayFirstDigit == 3)
        {
            bool isThirtyDayMonth = month == 4 || month == 6 || month == 9 || month == 11;
            dayTextSecond.text = (isThirtyDayMonth && int.Parse(dayTextSecond.text) >= 1) ? "0" : "1";
        }
        else
        {
            dayTextSecond.text = (int.Parse(dayTextSecond.text) % 10).ToString();
            if (dayFirstDigit == 0 && int.Parse(dayTextSecond.text) == 0)
            {
                dayTextSecond.text = "1";
            }
        }

        if (int.Parse(dayTextFirst.text) < 0)
            {
                dayTextFirst.text = "0";
            }

        if (int.Parse(dayTextSecond.text) < 0)
            {
                dayTextSecond.text = "1";
            }

        // Ensure hour values are valid (0-23)
        hourTextFirst.text = (int.Parse(hourTextFirst.text) % 3).ToString();
        if (int.Parse(hourTextFirst.text) == 2)
        {
            hourTextSecond.text = (int.Parse(hourTextSecond.text) % 4).ToString();
        }
        else
        {
            hourTextSecond.text = (int.Parse(hourTextSecond.text) % 10).ToString();
        }

        if (int.Parse(hourTextFirst.text) < 0)
            {
                hourTextFirst.text = "0";
            }

        if (int.Parse(hourTextSecond.text) < 0)
            {
                hourTextSecond.text = "0";
            }
        // Ensure minute values are valid (0-59)
        minuteTextFirst.text = (int.Parse(minuteTextFirst.text) % 6).ToString();
        
        if (int.Parse(minuteTextFirst.text) < 0)
            {
                minuteTextFirst.text = "0";
            }

        minuteTextSecond.text = (int.Parse(minuteTextSecond.text) % 10).ToString();

        if (int.Parse(minuteTextSecond.text) < 0)
            {
                minuteTextSecond.text = "0";
            }


    }

    */
}
