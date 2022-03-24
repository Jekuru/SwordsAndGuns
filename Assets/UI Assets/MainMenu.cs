using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Audio;

public class MainMenu : MonoBehaviour
{

    public TMPro.TMP_Dropdown resolutionDrop;
    Resolution[] resolutions;
    public TMPro.TMP_Dropdown fpsDrop;
    public AudioMixer audioMixer;

    void Start()
    {
        //obtener resoluciones disponibles, mostrarlas en el dropdown y agregarlas a una lista
        resolutions = Screen.resolutions;
        resolutionDrop.ClearOptions();
        List<string> options = new List<string>();
        int currentResolutionIndex = 0;
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + "x" + resolutions[i].height;
            options.Add(option);

            if(resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDrop.AddOptions(options);
        resolutionDrop.value = currentResolutionIndex;
        resolutionDrop.RefreshShownValue();

        int fpsIndex = fpsDrop.GetComponent<Dropdown>().value;

    }

    //cambio de resolucion
    public void setResolution (int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    //cambio de tasa de refresco
    public void setFps(int fpsIndex)
    {
        List<Dropdown.OptionData> menuOptions = fpsDrop.GetComponent<Dropdown>().options;
        string value = menuOptions[fpsIndex].text;
        int fps;
        int.TryParse(value, out fps);
        Application.targetFrameRate = fps;
    }
    
    //activar y desectivar pantalla completa
    public void setFullScreen(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
    }

    //cambios en los diferentes volumenes
    public void setGeneralVolume (float generalVolume)
    {
        audioMixer.SetFloat("volumeGeneral", generalVolume);
    }

    public void setEffectsVolume (float effectsVolume)
    {
        audioMixer.SetFloat("volumeEffects", effectsVolume);
    }

    public void setMusicVolume (float musicVolume)
    {
        audioMixer.SetFloat("volumeMusic", musicVolume);
    }


    //salida del juego
    public void exitButton()
    {
        Debug.Log("Exit game");
        Application.Quit();
    }
}
