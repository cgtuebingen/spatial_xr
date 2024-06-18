using UnityEngine;
using TMPro;

public class TextMeshProComponentInitializationException : System.Exception
{
    public TextMeshProComponentInitializationException(string message) : base(message) { }
}

public class ClockInteraction : MonoBehaviour
{
    private ContactPoint firstContactPoint; // Erster Kontaktpunkt bei der Kollision
    private ContactPoint lastContactPoint;  // Letzter Kontaktpunkt bei der Kollision
    private TextMeshProUGUI textMeshProComponent; // TextMeshPro-Komponente
    private int guiTextValue; // Wert, der im TextMeshPro angezeigt wird

    public ClockController clockController; // Referenz auf ClockController

    private void OnCollisionEnter(Collision collision)
    {
        // Speichere den ersten Kontaktpunkt bei der Kollision
        firstContactPoint = collision.contacts[0];
    }

    private void OnCollisionExit(Collision collision)
    {
        // Speichere den letzten Kontaktpunkt bei der Kollision
        lastContactPoint = collision.contacts[collision.contacts.Length - 1];
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

        UpdateGUITextValue(firstContactPoint, lastContactPoint, ref guiTextValue, textMeshProComponent, clockController);
    }

    void UpdateGUITextValue(ContactPoint firstContactPoint, ContactPoint lastContactPoint, ref int guiTextValue, TextMeshProUGUI textMeshProComponent, ClockController clockController)
    {
        // Überprüfe, ob gültige ContactPoints vorhanden sind
        if (firstContactPoint.normal != Vector3.zero && lastContactPoint.normal != Vector3.zero)
        {
            // Berechne den Unterschied der y-Komponente der Normalenvektoren der Kontaktpunkte
            float contactDifference = Mathf.Abs(firstContactPoint.normal.y - lastContactPoint.normal.y);

            // Vergleiche die y-Komponente der Normalenvektoren und aktualisiere den Wert entsprechend
            if (firstContactPoint.normal.y < lastContactPoint.normal.y)
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
            else if (firstContactPoint.normal.y > lastContactPoint.normal.y)
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
            clockController.isTimeChangedManually = true;

            // Zurücksetzen der ContactPoints nach der Verarbeitung
            firstContactPoint = new ContactPoint();
            lastContactPoint = new ContactPoint();
            Debug.Log("Updated value: " + guiTextValue);
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

        // Erfolgreich initialisiert
        Debug.Log("Initial value parsed: " + guiTextValue);
    }
}
