using UnityEngine;
using TMPro;
using System; // Fügt den Namespace hinzu, der DateTime enthält

// Enum zur Unterscheidung zwischen Stunden und Tagen
public enum DeltaType
{
    HOUR,
    DAY
}

public class TextMeshProComponentInitializationException : System.Exception
{
    public TextMeshProComponentInitializationException(string message) : base(message) { }
}

public class ClockInteraction : MonoBehaviour
{
    public AudioSource clockTickSound;  // Referenz zur AudioSource für das Tick-Geräusch

    // Temporäre Variablen um auf ClockController die Uhr zu aktualisieren, falls die Uhrzeit geändert wurde
    public TextMeshProUGUI yearTextSecondChange;
    public TextMeshProUGUI monthTextSecondChange;
    public TextMeshProUGUI dayTextSecondChange;
    public TextMeshProUGUI hourTextSecondChange;
    public TextMeshProUGUI minuteTextSecondChange;

    public Vector3 firstContactPoint; // Erster Kontaktpunkt bei der Kollision
    public Vector3 lastContactPoint;  // Letzter Kontaktpunkt bei der Kollision
    public TextMeshProUGUI textMeshProComponent; // TextMeshPro-Komponente
    private int guiTextValue = 0; // Wert, der im TextMeshPro angezeigt wird

    public ClockController clockController; // Referenz auf ClockController
    public DateTime oldDate;

    public string newDay;
    public string newHour;
    public DateTime updatedDate;

    public TextMeshProUGUI tempMash;

    private void OnTriggerEnter(Collider other)
    {
        firstContactPoint = other.transform.position;
    }

    private void OnTriggerExit(Collider other)
    {
        lastContactPoint = other.transform.position;

        // Alte Uhrzeit als DateTime erfassen
        oldDate = DateTime.Parse(GetFormattedDateTime());

        // Tupel delta berechnen, gibt zurück, ob Stunde oder Tag verändert werden soll, und um wie viel
        var delta = UpdateGUITextValue(firstContactPoint, lastContactPoint, ref guiTextValue, textMeshProComponent, clockController);

        if (delta.Item2 == DeltaType.DAY)
        {
            updatedDate = DateInFuture(oldDate, delta.Item1, 0); // Tage dem neuen Datum hinzufügen
            clockController.dayTextSecond = SetDatePartInTextMesh(updatedDate.ToString("yyyy-MM-dd HH:mm"), "day", tempMash);
            clockController.monthTextSecond = SetDatePartInTextMesh(updatedDate.ToString("yyyy-MM-dd HH:mm"), "month", tempMash);
            clockController.yearTextSecond = SetDatePartInTextMesh(updatedDate.ToString("yyyy-MM-dd HH:mm"), "year", tempMash);

        }
        else if (delta.Item2 == DeltaType.HOUR)
        {
            updatedDate = DateInFuture(oldDate, 0, delta.Item1); // Stunden dem neuen Datum hinzufügen
            clockController.hourTextSecond = SetDatePartInTextMesh(updatedDate.ToString("yyyy-MM-dd HH:mm"), "hour", tempMash);
        }

        clockController.isTimeChangedManually = true;

           


        // Spiele den Tick-Sound ab, wenn sich eine Ziffer geändert hat
        PlayClockTickSound();
    }

    void Update()
    {
        try
        {
            InitializeTextMeshProComponent();
        }
        catch (TextMeshProComponentInitializationException ex)
        {
            Debug.LogError(ex.Message);
            guiTextValue = 0; // Setze den Default-Wert auf 0
        }

        if (clockTickSound == null)
        {
            clockTickSound = GetComponent<AudioSource>();
        }


       
    }

