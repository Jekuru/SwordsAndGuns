using UnityEngine;
using Photon.Pun;


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
            PhotonNetwork.LoadLevel(Random.Range(2, 4));
        }
    }
}
