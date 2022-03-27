using UnityEngine;
using Photon.Pun;
using TMPro;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using System.Collections;
using Photon.Realtime;

public class SelectionController : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject[] gameObjectPositions;

    [SerializeField] private TMP_Text customText;
    [SerializeField] private GameObject commenceButton;
    [SerializeField] private GameObject loadingGif;

    private bool bLoading = false;

    private void Awake()
    {
        GameObject networkController = GameObject.FindGameObjectWithTag("NetworkController");
        Destroy(networkController);
    }

    private void Start()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            commenceButton.SetActive(true);
        }
            

        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            if (PhotonNetwork.PlayerList[i] == PhotonNetwork.LocalPlayer)
                SpawnSelector(i);
        }
    }

    private void SpawnSelector(int position)
    {
        PhotonNetwork.Instantiate("PlayerSelection", gameObjectPositions[position].transform.position, gameObjectPositions[position].transform.rotation);
    }

    public void StartGameButton()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
            {
                Hashtable hash = new Hashtable();
                hash.Add("Score", 0);
                PhotonNetwork.PlayerList[i].SetCustomProperties(hash);
                PlayerPrefs.SetInt("Rounds", 0);
                if (!bLoading)
                    photonView.RPC("PunStartingGame", RpcTarget.All);
            }
        }
    }

    IEnumerator StartGame()
    {
        bLoading = true;
        commenceButton.SetActive(false);
        loadingGif.SetActive(true);
        customText.text = "Comenzando partida...";
        yield return new WaitForSeconds(3);
        int firstMap = Random.Range(2, 6);
        PlayerPrefs.SetInt("PreviousMap", firstMap);
        if(PhotonNetwork.IsMasterClient)
            PhotonNetwork.LoadLevel(firstMap);
    }

    [PunRPC]
    void PunStartingGame()
    {
        StartCoroutine(StartGame());
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            commenceButton.SetActive(true);
        }
            
    }
}
