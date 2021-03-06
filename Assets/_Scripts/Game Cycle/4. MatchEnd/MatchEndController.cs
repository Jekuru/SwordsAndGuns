using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using Photon.Pun;
using System.Linq;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class MatchEndController : MonoBehaviourPunCallbacks
{
    [Header("Datos leaderboard")]
    [SerializeField] private Image[] playersHelmet;
    [SerializeField] private Image playerBody;
    [SerializeField] private TMP_Text[] playersNickName;
    [SerializeField] private TMP_Text[] playersVictories;
    [SerializeField] private GameObject[] playersPositionText;
    [SerializeField] private List<UserScore> userScore = new List<UserScore>();

    [SerializeField] private Sprite[] helmets;

    [SerializeField] private GameObject rematchButton;
    [SerializeField] private GameObject exitButton;

    [SerializeField] private GameObject waitingForHostText;
    [SerializeField] private GameObject playerLeftText;
    [SerializeField] private GameObject loadingRing;

    // Start is called before the first frame update
    void Start()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            rematchButton.SetActive(true);
            waitingForHostText.SetActive(false);
        }
            

        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            userScore.Add(new UserScore(PhotonNetwork.PlayerList[i], (int)PhotonNetwork.PlayerList[i].CustomProperties["Score"]));
        }
        userScore = userScore.OrderByDescending(x => x.score).ToList();

        for (int i = 0; i < userScore.Count; i++)
        {
            playersHelmet[i].sprite = helmets[(int)userScore[i].player.CustomProperties["Helmet"]];
            playersNickName[i].text = userScore[i].player.NickName;
            if (userScore[i].player.UserId == PhotonNetwork.PlayerList[0].UserId)
            {
                playersNickName[i].color = Color.red;
                if(userScore.First().player == PhotonNetwork.PlayerList[0])
                {
                    playerBody.color = Color.red;
                }
            }
            else if (userScore[i].player.UserId == PhotonNetwork.PlayerList[1].UserId)
            {
                playersNickName[i].color = Color.blue;
                if (userScore.First().player == PhotonNetwork.PlayerList[1])
                {
                    playerBody.color = Color.blue;
                }
            }
            else if (userScore[i].player.UserId == PhotonNetwork.PlayerList[2].UserId)
            {
                playersNickName[i].color = Color.green;
                if (userScore.First().player == PhotonNetwork.PlayerList[2])
                {
                    playerBody.color = Color.green;
                }
            }
            else if (userScore[i].player.UserId == PhotonNetwork.PlayerList[3].UserId)
            {
                playersNickName[i].color = Color.magenta;
                if (userScore.First().player == PhotonNetwork.PlayerList[3])
                {
                    playerBody.color = Color.magenta;
                }
            }
            playersVictories[i].text = userScore[i].player.CustomProperties["Score"].ToString();
        }
        
        if(userScore.Count == 2)
        {
            playersHelmet[2].gameObject.SetActive(false);
            playersNickName[2].gameObject.SetActive(false);
            playersVictories[2].gameObject.SetActive(false);
            playersPositionText[1].SetActive(false);
            playersHelmet[3].gameObject.SetActive(false);
            playersNickName[3].gameObject.SetActive(false);
            playersVictories[3].gameObject.SetActive(false);
            playersPositionText[2].SetActive(false);
        }
        if(userScore.Count == 3)
        {
            playersHelmet[3].gameObject.SetActive(false);
            playersNickName[3].gameObject.SetActive(false);
            playersVictories[3].gameObject.SetActive(false);
            playersPositionText[2].SetActive(false);
        }
    }

    public void RematchButton()
    {
        StartCoroutine(LoadNewMap());
    }

    IEnumerator LoadNewMap()
    {
        yield return null;
        photonView.RPC("ActivateLoadingIndicator", RpcTarget.All);

        if(PhotonNetwork.IsMasterClient)
            PlayerPrefs.SetInt("Rounds", 0);

        foreach (var player in PhotonNetwork.PlayerList)
        {
            Hashtable hash = new Hashtable();
            hash.Add("Score", 0);
            player.SetCustomProperties(hash);
        }

        yield return new WaitForSeconds(6);
        PhotonNetwork.LoadLevel(Random.Range(2, 5));
    }

    [PunRPC]
    void ActivateLoadingIndicator()
    {
        loadingRing.SetActive(true);
        exitButton.SetActive(false);
    }

    public void ExitButton()
    {
        PhotonNetwork.Disconnect();

    }

    public override void OnDisconnected(Photon.Realtime.DisconnectCause cause)
    {
        SceneManager.LoadScene("Menu");
    }

    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        playerLeftText.SetActive(true);
        waitingForHostText.SetActive(false);
        rematchButton.SetActive(false);
    }
}
