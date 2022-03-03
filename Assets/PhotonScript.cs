using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine.UI;

public class PhotonScript : MonoBehaviourPunCallbacks
{
    public TMP_Text playerList;
    public Button commenceButton;

    public Dictionary<int, Player> players;
    public PhotonView view;
    private void Update()
    {
        if (PhotonNetwork.InRoom)
        {
            Debug.Log("In room");
            players = PhotonNetwork.CurrentRoom.Players;

            if (PhotonNetwork.CurrentRoom.PlayerCount >= 1) // TODO: 1 por testeo, 2 es el valor que debería tener...
            {
                commenceButton.interactable = true;
            }
            else
            {
                commenceButton.interactable = false;
            }
        }
    }

    public void CommenceButton()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            StartCoroutine(LoadScene());
        }
    }

    public IEnumerator LoadScene()
    {
        yield return new WaitForSeconds(2);
        Debug.Log("Loading scene...");
        PhotonNetwork.LoadLevel("Map2");
    }

    public override void OnLeftRoom()
    {
        PhotonNetwork.Disconnect();
    }

    public void QuickPlayButton()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("OnConnectedToMaster() was called by PUN.");
        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("Failed to connect to room" + message);
        RoomOptions r = new RoomOptions
        {
            MaxPlayers = 4,
            IsVisible = true,
            IsOpen = true
        };
        PhotonNetwork.JoinOrCreateRoom("RoomTest", r, TypedLobby.Default);
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Joined Room as " + PhotonNetwork.LocalPlayer.UserId);
        UpdatePlayerList();
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log(newPlayer.UserId + " joined the game.");
        UpdatePlayerList();
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Debug.Log(otherPlayer.UserId + " left the game.");
        UpdatePlayerList();
    }

    [PunRPC]
    public void UpdatePlayerList()
    {
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            playerList.text = player.UserId; // TODO: Cambiar por NICKNAME mas adelante
        }
        PhotonView photonView = PhotonView.Get(this);
        this.photonView.RPC("UpdatePlayerList", RpcTarget.All);
    }
}
