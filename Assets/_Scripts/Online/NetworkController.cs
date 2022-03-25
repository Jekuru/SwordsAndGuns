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
    [SerializeField] private Button leftRoundButton;
    [SerializeField] private Button rightRoundButton;
    [SerializeField] private TMP_Text roundsText;
    [SerializeField] private int rounds = 5;
    [Header("Selection elements")]
    [SerializeField] private GameObject selection;

    [Header("Loading screen")]
    [SerializeField] private GameObject loadingScreen;

    // *** Fin UI *** ///

    // *** Fin Inspector *** //

    private void Start()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        DontDestroyOnLoad(this.gameObject);
    }

    private void Update()
    {
        if (PhotonNetwork.InRoom && SceneManagerHelper.ActiveSceneName == "Menu")
        {
            Debug.Log("In room");
            Debug.Log("Jugadores en la sala: " + PhotonNetwork.PlayerList.Length);

            if (PhotonNetwork.CurrentRoom.PlayerCount >= 1) // TODO: 1 por testeo, 2 es el valor que debería tener...
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
    public void ButtonPlayLocale()
    {
        PhotonNetwork.OfflineMode = true;
        PhotonNetwork.Disconnect();
        PhotonNetwork.LoadLevel("MainTestingScene");
    }

    public void ButtonConnectOnline()
    {
        if(playerPreferences.nickName.Length < 3)
            playerPreferences.nickName = "Player" + Random.Range(1, 999999);

        PhotonNetwork.OfflineMode = false;
        PhotonNetwork.ConnectUsingSettings();
    }

    public void ButtonQuickPlay()
    {
        if (PhotonNetwork.NetworkClientState == ClientState.JoinedLobby)
        {
            PhotonNetwork.JoinRandomRoom();
            playOnlineMenu.SetActive(false);
            loadingScreen.SetActive(true);
        }
    }

    public void ButtonCreateRoom()
    {
        if (PhotonNetwork.NetworkClientState == ClientState.JoinedLobby)
        {
            playOnlineMenu.SetActive(false);
            loadingScreen.SetActive(true);
            HostRoom();
        }   
    }

    public void ButtonHostPrivate()
    {
        if (PhotonNetwork.NetworkClientState == ClientState.JoinedLobby)
        {
            playOnlineMenu.SetActive(false);
            loadingScreen.SetActive(true);
            PrivateHost();
        }
    }

    public void ButtonJoinRoom()
    { 
        if(PhotonNetwork.NetworkClientState == ClientState.JoinedLobby)
        {
            loadingScreen.SetActive(true);
            PhotonNetwork.JoinRoom(roomNumberInput.text);
        }
    }

    public void ButtonBack()
    {
        loadingScreen.SetActive(true);
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
        //playerPreferences.NameSave();
        LoadScene();
        PhotonNetwork.LoadLevel("CharSelection");
    }

    public void ButtonStartOnlineGame()
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
        Debug.Log("Cargando escena de selección...");
        yield return new WaitForSeconds(2);

        PhotonNetwork.LoadLevel("CharSelection");
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
            loadingScreen.SetActive(false);
        }
    }

    public override void OnLeftRoom()
    {
        Debug.Log(PhotonNetwork.LocalPlayer.NickName + " left the room");

        if(SceneManagerHelper.ActiveSceneName == "Menu")
            roomNumber.text = "";
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log(PhotonNetwork.LocalPlayer.NickName + " disconnected from server " + cause);

        if(SceneManagerHelper.ActiveSceneName == "Menu")
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
            loadingScreen.SetActive(false);
        }
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log("No se pudo unir a la sala especificada");
        PhotonNetwork.JoinLobby();
        if(PhotonNetwork.NetworkClientState == ClientState.JoinedLobby)
        {
            playOnlineMenu.SetActive(true);
            loadingScreen.SetActive(false);
        }
        
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Uniéndose como: " + PhotonNetwork.LocalPlayer.NickName);

        roomNumber.text = PhotonNetwork.CurrentRoom.Name;
        commenceButton.gameObject.SetActive(PhotonNetwork.IsMasterClient);

        if(PhotonNetwork.IsMasterClient)
            PhotonNetwork.Instantiate("LobbyController", gameObject.transform.position, Quaternion.identity);

        lobby.SetActive(true);
        loadingScreen.SetActive(false);

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
        if (PhotonNetwork.IsMasterClient && SceneManagerHelper.ActiveSceneName == "Menu")
            commenceButton.gameObject.SetActive(true);

        if (PhotonNetwork.IsMasterClient && SceneManagerHelper.ActiveSceneName == "MatchEnd")
        {
            GameObject button = GameObject.FindGameObjectWithTag("RematchButton");
            button.SetActive(false);
        }
    }

    public override void OnJoinedLobby()
    {
        playOnlineMenu.SetActive(true);
        loadingScreen.SetActive(false);
    }

    void HostRoom()
    {
        Debug.Log("Creando nueva sala...");
        RoomOptions r = new RoomOptions
        {
            MaxPlayers = 4,
            IsVisible = true,
            IsOpen = true,
            PublishUserId = true
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
            IsOpen = true,
            PublishUserId = true
        };
        randomRoomNumber = Random.Range(100000, 999999);
        PhotonNetwork.JoinOrCreateRoom(randomRoomNumber.ToString(), r, TypedLobby.Default);
        roomNumber.text = randomRoomNumber.ToString();
    }

    void PrivateHost()
    {
        Debug.Log("Creando nueva sala...");
        RoomOptions r = new RoomOptions
        {
            MaxPlayers = 4,
            IsVisible = false,
            IsOpen = true,
            PublishUserId = true
        };
        randomRoomNumber = Random.Range(100000, 999999);
        PhotonNetwork.JoinOrCreateRoom(randomRoomNumber.ToString(), r, TypedLobby.Default);
        roomNumber.text = randomRoomNumber.ToString();
    }

    void ClearPlayerListings()
    {
        if (playersContainer == null)
            return;

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
            if(player == PhotonNetwork.PlayerList[0])
            {
                tempText.color = Color.red;
            } else if(player == PhotonNetwork.PlayerList[1])
            {
                tempText.color = Color.blue;
            } else if (player == PhotonNetwork.PlayerList[2])
            {
                tempText.color = Color.green;
            } else if (player == PhotonNetwork.PlayerList[3])
            {
                tempText.color = Color.magenta;
            }
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
