using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;

public class PlayerPreferences : MonoBehaviour
{
    public string nickName = "Player";
    //public int score; WIP, necesita API.

    public TMP_InputField nameField;

    private PlayerPreferences[] exists;

    public bool onMain = true;

    private void Awake()
    {
        exists = GameObject.FindObjectsOfType<PlayerPreferences>();
        if(exists.Length > 1)
            Destroy(exists[1].gameObject);
    }

    private void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    private void Update()
    {
        if (SceneManagerHelper.ActiveSceneName == "Menu" || onMain)
        {
            nameField = FindObjectOfType<TMP_InputField>();
            nameField.onValueChanged.AddListener(delegate { NameChange(); });
            nameField.onEndEdit.AddListener(delegate { NameChange(); });
            //nameField = GameObject.Find("NameInputField").GetComponent<TMP_InputField>();
        }

    }

    public void NameChange()
    {
        if(nameField.textComponent.text.Length >= 3)
        {
            nickName = nameField.textComponent.text;
            PlayerPrefs.SetString("SavedNickname", nickName);
        }
    }    
}
