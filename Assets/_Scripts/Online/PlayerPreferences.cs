using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerPreferences : MonoBehaviour
{
    public string nickName = "Player";
    //public int score; WIP, necesita API.

    public TMP_InputField nameField;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    public void NameChange()
    {
        nickName = nameField.textComponent.text;
    }    
}
