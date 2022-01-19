using UnityEngine;

public class GamePreferences : MonoBehaviour
{
    public int targetFrameRate = 60;


    void Start()
    {
        SetTargetFrameRate();
    }

    public void SetTargetFrameRate()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = targetFrameRate;
    }
}