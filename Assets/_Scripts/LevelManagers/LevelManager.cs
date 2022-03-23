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

    private void Awake()
    {
        photonView = GetComponent<PhotonView>();

        if(spawnPoints == null)
        {
            Debug.LogWarning("¡¡Añadir spawns desde la jerarquía!!");
            return;
        }
    }

    private void Update()
    {
        if (!PhotonNetwork.IsMasterClient && !photonView.IsMine)
            return;

        StartTimer();
        photonView.RPC("CountPlayersAlive", RpcTarget.All);
        photonView.RPC("DeadCheck", RpcTarget.All);
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

        for (int i = 0; i < players.Count; i++)
        { 
            if(players.Count == 1) // Solo queda uno con vida
            {
                Debug.Log("Solo uno con vida!");
                if (!players[i].isDead) // Saber quien queda vivo
                {
                    for (int u = 0; u < PhotonNetwork.PlayerList.Length; u++)
                    {
                        if(players[u].name == PhotonNetwork.PlayerList[u].NickName)
                        {
                            playerAlive = PhotonNetwork.PlayerList[u]; // Jugador vivo
                        }
                    }
                }
                if (PhotonNetwork.IsMasterClient)
                    photonView.RPC("FinishRound", RpcTarget.All);

                return;
            }
            else if(players.Count <= 0)
            {
                // Todos los jugadores muertos...
                Debug.LogWarning("Todos los jugadores han muerto!!");
            }
            else 
            {
                if (players[i].isDead) {
                    players.RemoveAt(i);
                    Debug.Log("Quitando jugador " + i + " de la lista.");
                } // Quitar de la lista
                    
            }
        }
    }

    [PunRPC]
    private void FinishRound()
    {
        if (!PhotonNetwork.IsMasterClient)
            return;

        // Cargar la leaderboard con info, ocultar donde no hay jugadores
        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            if (PhotonNetwork.PlayerList[i] == null)
            {
                playerHelmet[i].gameObject.SetActive(false);
                playerNickName[i].gameObject.SetActive(false);
                playerScore[i].gameObject.SetActive(false);
            }
            else
            {
                playerHelmet[i].sprite = availableHelmets[(int)PhotonNetwork.PlayerList[i].CustomProperties["Helmet"]];
                playerNickName[i].text = PhotonNetwork.PlayerList[i].NickName;
                playerScore[i].text = (string)PhotonNetwork.PlayerList[i].CustomProperties["Score"];
            }
        }

        // Mostrar leaderboard
        canvasLeaderboard.SetActive(true);

        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            if (playerAlive == PhotonNetwork.PlayerList[i])
            {
                // Sumarle el punto y conservarlo hasta el final de la partida
                Hashtable hash = new Hashtable();
                hash.Add("Score", +1);
                playerAlive.SetCustomProperties(hash);

                // Cambiar resultado en el leaderboard
                SumPoint(i);
            }
        }

        // Cargar el siguiente mapa de fondo mientras los jugadores ven la puntuación (5 secs?)
        LoadNewMap();
    }

    IEnumerator LoadNewMap()
    {
        yield return new WaitForSeconds(5);
        PhotonNetwork.LoadLevel(Random.Range(2, 5));
    }

    IEnumerator SumPoint(int player)
    {
        yield return new WaitForSeconds(2);
        playerScore[player].text = (string)playerAlive.CustomProperties["Score"];
    }
}
