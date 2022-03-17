using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerPreferences : MonoBehaviour
{
    public string nickName = "Player";
    public int score;

    public TMP_InputField nameField;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(nameField.text.Length >= 3)
        {
            nickName = nameField.text;
        }
        else
        {
            nickName = "Player" + Random.Range(1, 999999);
        }
    }

    
}
