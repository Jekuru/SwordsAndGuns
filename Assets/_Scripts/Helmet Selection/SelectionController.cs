using UnityEngine;
using Photon.Pun;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class SelectionController : MonoBehaviour
{
    [SerializeField] GameObject[] gameObjectPositions;

    private void Start()
    {
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
            }
            int firstMap = Random.Range(2, 4);
            PlayerPrefs.SetInt("PreviousMap", firstMap);
            PhotonNetwork.LoadLevel(firstMap);
        }
    }
}
