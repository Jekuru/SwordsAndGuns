using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;

// DE PRUEBAS PARA LA PRESENTACIÓN DEL 21 DE FEBRERO
public class LevelManager : MonoBehaviour
{
    [SerializeField] public List<SpawnPoints> spawnPoints;
    [SerializeField] private List<PlayerStats> players;

    [SerializeField] private float timeStart;
    [SerializeField] private float maxTimeStart = 5f;

    private int deadPlayers;

    private void Update()
    {
        Timer();

    }

    // Start is called before the first frame update
    void Start()
    {
        int randomSpawner = Random.Range(0, spawnPoints.Count);
        if (spawnPoints[randomSpawner].active)
        {
            GameObject player = PhotonNetwork.Instantiate("Player", spawnPoints[randomSpawner].transform.position, Quaternion.identity, 0);
            players.Add(player.GetComponent<PlayerStats>());
            
            spawnPoints[randomSpawner].active = false;
        }
    }

    private void Timer()
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
            if (players[i].isDead)
            {
                deadPlayers++;
                players.RemoveAt(i);
            }

            if(deadPlayers <= 1)
            {
                FinishRound();
            }
        }
    }

    private void FinishRound()
    {
        // TODO: Saber quien esta con vida.
        // Sumarle el punto y conservarlo hasta el final de la partida
        // Cargar el siguiente mapa de fondo mientras los jugadores ven la puntuación (5 secs?)
    }
}
