using UnityEngine;
using TMPro;

public class TextMeshProComponentInitializationException : System.Exception
{
    public TextMeshProComponentInitializationException(string message) : base(message) { }
}

public class ClockInteraction : MonoBehaviour
{
    public AudioSource clockTickSound;  // Referenz zur AudioSource für das Tick-Geräusch

    //temporäre Variablen um auf ClockController die Uhr zu aktualisieren, falls die Uhrzeit geändert wurde
    public TextMeshProUGUI yearTextFirstChange;
    public TextMeshProUGUI yearTextSecondChange;
    public TextMeshProUGUI monthTextFirstChange;
    public TextMeshProUGUI monthTextSecondChange;
    public TextMeshProUGUI dayTextFirstChange;
    public TextMeshProUGUI dayTextSecondChange;
    public TextMeshProUGUI hourTextFirstChange;
    public TextMeshProUGUI hourTextSecondChange;
    public TextMeshProUGUI minuteTextFirstChange;
    public TextMeshProUGUI minuteTextSecondChange;

    private Vector3 firstContactPoint; // Erster Kontaktpunkt bei der Kollision
    private Vector3 lastContactPoint;  // Letzter Kontaktpunkt bei der Kollision
    private TextMeshProUGUI textMeshProComponent; // TextMeshPro-Komponente
    private int guiTextValue = 0; // Wert, der im TextMeshsPro angezeigt wird

    public ClockController clockController; // Referenz auf ClockController

    public float targetHeightUP = 200f;
    public float targetHeightDown = 2000f;

    private void Start()
    {
        // Falls die AudioSource nicht über den Editor zugewiesen wurde, versuche sie vom GameObject zu beziehen
        if (clockTickSound == null)
        {
            clockTickSound = GetComponent<AudioSource>();
        }
    }

    private void OnTriggerEnter(Collider other){
        firstContactPoint = other.transform.position;
    }

    private void OnTriggerExit(Collider other){
        lastContactPoint = other.transform.position;
        UpdateGUITextValue(firstContactPoint, lastContactPoint, ref guiTextValue, textMeshProComponent, clockController);

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
    }

    void UpdateGUITextValue(Vector3 firstContactPoint, Vector3 lastContactPoint, ref int guiTextValue, TextMeshProUGUI textMeshProComponent, ClockController clockController)
    {
        // Überprüfe, ob gültige ContactPoints vorhanden sind
        if (firstContactPoint != Vector3.zero && lastContactPoint != Vector3.zero)
        {
            // Berechne den Unterschied der y-Komponente der Normalenvektoren der Kontaktpunkte
            float contactDifference = Mathf.Abs(firstContactPoint.y - lastContactPoint.y);

            // Vergleiche die y-Komponente der Normalenvektoren und aktualisiere den Wert entsprechend
            if (firstContactPoint.y < lastContactPoint.y)
            {
                if (contactDifference <= 0.3f)
                {
                    guiTextValue++;
                }
                else if (contactDifference <= 0.6f)
                {
                    guiTextValue += 2;
                }
                else
                {
                    guiTextValue += 5;
                }
            }
            else if (firstContactPoint.y > lastContactPoint.y)
            {
                if (contactDifference <= 0.3f)
                {
                    guiTextValue--;
                }
                else if (contactDifference <= 0.6f)
                {
                    guiTextValue -= 2;
                }
                else
                {
                    guiTextValue -= 5;
                }
            }

            // Aktualisiere den Text der TextMeshPro-Komponente mit dem neuen Wert
            textMeshProComponent.text = guiTextValue.ToString();
            
            // Weise den aktualisierten Wert der entsprechenden Komponente des ClockControllers zu
            switch (gameObject.name)
            {
                case "first_year_digit":
                    clockController.yearTextFirst = textMeshProComponent;
                    break;
                case "second_year_digit":
                    clockController.yearTextSecond = textMeshProComponent;
                    break;
                case "first_month_digit":
                    clockController.monthTextFirst = textMeshProComponent;
                    break;
                case "second_month_digit":
                    clockController.monthTextSecond = textMeshProComponent;
                    break;
                case "first_day_digit":
                    clockController.dayTextFirst = textMeshProComponent;
                    break;
                case "second_day_digit":
                    clockController.dayTextSecond = textMeshProComponent;
                    break;
                case "first_hour_digit":
                    clockController.hourTextFirst = textMeshProComponent;
                    break;
                case "second_hour_digit":
                    clockController.hourTextSecond = textMeshProComponent;
                    break;
                case "first_minute_digit":
                    clockController.minuteTextFirst = textMeshProComponent;
                    break;
                case "second_minute_digit":
                    clockController.minuteTextSecond = textMeshProComponent;
                    break;
                default:
                    Debug.Log(gameObject.name + " is NOT one of the specific GameObjects.");
                    break;
            }
        
            clockController.isTimeChangedManually = true;

            // Zurücksetzen der ContactPoints nach der Verarbeitung
            firstContactPoint = new Vector3(0,0,0);
            lastContactPoint = new Vector3(0,0,0);
        }
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

    // Funktion zum Abspielen des Tick-Sounds
    void PlayClockTickSound()
    {
        if (clockTickSound != null)
        {
            clockTickSound.Play();
        }
    }
}
