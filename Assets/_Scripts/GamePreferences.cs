using UnityEngine;

public class GamePreferences : MonoBehaviour
{
    // ESTABLECE LA TASA DE FRAMES POR SEGUNDO DE LA APLICACI�N
    // ES NECESARIO DESACTIVAR LA SINCRONIZACI�N VERTICAL (vSync) PARA ELLO

    public int targetFrameRate = 60; // TASA DE FRAMES POR SEGUNDO (FPS)

    void Start()
    {
        SetTargetFrameRate();
    }

    public void SetTargetFrameRate()
    {
        QualitySettings.vSyncCount = 0; // DESACTIVAR SINCRONIZACI�N VERTICAL
        Application.targetFrameRate = targetFrameRate; // ESTABLECER TASA DEFRAMES EN LA APP
    }
}