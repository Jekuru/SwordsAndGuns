using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class SelectionController : MonoBehaviour
{
    [SerializeField] private PlayerPreferences playerPreferences;

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
    [SerializeField] private int[] playerHelmetPos;
    [SerializeField] private Image[] playerHelmets;
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

        RandomHelmets(Random.Range(0, helmetStorage.Length));
    }

    [PunRPC]
    void RandomHelmets(int randomNumber)
    {
        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            playerHelmetPos[i] = randomNumber;
            playerHelmets[i].sprite = helmetStorage[randomNumber];
        }
    }

    void SyncHelmet()
    {
        
    }

    #region Botones

    public void Player01Left()
    {
        if(playerHelmetPos[0] == 0)
        {
            playerHelmetPos[0] = helmetStorage.Length;
        } else
        {
            playerHelmetPos[0]--;
        }
        
        playerHelmets[0].sprite = helmetStorage[playerHelmetPos[0]];
    }

    public void Player01Right()
    {
        if (playerHelmetPos[0] == 0)
        {
            playerHelmetPos[0] = 0;
        }
        else
        {
            playerHelmetPos[0]++;
        }

        playerHelmets[0].sprite = helmetStorage[playerHelmetPos[0]];
    }

    public void Player02Left()
    {
        if (playerHelmetPos[1] == 0)
        {
            playerHelmetPos[1] = helmetStorage.Length;
        }
        else
        {
            playerHelmetPos[1]--;
        }

        playerHelmets[1].sprite = helmetStorage[playerHelmetPos[1]];
    }

    public void Player02Right()
    {
        if (playerHelmetPos[1] == 0)
        {
            playerHelmetPos[1] = 0;
        }
        else
        {
            playerHelmetPos[1]++;
        }

        playerHelmets[1].sprite = helmetStorage[playerHelmetPos[1]];
    }

    public void Player03Left()
    {
        if (playerHelmetPos[2] == 0)
        {
            playerHelmetPos[2] = helmetStorage.Length;
        }
        else
        {
            playerHelmetPos[2]--;
        }

        playerHelmets[2].sprite = helmetStorage[playerHelmetPos[2]];
    }

    public void Player03Right()
    {
        if (playerHelmetPos[2] == 0)
        {
            playerHelmetPos[2] = 0;
        }
        else
        {
            playerHelmetPos[2]++;
        }

        playerHelmets[2].sprite = helmetStorage[playerHelmetPos[2]];
    }

    public void Player04Left()
    {
        if (playerHelmetPos[3] == 0)
        {
            playerHelmetPos[3] = helmetStorage.Length;
        }
        else
        {
            playerHelmetPos[3]--;
        }

        playerHelmets[3].sprite = helmetStorage[playerHelmetPos[3]];
    }

    public void Player04Right()
    {
        if (playerHelmetPos[3] == 0)
        {
            playerHelmetPos[3] = 0;
        }
        else
        {
            playerHelmetPos[3]++;
        }

        playerHelmets[3].sprite = helmetStorage[playerHelmetPos[3]];
    }

    // TODO: 1. Asignar botones. 2. Realizar notificación por Photon.Pun. 3. Que solo los jugadores puedan ver SUS botones.

    #endregion
}
