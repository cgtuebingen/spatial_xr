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

    public bool isTimeChangedManually = false;
    
       void Start()
    {

        
        UpdateClock();
    
        
    }

    void Update() 
    {
        if (!isTimeChangedManually) {
            InvokeRepeating("UpdateClock", 1f, 60f);
            
        } 
        else {
            KeepTimeInRange();

        }
        
    }
    void UpdateClock()
    {
        DateTime now = DateTime.Now;

        if (!isTimeChangedManually)  {
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

        void KeepTimeInRange()
    {
        // Jahr
        int yearFirstDigit = int.Parse(yearTextFirst.text) % 10;
        int yearSecondDigit = int.Parse(yearTextSecond.text) % 10;
        yearTextFirst.text = yearFirstDigit.ToString();
        yearTextSecond.text = yearFirstDigit.ToString();

        // Monat
        int monthFirstDigit = int.Parse(monthTextFirst.text) % 2;
        monthTextFirst.text = monthFirstDigit.ToString();
        if (monthFirstDigit == 1) // Monate 10, 11, 12
        {
            monthTextSecond.text = (int.Parse(monthTextSecond.text) % 10).ToString();
        }
        else
        {
            int monthSecondDigit = int.Parse(monthTextSecond.text) % 3; // Monate 1-9
            if (monthFirstDigit == 0 && monthSecondDigit == 0)
            {
                monthSecondDigit = 1;
            }
            monthTextSecond.text = monthSecondDigit.ToString();
        }

        // Tag
        int dayFirstDigit = int.Parse(yearTextFirst.text) % 4;
        dayTextFirst.text = dayFirstDigit.ToString();
        if (dayFirstDigit == 2 && int.Parse(monthTextSecond.text) == 2)
        {
            // Schaltjahrprüfung
            int year = int.Parse("20" + yearTextFirst.text + yearTextSecond.text);
            if ((year % 4 == 0 && year % 100 != 0) || (year % 400 == 0))
            {
                dayTextSecond.text = (int.Parse(monthTextSecond.text) % 9).ToString(); // Schaltjahr Februar Tage (29)
            }
            else
            {
                dayTextSecond.text = (int.Parse(monthTextSecond.text) % 8).ToString(); // Kein Schaltjahr Februar Tage (28)
            }
        }
        else if (dayFirstDigit == 3)
        {
            int monthFirst = int.Parse(monthTextFirst.text);
            int monthSecond = int.Parse(monthTextSecond.text);
            if (monthFirst == 0 && (monthSecond == 1 || monthSecond == 3 || monthSecond == 5 || monthSecond == 7 || monthSecond == 8))
            {
                dayTextSecond.text = "1"; // Monate mit 31 Tagen
            }
            else
            {
                dayTextSecond.text = "0"; // Monate mit 30 Tagen
            }
        }
        else if (dayFirstDigit == 0)
        {
            if (dayFirstDigit == 0 && int.Parse(dayTextSecond.text) == 0)
            {
                dayTextSecond.text = "1";
            }
        }
        else
        {
            dayTextSecond.text = (int.Parse(dayTextSecond.text) % 10).ToString();
        }

        // Stunde
        int hourFirstDigit = int.Parse(yearTextFirst.text) % 3;
        hourTextFirst.text = hourFirstDigit.ToString();
        if (hourFirstDigit == 2)
        {
            hourTextSecond.text = (int.Parse(hourTextSecond.text) % 4).ToString(); // Stunden 20, 21, 22, 23
        }
        else
        {
            hourTextSecond.text = (int.Parse(hourTextSecond.text) % 10).ToString();
        }

        // Minute
        minuteTextFirst.text = (int.Parse(yearTextFirst.text) % 7).ToString();
        minuteTextSecond.text = (int.Parse(yearTextFirst.text) % 10).ToString();
    }
}
