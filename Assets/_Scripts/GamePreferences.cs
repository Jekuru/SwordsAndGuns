using UnityEngine;

public class GamePreferences : MonoBehaviour
{
    // ESTABLECE LA TASA DE FRAMES POR SEGUNDO DE LA APLICACIÓN
    // ES NECESARIO DESACTIVAR LA SINCRONIZACIÓN VERTICAL (vSync) PARA ELLO

    public int targetFrameRate = 60; // TASA DE FRAMES POR SEGUNDO (FPS)

    void Start()
    {
        SetTargetFrameRate();
    }

    public void SetTargetFrameRate()
    {
        QualitySettings.vSyncCount = 0; // DESACTIVAR SINCRONIZACIÓN VERTICAL
        Application.targetFrameRate = targetFrameRate; // ESTABLECER TASA DEFRAMES EN LA APP
    }
}