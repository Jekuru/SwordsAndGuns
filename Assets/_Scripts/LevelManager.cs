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

    // Start is called before the first frame update
    void Start()
    {
        int randomSpawner = Random.Range(0, spawnPoints.Count);
        if (spawnPoints[randomSpawner].active)
        {
            PhotonNetwork.Instantiate("Player", spawnPoints[randomSpawner].transform.position, Quaternion.identity, 0);
            spawnPoints[randomSpawner].active = false;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }


}
