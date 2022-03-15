using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SpawnerSync : MonoBehaviour
{
    [Header("Spawner ARMAS")]
    [SerializeField] private List<Transform> weaponSpawners;
    [SerializeField] private List<SpawnerController.WeaponTypes> spawnedWeapons;

    [Header("Spawner OBJETOS")]
    [SerializeField] private List<Transform> itemSpawners;
    [SerializeField] private List<ItemSpawnerController.ItemTypes> spawnedItems;

    private PhotonView playerView;

    private void Awake()
    {
        playerView = GetComponent<PhotonView>();            
    }

    private void Start()
    {
        if (!playerView.IsMine && !PhotonNetwork.IsMasterClient)
            return;

        GenerateWeaponTypeNumber();
        SyncWeaponSpawners();
    }

    // Update is called once per frame
    void Update()
    {
        if (!playerView.IsMine && !PhotonNetwork.IsMasterClient)
            return;

        AddMapSpawns();
    }

    private void AddMapSpawns()
    {
        SpawnerController[] spawnerControllers = FindObjectsOfType<SpawnerController>();
        foreach (var spawn in spawnerControllers)
        {
            if (!weaponSpawners.Contains(spawn.transform))
                weaponSpawners.Add(spawn.transform);
        }

        ItemSpawnerController[] itemSpawnerControllers = FindObjectsOfType<ItemSpawnerController>();
        foreach (var spawn in itemSpawnerControllers)
        {
            if (!itemSpawners.Contains(spawn.transform))
                itemSpawners.Add(spawn.transform);
        }
    }

    void GenerateWeaponTypeNumber()
    {
        for (int i = 0; i < weaponSpawners.Count; i++)
        {
            spawnedWeapons[i] = (SpawnerController.WeaponTypes)Random.Range(1, System.Enum.GetValues(typeof(SpawnerController.WeaponTypes)).Length);
        }
    }

    public void SyncWeaponSpawners()
    {
        playerView.RPC("GenerateWeapon", RpcTarget.All);
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
