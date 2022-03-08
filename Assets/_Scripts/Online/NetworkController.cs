using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine.UI;

public class NetworkController : MonoBehaviourPunCallbacks
{
    [SerializeField] private ClientState UserStatus;
    // *** Asignar desde el inspector ***
    [SerializeField] private PlayerPreferences playerPreferences;

    // *** UI *** //
    [Header("Lobby elements")]
    [SerializeField] private GameObject playOnlineMenu;
    [SerializeField] private TMP_Text serverText;
    [SerializeField] private TMP_InputField roomNumberInput;
    [SerializeField] private Button joinRoomButton;
    [Header("Room elements")]
    [SerializeField] private GameObject lobby;
    [SerializeField] private TMP_Text roomNumber;
    [SerializeField] private int randomRoomNumber;
    [SerializeField] private Transform playersContainer;
    [SerializeField] private GameObject playerListingPrefab;
    [SerializeField] private Button commenceButton;

    // *** Fin UI *** ///

    // *** Fin Inspector *** //



    private void Update()
    {
        if (PhotonNetwork.InRoom)
        {
            Debug.Log("In room");
            Debug.Log("Jugadores en la sala: " + PhotonNetwork.PlayerList.Length);

            if (PhotonNetwork.CurrentRoom.PlayerCount >= 1) // TODO: 1 por testeo, 2 es el valor que deber�a tener...
            {
                commenceButton.interactable = true;
            }
            else
            {
                commenceButton.interactable = false;
            }
        }
        UserStatus = PhotonNetwork.NetworkClientState;
    }

    #region Botones
    public void ButtonConnectOnline()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public void ButtonQuickPlay()
    {
        if (PhotonNetwork.NetworkClientState == ClientState.JoinedLobby)
        {
            PhotonNetwork.JoinRandomRoom();
            playOnlineMenu.SetActive(false);
        }
    }

    public void ButtonCreateRoom()
    {
        if (PhotonNetwork.NetworkClientState == ClientState.JoinedLobby)
        {
            playOnlineMenu.SetActive(false);
            HostRoom();
        }   
    }

    public void ButtonJoinRoom()
    { 
        if(PhotonNetwork.NetworkClientState == ClientState.JoinedLobby)
        {
            PhotonNetwork.JoinRoom(roomNumberInput.text);
        }
    }

    public void ButtonBack()
    {
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.LeaveLobby();
        StartCoroutine(rejoinLobby());
    }

    public void ButtonDisconnect()
    {
        PhotonNetwork.LeaveLobby();
        PhotonNetwork.Disconnect();
    }

    public void ButtonCommence()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.CurrentRoom.IsOpen = false;
            StartCoroutine(LoadScene());
        }
    }
    #endregion

    IEnumerator LoadScene()
    {
        yield return new WaitForSeconds(2);
        Debug.Log("Cargando escena...");
        PhotonNetwork.LoadLevel("Map2");
    }

    IEnumerator rejoinLobby()
    {
        yield return new WaitForSeconds(1);
        PhotonNetwork.JoinLobby();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Conectado al servidor " + PhotonNetwork.CloudRegion);

        PhotonNetwork.JoinLobby();
        serverText.text = PhotonNetwork.CloudRegion;
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.NickName = playerPreferences.nickName;

        if (PhotonNetwork.NetworkClientState == ClientState.JoinedLobby)
        {
            playOnlineMenu.SetActive(true);
        }
    }

    public override void OnLeftRoom()
    {
        Debug.Log(PhotonNetwork.LocalPlayer.NickName + " left the room");

        roomNumber.text = "";
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log(PhotonNetwork.LocalPlayer.NickName + " disconnected from server " + cause);

        serverText.text = "Conectando...";
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        if(message == "No match found")
        {
            Debug.Log("No se ha encontrado una sala libre.");
        }
        else
        {
            Debug.LogError("Error: " + message);
        }

        QuickPlay();
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("No se pudo crear una nueva sala.\n" + returnCode + " - " + message);
        PhotonNetwork.JoinLobby();
        if (PhotonNetwork.NetworkClientState == ClientState.JoinedLobby)
        {
            playOnlineMenu.SetActive(true);
        }
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log("No se pudo unir a la sala especificada");
        PhotonNetwork.JoinLobby();
        if(PhotonNetwork.NetworkClientState == ClientState.JoinedLobby)
        {
            playOnlineMenu.SetActive(true);
        }
        
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Uni�ndose como: " + PhotonNetwork.LocalPlayer.NickName);

        roomNumber.text = PhotonNetwork.CurrentRoom.Name;
        commenceButton.gameObject.SetActive(PhotonNetwork.IsMasterClient);

        lobby.SetActive(true);

        ClearPlayerListings();
        ListPlayers();
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log(newPlayer.NickName + " se ha unido a la sala.");
        ClearPlayerListings();
        ListPlayers();
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Debug.Log(otherPlayer.NickName + " se ha ido de la sala.");

        ClearPlayerListings();
        ListPlayers();
        if (PhotonNetwork.IsMasterClient)
            commenceButton.gameObject.SetActive(true);
    }

    public override void OnJoinedLobby()
    {
        playOnlineMenu.SetActive(true);
    }

    void HostRoom()
    {
        Debug.Log("Creando nueva sala...");
        RoomOptions r = new RoomOptions
        {
            MaxPlayers = 4,
            IsVisible = true,
            IsOpen = true
        };
        randomRoomNumber = Random.Range(100000, 999999);
        PhotonNetwork.CreateRoom(randomRoomNumber.ToString(), r, TypedLobby.Default);
        roomNumber.text = randomRoomNumber.ToString();
    }

    void QuickPlay()
    {
        Debug.Log("Creando nueva sala...");
        RoomOptions r = new RoomOptions
        {
            MaxPlayers = 4,
            IsVisible = true,
            IsOpen = true
        };
        randomRoomNumber = Random.Range(100000, 999999);
        PhotonNetwork.JoinOrCreateRoom(randomRoomNumber.ToString(), r, TypedLobby.Default);
        roomNumber.text = randomRoomNumber.ToString();
    }

    void ClearPlayerListings()
    {
        for (int i = playersContainer.childCount - 1; i >= 0; i--)
        {
            Destroy(playersContainer.GetChild(i).gameObject);
        }
    }

    void ListPlayers()
    {
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            GameObject tempListing = Instantiate(playerListingPrefab, playersContainer);
            TMP_Text tempText = tempListing.transform.GetChild(0).GetComponent<TMP_Text>();
            tempText.text = player.NickName;
        }
    }

    public void RoomNumberChecker()
    {
        if(roomNumberInput.text.Length == 6)
        {
            joinRoomButton.interactable = true;   
        }
        else
        {
            joinRoomButton.interactable = false;
        }
    }
}