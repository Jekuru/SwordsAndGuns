using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using TMPro;

public class LevelManager : MonoBehaviour
{
    [SerializeField] public int position;
    [SerializeField] public List<SpawnPoints> spawnPoints;
    [SerializeField] private List<PlayerStats> players;

    [SerializeField] private float timeStart;
    [SerializeField] private float maxTimeStart = 5f;

    [SerializeField] private GameObject canvasLeaderboard;
    [SerializeField] private TMP_Text player01Score;
    [SerializeField] private TMP_Text player02Score;
    [SerializeField] private TMP_Text player03Score;
    [SerializeField] private TMP_Text player04Score;

    private Player playerAlive;

    private void Update()
    {
        StartTimer();

    }

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            if(PhotonNetwork.PlayerList[i] == PhotonNetwork.LocalPlayer)
                InstatiateSpawn(i);
        }
    }

    private void InstatiateSpawn(int position)
    {
        GameObject player = PhotonNetwork.Instantiate("Player", spawnPoints[position].transform.position, Quaternion.identity, 0);
        player.name = PhotonNetwork.LocalPlayer.NickName;
        players.Add(player.GetComponent<PlayerStats>());
    }

    private void StartTimer()
    {
        timeStart += Time.deltaTime;

        if (timeStart > maxTimeStart)
        {
            DeadCheck();
        }
    }

    private void DeadCheck()
    {
        for (int i = 0; i < players.Count; i++)
        { 
            if(players.Count == 1) // Solo queda uno con vida
            {
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
                FinishRound(); // Finalizar ronda
            }
            else if(players.Count <= 0)
            {
                // Todos los jugadores muertos...
                Debug.LogWarning("Todos los jugadores han muerto!!");
            }
            else 
            {
                if (players[i].isDead) // Quitar de la lista
                    players.RemoveAt(i);
            }
        }
    }

    [PunRPC]
    private void FinishRound()
    {
        if (!PhotonNetwork.IsMasterClient)
            return;

        // Sumarle el punto y conservarlo hasta el final de la partida
        Hashtable hash = new Hashtable();
        hash.Add("Score", +1);
        playerAlive.SetCustomProperties(hash);

        // Cargar el siguiente mapa de fondo mientras los jugadores ven la puntuación (5 secs?)
        

        // Mostrar leaderboard y sumarle el punto


    }

    IEnumerator loadNewMap()
    {
        yield return new WaitForSeconds(5);
        PhotonNetwork.LoadLevel(Random.Range(2, 5));
    }
}
