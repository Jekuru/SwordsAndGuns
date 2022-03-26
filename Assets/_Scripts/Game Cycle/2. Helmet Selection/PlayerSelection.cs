using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class PlayerSelection : MonoBehaviour
{
    [SerializeField] private int player;
    [SerializeField] public GameObject canvas;
    [SerializeField] private GameObject[] playerPositions;

    [Header("PlayerInfo")]
    [SerializeField] private TMP_Text playerText;
    [SerializeField] private Image playerBody;
    [SerializeField] private GameObject playerHelmet;
    [SerializeField] private GameObject playerLeftButton;
    [SerializeField] private GameObject playerRightButton;

    [Header("Helmets")]
    [SerializeField] private int helmetPosition = 0;
    [SerializeField] private Sprite[] helmets;

    [Header("IsReady")] // WIP
    [SerializeField] private bool isReady = false;  // WIP
    
    private PhotonView photonView;

    private void Awake()
    {
        photonView = GetComponent<PhotonView>();

        if (!photonView.IsMine)
            return;

        canvas = GameObject.FindGameObjectWithTag("Canvas");
        playerPositions = GameObject.FindGameObjectsWithTag("PlayerPositions");

        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            Debug.Log("Player: " + PhotonNetwork.PlayerList[i]);
            if (PhotonNetwork.PlayerList[i].IsLocal)
                player = i;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        if (!photonView.IsMine)
            return;

        gameObject.transform.SetParent(canvas.transform);
        gameObject.GetComponent<RectTransform>().sizeDelta = playerPositions[player].GetComponent<RectTransform>().sizeDelta;
        gameObject.GetComponent<RectTransform>().localScale = playerPositions[player].GetComponent<RectTransform>().localScale;
        gameObject.GetComponent<RectTransform>().position = playerPositions[player].GetComponent<RectTransform>().position;
        photonView.RPC("SetCanvas", RpcTarget.Others, player);

        SetSelectionInfoLocale();
        photonView.RPC("SetSelectionInfoOnline", RpcTarget.Others, PhotonNetwork.LocalPlayer.NickName, PhotonNetwork.LocalPlayer.UserId);

        Hashtable hash = new Hashtable();
        hash.Add("Helmet", helmetPosition);
        PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
    }

    [PunRPC]
    void SetCanvas(int player)
    {
        canvas = GameObject.FindGameObjectWithTag("Canvas");
        gameObject.transform.SetParent(canvas.transform);

        playerPositions = GameObject.FindGameObjectsWithTag("PlayerPositions");
        gameObject.GetComponent<RectTransform>().sizeDelta = playerPositions[player].GetComponent<RectTransform>().sizeDelta;
        gameObject.GetComponent<RectTransform>().localScale = playerPositions[player].GetComponent<RectTransform>().localScale;
        gameObject.GetComponent<RectTransform>().position = playerPositions[player].GetComponent<RectTransform>().position;
    }

    void SetSelectionInfoLocale()
    {
        playerLeftButton.SetActive(true);
        playerRightButton.SetActive(true);
        playerText.text = PhotonNetwork.LocalPlayer.NickName;
        if (PhotonNetwork.LocalPlayer.UserId == PhotonNetwork.PlayerList[0].UserId)
        {
            playerText.color = Color.red;
            playerBody.color = Color.red;
        }
        else if (PhotonNetwork.LocalPlayer.UserId == PhotonNetwork.PlayerList[1].UserId)
        {
            playerText.color = Color.blue;
            playerBody.color = Color.blue;
        }
        else if (PhotonNetwork.LocalPlayer.UserId == PhotonNetwork.PlayerList[2].UserId)
        {
            playerText.color = Color.green;
            playerBody.color = Color.green;
        }
        else if (PhotonNetwork.LocalPlayer.UserId == PhotonNetwork.PlayerList[3].UserId)
        {
            playerText.color = Color.magenta;
            playerBody.color = Color.magenta;
        }
    }

    [PunRPC]
    void SetSelectionInfoOnline(string nickName, string userId)
    {
        Debug.Log("Hola " + nickName + " - " + userId);
        playerText.text = nickName;
        if (PhotonNetwork.PlayerList.Length >= 2)
        {
            Debug.Log("Dos jugadores");
            Debug.Log("1: " + userId == PhotonNetwork.PlayerList[0].UserId);
            if (userId == PhotonNetwork.PlayerList[0].UserId)
            {
                playerText.color = Color.red;
                playerBody.color = Color.red;
                Debug.Log("Pintar rojo");
            }
            Debug.Log("2: " + userId == PhotonNetwork.PlayerList[1].UserId);
            if (userId == PhotonNetwork.PlayerList[1].UserId)
            {
                playerText.color = Color.blue;
                playerBody.color = Color.blue;
                Debug.Log("Pintar azul");
            }
        }
        if (PhotonNetwork.PlayerList.Length >= 3)
        {
            if (userId == PhotonNetwork.PlayerList[2].UserId)
            {
                playerText.color = Color.green;
                playerBody.color = Color.green;
                Debug.Log("Pintar verde");
            }
        }
        if (PhotonNetwork.PlayerList.Length == 4)
        {
            if (userId == PhotonNetwork.PlayerList[3].UserId)
            {
                playerText.color = Color.magenta;
                playerBody.color = Color.magenta;
                Debug.Log("Pintar magenta");
            }   
        }
        Debug.Log("Adios");
    }

    public void HelmetLeftButton()
    {
        if (helmetPosition == 0)
        {
            helmetPosition = helmets.Length - 1;
        }
        else
        {
            helmetPosition--;
        }

        photonView.RPC("ChangeHelmet", RpcTarget.All, helmetPosition); // Cambiar casco para OTROS

        Hashtable hash = new Hashtable();
        hash.Add("Helmet", helmetPosition);
        PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
    }

    public void HelmetRightButton()
    {
        if (helmetPosition == helmets.Length - 1)
        {
            helmetPosition = 0;
        }
        else
        {
            helmetPosition++;
        }

        photonView.RPC("ChangeHelmet", RpcTarget.All, helmetPosition); // Cambiar casco para OTROS

        Hashtable hash = new Hashtable();
        hash.Add("Helmet", helmetPosition);
        PhotonNetwork.LocalPlayer.SetCustomProperties(hash);

    }

    [PunRPC]
    void ChangeHelmet(int position)
    {
        playerHelmet.GetComponent<Image>().sprite = helmets[position];
    }
}
