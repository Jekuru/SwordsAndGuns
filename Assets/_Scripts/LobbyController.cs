using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine.UI;

public class LobbyController : MonoBehaviour
{
    [SerializeField] private GameObject lobbyCanvas;
    [SerializeField] private GameObject roundSelectorPosition;
    [SerializeField] private GameObject lRoundButton;
    [SerializeField] private GameObject rRoundButton;
    [SerializeField] private TMP_Text roundsText;
    [SerializeField] private int rounds = 5;

    private PhotonView photonView;

    private void Awake()
    {
        photonView = GetComponent<PhotonView>();
    }

    // Start is called before the first frame update
    void Start()
    {
        lobbyCanvas = GameObject.FindGameObjectWithTag("LobbyCanvas");
        roundSelectorPosition = GameObject.FindGameObjectWithTag("RoundsSelectorPosition");

        gameObject.transform.SetParent(lobbyCanvas.transform);
        gameObject.GetComponent<RectTransform>().sizeDelta = roundSelectorPosition.GetComponent<RectTransform>().sizeDelta;
        gameObject.GetComponent<RectTransform>().localScale = roundSelectorPosition.GetComponent<RectTransform>().localScale;
        gameObject.GetComponent<RectTransform>().position = roundSelectorPosition.GetComponent<RectTransform>().position;
        photonView.RPC("SetRoundsSelector", RpcTarget.Others);
    }

    private void Update()
    {
        if (photonView.IsMine)
        {
            rRoundButton.SetActive(true);
            lRoundButton.SetActive(true);
        }
    }

    [PunRPC]
    void SetRoundsSelector()
    {
        gameObject.GetComponent<RectTransform>().sizeDelta = roundSelectorPosition.GetComponent<RectTransform>().sizeDelta;
        gameObject.GetComponent<RectTransform>().localScale = roundSelectorPosition.GetComponent<RectTransform>().localScale;
        gameObject.GetComponent<RectTransform>().position = roundSelectorPosition.GetComponent<RectTransform>().position;
    }

    public void ButtonLeftRound()
    {
        if (rounds == 1)
        {
            rounds = 10;
        }
        else
        {
            rounds--;
        }
        //PlayerPrefs.SetInt("Rounds", rounds);
        photonView.RPC("SendRounds", RpcTarget.All, rounds);
    }

    public void ButtonRightRound()
    {
        if (rounds == 10)
        {
            rounds = 1;
        }
        else
        {
            rounds++;
        }
        //PlayerPrefs.SetInt("Rounds", rounds);
        photonView.RPC("SendRounds", RpcTarget.All, rounds);
    }

    [PunRPC]
    void SendRounds(int rounds)
    {
        roundsText.text = rounds.ToString();
        PlayerPrefs.SetInt("MaxRounds", rounds);
    }
}
