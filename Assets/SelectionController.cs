using UnityEngine;
using Photon.Pun;


public class SelectionController : MonoBehaviour
{
    [SerializeField] GameObject[] gameObjectPositions;

    private void Start()
    {
        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            if (PhotonNetwork.PlayerList[i].IsLocal)
            {
                switch (i)
                {
                    case 0:
                        // Instanciar en posición 0
                        GameObject playerSelection0 = PhotonNetwork.Instantiate("PlayerSelection", gameObjectPositions[0].transform.position, gameObjectPositions[0].transform.rotation);
                        break;
                    case 1:
                        // Instanciar en posición 1
                        GameObject playerSelection1 = PhotonNetwork.Instantiate("PlayerSelection", gameObjectPositions[1].transform.position, gameObjectPositions[1].transform.rotation);
                        break;
                    case 2:
                        // Instanciar en posición 2
                        GameObject playerSelection2 = PhotonNetwork.Instantiate("PlayerSelection", gameObjectPositions[2].transform.position, gameObjectPositions[2].transform.rotation);
                        break;
                    case 3:
                        // Instanciar en posición 3
                        GameObject playerSelection3 = PhotonNetwork.Instantiate("PlayerSelection", gameObjectPositions[3].transform.position, gameObjectPositions[3].transform.rotation);
                        break;
                    default:
                        Debug.Log("Error en switch al instanciar PlayerSelection");
                        break;
                }
            }
        }
    }

    public void StartGameButton()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.LoadLevel(Random.Range(2, 4));
        }
    }
}
