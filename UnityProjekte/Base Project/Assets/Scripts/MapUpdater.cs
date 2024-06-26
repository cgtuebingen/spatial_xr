using UnityEngine;

public class MapUpdater : MonoBehaviour
{
    public OSMRequest tileLayer;

    void Start()
    {
        // Beispiel: Aktualisiere die Karte auf einen neuen Standort und Zoom-Level
        tileLayer.UpdateTile(48.8588443f, 2.2943506f, 15); // Beispiel: Eiffelturm, Paris
    }
}
