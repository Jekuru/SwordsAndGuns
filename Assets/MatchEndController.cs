using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using Photon.Pun;
using Photon.Realtime;
using System.Linq;

public class MatchEndController : MonoBehaviour
{
    [Header("Datos leaderboard")]
    [SerializeField] private Image[] playersHelmet;
    [SerializeField] private TMP_Text[] playersNickName;
    [SerializeField] private TMP_Text[] playersVictories;
    [SerializeField] private List<UserScore> userScore = new List<UserScore>();

    [SerializeField] private Sprite[] helmets;

    [SerializeField] private GameObject rematchButton;

    [SerializeField] private GameObject loadingRing;

    private PhotonView photonView;

    private void Awake()
    {
        photonView = GetComponent<PhotonView>();
    }

    private void Update()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            userScore.Add(new UserScore(PhotonNetwork.PlayerList[i], (int)PhotonNetwork.PlayerList[i].CustomProperties["Score"]));
        }
        userScore = userScore.OrderByDescending(x => x.score).ToList();

        for (int i = 0; i < userScore.Count; i++)
        {
            playersHelmet[i].sprite = helmets[(int)userScore[i].player.CustomProperties["Helmet"]];
            playersNickName[i].text = userScore[i].player.NickName;
            playersVictories[i].text = userScore[i].player.CustomProperties["Score"].ToString();
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
        yield return new WaitForSeconds(6);
        PhotonNetwork.LoadLevel(Random.Range(2, 5));
    }

    [PunRPC]
    void ActivateLoadingIndicator()
    {
        loadingRing.SetActive(true);
    }

    public void ExitButton()
    {
        PhotonNetwork.Disconnect();
        SceneManager.LoadScene("Menu");
    }
}
