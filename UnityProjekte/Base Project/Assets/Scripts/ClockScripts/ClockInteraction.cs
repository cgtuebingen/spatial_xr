using UnityEngine;
using TMPro;

public class ClockInteraction : MonoBehaviour
{
    private ContactPoint firstContactPoint;
    private ContactPoint lastContactPoint;
    private TextMeshProUGUI textMeshProComponent;
    private int guiTextValue;

    private void OnCollisionEnter(Collision collision)
    {
        firstContactPoint = collision.contacts[0];
    }

    private void OnCollisionExit(Collision collision)
    {
        lastContactPoint = collision.contacts[collision.contacts.Length - 1];
    }

    private void Start()
    {
        
    }

    private void Update()
    {

        // Versuche, die TextMeshPro-Komponente zu erhalten
        textMeshProComponent = GetComponentInChildren<TextMeshProUGUI>();

        // Überprüfe, ob die Komponente gültig ist
        if (textMeshProComponent == null)
        {
            Debug.LogError("TextMeshPro component not found on GameObject.");
        }
        else
        {
            // Konvertiere den initialen Textinhalt in eine Ganzzahl, falls möglich
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
        // Vergleiche die Kontakt-Punkte und aktualisiere den Wert
        if (firstContactPoint.normal.y < lastContactPoint.normal.y)
        {
            guiTextValue++;
        }
        else if (firstContactPoint.normal.y > lastContactPoint.normal.y)
        {
            guiTextValue--;
        }

        // Update der GUI-Text-Komponente mit dem neuen Wert
        textMeshProComponent.text = guiTextValue.ToString();
        Debug.Log(" value parsed: " + guiTextValue);
    }
}