    // Funktion zur Berechnung des Delta-Tupels
    (int, DeltaType) UpdateGUITextValue(Vector3 firstContactPoint, Vector3 lastContactPoint, ref int guiTextValue, TextMeshProUGUI textMeshProComponent, ClockController clockController)
    {
        DeltaType component;

        if (gameObject.name == "second_hour_digit")
        {
            component = DeltaType.HOUR;
        }
        else if (gameObject.name == "second_day_digit")
        {
            component = DeltaType.DAY;
        }
        else
        {
            component = DeltaType.HOUR; // Fallback, sollte nicht passieren
        }

        if (firstContactPoint != Vector3.zero && lastContactPoint != Vector3.zero)
        {
            float contactDifference = Mathf.Abs(firstContactPoint.y - lastContactPoint.y);

            // Wenn die Bewegung nach oben ist
            if (firstContactPoint.y < lastContactPoint.y)
            {
                if (contactDifference <= 0.3f)
                {
                    return (1, component);
                }
                else if (contactDifference <= 0.6f)
                {
                    return (5, component);
                }
                else
                {
                    return (7, component);
                }
            }
            // Wenn die Bewegung nach unten ist
            else if (firstContactPoint.y > lastContactPoint.y)
            {
                if (contactDifference <= 0.3f)
                {
                    return (-1, component);
                }
                else if (contactDifference <= 0.6f)
                {
                    return (-5, component);
                }
                else
                {
                    return (-7, component);
                }
            }
            
        }

        return (0, component); // Fallback
    }

    void InitializeTextMeshProComponent()
    {
        // Initialisiere die TextMeshPro-Komponente
        textMeshProComponent = GetComponentInChildren<TextMeshProUGUI>();

        // Überprüfe, ob die Komponente gültig ist
        if (textMeshProComponent == null)
        {
            throw new TextMeshProComponentInitializationException("TextMeshPro component not found on GameObject.");
        }

        // Versuche, den initialen Textinhalt in eine Ganzzahl zu konvertieren
        if (!int.TryParse(textMeshProComponent.text, out guiTextValue))
        {
            throw new TextMeshProComponentInitializationException("Failed to parse initial TextMeshPro value to an integer. Defaulting to 0.");
        }
    }

    // Funktion zur Berechnung des zukünftigen Datums
    DateTime DateInFuture(DateTime startDate, int days, int hours)
    {
        // Die Anzahl der Tage und Stunden hinzufügen
        DateTime newDate = startDate.AddDays(days).AddHours(hours);

        // Rückgabe des neuen Datums
        return newDate;
    }

    // Funktion, die alle TextMeshPro-Komponenten in das Format "yyyy-MM-dd HH:mm" konkateniert
    public string GetFormattedDateTime()
    {
        // Konkateniere das Jahr
        string year = yearTextSecondChange.text;

        // Konkateniere den Monat (füge "-" hinzu)
        string month = monthTextSecondChange.text;

        // Konkateniere den Tag (füge "-" hinzu)
        string day = dayTextSecondChange.text;

        // Konkateniere die Stunden (füge ":" hinzu)
        string hour = hourTextSecondChange.text;

        // Konkateniere die Minuten (füge ":" hinzu)
        string minute = minuteTextSecondChange.text;

        // Kombiniere alle Teile im Format "yyyy-MM-dd HH:mm"
        string formattedDateTime = $"{year}-{month}-{day} {hour}:{minute}";

        return formattedDateTime;
    }

    // Kombinierte Funktion, um entweder den Tag oder die Stunde aus einem Datumsstring zu extrahieren
    public TextMeshProUGUI SetDatePartInTextMesh(string dateTimeString, string datePart, TextMeshProUGUI textMeshPro)
    {
        DateTime date = DateTime.Parse(dateTimeString);

        // Basierend auf dem angeforderten Datums- oder Zeitteil (day, hour, year, month) wird der Text der TextMeshPro-Komponente gesetzt
        switch (datePart)
        {
            case "day":
                textMeshPro.text = dateTimeString.Day.ToString();
                break;
            case "hour":
                textMeshPro.text = dateTimeString.Hour.ToString();
                break;
            case "year":
                textMeshPro.text = dateTimeStringdate.Year.ToString();
                break;
            case "month":
                textMeshPro.text = dateTimeString.Month.ToString();
                break;
            default:
                Debug.LogWarning("Ungültiger datePart: " + datePart);
                break;
        }

        return textMeshPro;
    }

    // Funktion zum Abspielen des Tick-Sounds
    void PlayClockTickSound()
    {
        if (clockTickSound != null)
        {
            clockTickSound.Play();
        }
    }
}  