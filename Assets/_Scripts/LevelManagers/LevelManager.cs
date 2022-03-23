using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using TMPro;

public class LevelManager : MonoBehaviour
{
    [Header("Jugadores con vida")]
    [SerializeField] private List<PlayerStats> players;
    private Player playerAlive; // Ultimo jugador con vida

    [Header("Puntos de spawn")]
    [SerializeField] public int position;
    [SerializeField] public List<SpawnPoints> spawnPoints;
    
    [Header("Inicio de ronda")]
    [SerializeField] private float timeStart;
    [SerializeField] private float maxTimeStart = 5f;

    [Header("Tabla de puntuaciones")]
    [SerializeField] private GameObject canvasLeaderboard;
    [SerializeField] private Image[] playerHelmet;
    [SerializeField] private Sprite[] availableHelmets;
    [SerializeField] private TMP_Text[] playerNickName;
    [SerializeField] private TMP_Text[] playerScore;

    private PhotonView photonView;

    public int playerCount;

    private void Awake()
    {
        photonView = GetComponent<PhotonView>();

        if(spawnPoints == null)
        {
            Debug.LogWarning("��A�adir spawns desde la jerarqu�a!!");
            return;
        }
    }

    private void Update()
    {
        playerCount = players.Count;
        if (!PhotonNetwork.IsMasterClient && !photonView.IsMine)
            return;

        StartTimer();
        photonView.RPC("CountPlayersAlive", RpcTarget.All);
    }

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            if (PhotonNetwork.PlayerList[i] == PhotonNetwork.LocalPlayer)
                InstatiateSpawn(i);
        }
    }

    private void InstatiateSpawn(int position)
    {
        PhotonNetwork.Instantiate("Player", spawnPoints[position].transform.position, Quaternion.identity);
    }

    private void StartTimer()
    {
        timeStart += Time.deltaTime;

        if (timeStart > maxTimeStart)
        {
            photonView.RPC("DeadCheck", RpcTarget.All);
        }
    }

    [PunRPC]
    void CountPlayersAlive()
    {
        PlayerStats[] playerStats = FindObjectsOfType<PlayerStats>();
        foreach (var player in playerStats)
        {
            if (!player.isDead && !players.Contains(player))
                players.Add(player);
        }
    }

    [PunRPC]
    private void DeadCheck()
    {
        if (players.Count <= 0)
            return;

        if (players.Count == 1) // Solo queda uno con vida
        {
            Debug.Log("Solo uno con vida!");
            for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++) // Buscar quien queda vivo
            {
                if (players[0].name == PhotonNetwork.PlayerList[i].NickName)
                {
                    playerAlive = PhotonNetwork.PlayerList[i]; // Jugador vivo
                    Debug.Log("El jugador vivo es " + playerAlive.NickName);
                }
            }
            if (PhotonNetwork.IsMasterClient)
            {
                Debug.Log("Finalizar ronda...");
                photonView.RPC("FinishRound", RpcTarget.All);
            }
            return;
        }


        for (int i = 0; i < players.Count; i++)
        {
            if (players.Count <= 0) // Todos los jugadores muertos...
            {
                Debug.LogWarning("Todos los jugadores han muerto!!");
                // TODO: Siguiente ronda sin sumar puntuaci�n
                return;
            }

            if (players[i].isDead)  // Quitar muerto de la lista de jugadores vivos
            {
                players.RemoveAt(i);
                Debug.Log("Quitando jugador " + PhotonNetwork.PlayerList[i].NickName + " de la lista.");
            }
        }
    }

    [PunRPC]
    private void FinishRound()
    {
        if (!PhotonNetwork.IsMasterClient)
            return;

        // Cargar la leaderboard con info, ocultar donde no hay jugadores
            switch (PhotonNetwork.PlayerList.Length)
            {
            case 2:
                for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
                {
                    Debug.Log("Cargar datos de leaderboard para jugador " + PhotonNetwork.PlayerList[i]);
                    playerHelmet[i].sprite = availableHelmets[(int)PhotonNetwork.PlayerList[i].CustomProperties["Helmet"]];
                    playerNickName[i].text = PhotonNetwork.PlayerList[i].NickName;
                    playerScore[i].text = PhotonNetwork.PlayerList[i].CustomProperties["Score"].ToString();
                }
                playerHelmet[2].gameObject.SetActive(false);
                playerNickName[2].gameObject.SetActive(false);
                playerScore[2].gameObject.SetActive(false);
                playerHelmet[3].gameObject.SetActive(false);
                playerNickName[3].gameObject.SetActive(false);
                playerScore[3].gameObject.SetActive(false);
                break;
            case 3:
                for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
                {
                    Debug.Log("Cargar datos de leaderboard para jugador " + PhotonNetwork.PlayerList[i]);
                    playerHelmet[i].sprite = availableHelmets[(int)PhotonNetwork.PlayerList[i].CustomProperties["Helmet"]];
                    playerNickName[i].text = PhotonNetwork.PlayerList[i].NickName;
                    playerScore[i].text = PhotonNetwork.PlayerList[i].CustomProperties["Score"].ToString();
                }
                playerHelmet[3].gameObject.SetActive(false);
                playerNickName[3].gameObject.SetActive(false);
                playerScore[3].gameObject.SetActive(false);
                break;
            default:
                for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
                {
                    Debug.Log("Cargar datos de leaderboard para jugador " + PhotonNetwork.PlayerList[i]);
                    playerHelmet[i].sprite = availableHelmets[(int)PhotonNetwork.PlayerList[i].CustomProperties["Helmet"]];
                    playerNickName[i].text = PhotonNetwork.PlayerList[i].NickName;
                    playerScore[i].text = PhotonNetwork.PlayerList[i].CustomProperties["Score"].ToString();
                }
                break;
            }

        // Mostrar leaderboard
        Debug.Log("Mostrar leaderboard");
        canvasLeaderboard.SetActive(true);

        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            if (playerAlive == PhotonNetwork.PlayerList[i])
            {
                // Sumarle el punto y conservarlo hasta el final de la partida
                Debug.Log("Sumar punto en Player Custom Properties");
                Hashtable hash = new Hashtable();
                hash.Add("Score", +1);
                playerAlive.SetCustomProperties(hash);

                // Cambiar resultado en el leaderboard
                SumPoint(i);
            }
        }

        // Cargar el siguiente mapa de fondo mientras los jugadores ven la puntuaci�n (5 secs?)
        LoadNewMap();
    }

    IEnumerator SumPoint(int player)
    {
        yield return new WaitForSeconds(2);
        Debug.Log("Sumar punto en la leaderboard");
        playerScore[player].text = playerAlive.CustomProperties["Score"].ToString();
    }

    IEnumerator LoadNewMap()
    {
        yield return new WaitForSeconds(5);
        Debug.Log("Cargar nuevo mapa...");
        PhotonNetwork.LoadLevel(Random.Range(2, 5));
    }


}
