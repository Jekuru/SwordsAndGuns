using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class WeaponSpawnerSync : MonoBehaviour
{
    [SerializeField] private List<Transform> weaponSpawners;
    [SerializeField] private List<SpawnerController.WeaponTypes> spawnedWeapons;

    private PhotonView playerView;

    private void Awake()
    {
        if (PhotonNetwork.IsMasterClient)
            playerView = gameObject.GetComponent<PhotonView>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }


    // Update is called once per frame
    void Update()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            AddMapSpawns();

            playerView.RPC("WeaponSpawner", RpcTarget.All);
        }
    }

    private void AddMapSpawns()
    {
        SpawnerController[] spawnerControllers = FindObjectsOfType<SpawnerController>();
        foreach (var spawn in spawnerControllers)
        {
            if (!weaponSpawners.Contains(spawn.transform))
                weaponSpawners.Add(spawn.transform);
        }
    }

    void GenerateWeaponTypeNumber()
    {
        for (int i = 0; i < weaponSpawners.Count; i++)
        {
            spawnedWeapons[i] = (SpawnerController.WeaponTypes)Random.Range(1, System.Enum.GetValues(typeof(SpawnerController.WeaponTypes)).Length);
        }
    }

    void SyncWeaponSpawners()
    {
        playerView.RPC("GenerateWeapon", RpcTarget.All, spawnedWeapons);
    }

    [PunRPC]
    void GenerateWeapon()
    {
        for (int i = 0; i < weaponSpawners.Count; i++)
        {
            weaponSpawners[i].GetComponent<SpawnerController>().currentWeapon = spawnedWeapons[i];
        }
    }

    // Llamar cuando se cambie de mapa
    public void RemoveMapSpawns()
    {
        weaponSpawners.Clear();
    }
}
