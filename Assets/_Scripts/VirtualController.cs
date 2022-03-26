using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirtualController : MonoBehaviour
{
    [SerializeField] private GameObject virtualController;

    private PhotonView photonView;

    private void Awake()
    {
        photonView = GetComponent<PhotonView>();
    }

    private void Start()
    {
        if (!photonView.IsMine)
            return;

        if (SystemInfo.deviceType == DeviceType.Handheld)
        {
            virtualController.SetActive(true);
        }
        else
        {
            Debug.Log("No estás jugando en movil!");
        }
            
    }
}
