using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class SelectionController : MonoBehaviour
{
    //[SerializeField] private PlayerPreferences playerPreferences;

    [Header("Player01")]
    [SerializeField] private GameObject playerOne_LeftHelmetButton;
    [SerializeField] private GameObject playerOne_RightHelmetButton;

    [Header("Player02")]
    [SerializeField] private GameObject playerTwo_LeftHelmetButton;
    [SerializeField] private GameObject playerTwo_RightHelmetButton;

    [Header("Player03")]
    [SerializeField] private GameObject playerThree_LeftHelmetButton;
    [SerializeField] private GameObject playerThree_RightHelmetButton;

    [Header("Player04")]
    [SerializeField] private GameObject playerFour_LeftHelmetButton;
    [SerializeField] private GameObject playerFour_RightHelmetButton;

    [Header("Player nicknames")]
    [SerializeField] private TMP_Text[] playerNames;

    [Header("Player helmets")]
    [SerializeField] private PhotonView[] helmetPhotonViews;
    [SerializeField] private int[] playerHelmetPos;
    [SerializeField] private List<GameObject> playerHelmets; // Posición donde se van a instanciar los "cascos" de cada jugador. Asignado desde el inspector
    [SerializeField] private Sprite[] helmetStorage;

    private void Awake()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            playerNames[i].text = PhotonNetwork.PlayerList[i].NickName;
        }

        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            if(PhotonNetwork.LocalPlayer == PhotonNetwork.PlayerList[i])
                if(PhotonNetwork.PlayerList[i] == PhotonNetwork.PlayerList[0])
                {
                    playerHelmets[0] = PhotonNetwork.Instantiate("HelmetSelection", playerHelmets[0].gameObject.transform.position, playerHelmets[0].gameObject.transform.rotation);
                    helmetPhotonViews[0] = playerHelmets[0].GetComponent<PhotonView>();
                }
                else if (PhotonNetwork.PlayerList[i] == PhotonNetwork.PlayerList[1])
                {
                    playerHelmets[1] = PhotonNetwork.Instantiate("HelmetSelection", playerHelmets[1].gameObject.transform.position, playerHelmets[1].gameObject.transform.rotation);
                    helmetPhotonViews[1] = playerHelmets[1].GetComponent<PhotonView>();
                }
                else if (PhotonNetwork.PlayerList[i] == PhotonNetwork.PlayerList[2])
                {
                    playerHelmets[2] = PhotonNetwork.Instantiate("HelmetSelection", playerHelmets[2].gameObject.transform.position, playerHelmets[2].gameObject.transform.rotation);
                    helmetPhotonViews[2] = playerHelmets[2].GetComponent<PhotonView>();
                }
                else if (PhotonNetwork.PlayerList[i] == PhotonNetwork.PlayerList[3])
                {
                    playerHelmets[3] = PhotonNetwork.Instantiate("HelmetSelection", playerHelmets[3].gameObject.transform.position, playerHelmets[3].gameObject.transform.rotation);
                    helmetPhotonViews[3] = playerHelmets[3].GetComponent<PhotonView>();
                }
        }

        if (!PhotonNetwork.IsMasterClient)
            return;
        
        helmetPhotonViews[0].RPC("RandomHelmets", RpcTarget.All, Random.Range(0, helmetStorage.Length));
    }

    void SetPlayerHelmet(Player player, int helmet)
    {
        ExitGames.Client.Photon.Hashtable customProperties = new ExitGames.Client.Photon.Hashtable() { { "helmet", helmet } };
        player.SetCustomProperties(customProperties);
    }

    [PunRPC]
    void RandomHelmets(int randomNumber)
    {
        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            playerHelmetPos[i] = randomNumber;
            playerHelmets[i].GetComponent<SpriteRenderer>().sprite = helmetStorage[randomNumber];
        }
    }

    [PunRPC]
    void SyncHelmet(int player, int helmetPos)
    {
        playerHelmets[player].GetComponent<SpriteRenderer>().sprite = helmetStorage[playerHelmetPos[helmetPos]];
    }

    #region Botones

    public void Player01Left()
    {
        if(playerHelmetPos[0] == 0)
        {
            playerHelmetPos[0] = helmetStorage.Length-1;
        }
        else
        {
            playerHelmetPos[0]--;
        }

        SyncHelmet(0, playerHelmetPos[0]);
        //playerHelmets[0].sprite = helmetStorage[playerHelmetPos[0]];
    }

    public void Player01Right()
    {
        if (playerHelmetPos[0] == helmetStorage.Length-1)
        {
            playerHelmetPos[0] = 0;
        }
        else
        {
            playerHelmetPos[0]++;
        }
        SyncHelmet(0, playerHelmetPos[0]);
        //playerHelmets[0].sprite = helmetStorage[playerHelmetPos[0]];
    }

    public void Player02Left()
    {
        if (playerHelmetPos[1] == 0)
        {
            playerHelmetPos[1] = helmetStorage.Length-1;
        }
        else
        {
            playerHelmetPos[1]--;
        }

        SyncHelmet(1, playerHelmetPos[1]);
        //playerHelmets[1].sprite = helmetStorage[playerHelmetPos[1]];
    }

    public void Player02Right()
    {
        if (playerHelmetPos[1] == helmetStorage.Length-1)
        {
            playerHelmetPos[1] = 0;
        }
        else
        {
            playerHelmetPos[1]++;
        }

        SyncHelmet(1, playerHelmetPos[1]);
        //playerHelmets[1].sprite = helmetStorage[playerHelmetPos[1]];
    }

    public void Player03Left()
    {
        if (playerHelmetPos[2] == 0)
        {
            playerHelmetPos[2] = helmetStorage.Length-1;
        }
        else
        {
            playerHelmetPos[2]--;
        }

        SyncHelmet(2, playerHelmetPos[2]);
        //playerHelmets[2].sprite = helmetStorage[playerHelmetPos[2]];
    }

    public void Player03Right()
    {
        if (playerHelmetPos[2] == helmetStorage.Length-1)
        {
            playerHelmetPos[2] = 0;
        }
        else
        {
            playerHelmetPos[2]++;
        }

        SyncHelmet(2, playerHelmetPos[2]);
        //playerHelmets[2].sprite = helmetStorage[playerHelmetPos[2]];
    }

    public void Player04Left()
    {
        if (playerHelmetPos[3] == 0)
        {
            playerHelmetPos[3] = helmetStorage.Length-1;
        }
        else
        {
            playerHelmetPos[3]--;
        }

        SyncHelmet(3, playerHelmetPos[3]);
        //playerHelmets[3].sprite = helmetStorage[playerHelmetPos[3]];
    }

    public void Player04Right()
    {
        if (playerHelmetPos[3] == helmetStorage.Length-1)
        {
            playerHelmetPos[3] = 0;
        }
        else
        {
            playerHelmetPos[3]++;
        }

        SyncHelmet(3, playerHelmetPos[3]);
        //playerHelmets[3].sprite = helmetStorage[playerHelmetPos[3]];
    }

    // TODO: 1. Asignar botones. 2. Realizar notificación por Photon.Pun. 3. Que solo los jugadores puedan ver SUS botones.

    #endregion
}
