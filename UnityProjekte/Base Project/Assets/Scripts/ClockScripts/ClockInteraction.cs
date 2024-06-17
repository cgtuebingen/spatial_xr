using UnityEngine;
using TMPro;

public class ClockInteraction : MonoBehaviour
{
    private ContactPoint firstContactPoint; // Erster Kontaktpunkt bei der Kollision
    private ContactPoint lastContactPoint;  // Letzter Kontaktpunkt bei der Kollision
    private TextMeshProUGUI textMeshProComponent; // TextMeshPro-Komponente
    private int guiTextValue; // Wert, der im TextMeshPro angezeigt wird

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

    private void Start()
    {
        
    }

    private void Update()
    {
        // Initialisiere die TextMeshPro-Komponente
        textMeshProComponent = GetComponentInChildren<TextMeshProUGUI>();

        // Überprüfe, ob die Komponente gültig ist
        if (textMeshProComponent == null)
        {
            Debug.LogError("TextMeshPro component not found on GameObject.");
        }
        else
        {
            // Versuche, den initialen Textinhalt in eine Ganzzahl zu konvertieren
            if (int.TryParse(textMeshProComponent.text, out guiTextValue))
            {
                // Erfolgreich initialisiert
                Debug.Log("Initial value parsed: " + guiTextValue);
            }
            else
            {
                Debug.LogWarning("Failed to parse initial TextMeshPro value to an integer. Defaulting to 0.");
                Debug.Log("Current TextMeshPro text: " + textMeshProComponent.text);
                guiTextValue = 0;
            }
        }

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
        Debug.Log("Updated value: " + guiTextValue);
    }
}
