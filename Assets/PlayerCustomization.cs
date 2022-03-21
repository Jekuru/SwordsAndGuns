using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerCustomization : MonoBehaviour
{

    [SerializeField] private SpriteRenderer helmet;
    [SerializeField] private Sprite[] helmets;
    [SerializeField] private int helmetPosition;

    private PhotonView photonView;

    private void Awake()
    {
        photonView = GetComponent<PhotonView>();
    }

    void Start()
    {
        if (!photonView.IsMine)
            return;

            helmetPosition = (int)PhotonNetwork.LocalPlayer.CustomProperties["Helmet"];
            photonView.RPC("LoadHelmet", RpcTarget.All, helmetPosition);
    }

    [PunRPC]
    void LoadHelmet(int helmetPosition)
    {
        helmet.sprite = helmets[helmetPosition];
    }
}
