using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerCustomization : MonoBehaviour
{
    [Header("Cuerpo/Color del cuerpo/Colores")]
    [SerializeField] private SpriteRenderer body;
    [SerializeField] private Color[] colors;
    [SerializeField] private int bodyPosition;
    [Header("Yelmos/Cascos/Sombreros/Cabezas")]
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
        bodyPosition = PlaceBodyColor();
        photonView.RPC("LoadBody", RpcTarget.All, bodyPosition);
        photonView.RPC("LoadHelmet", RpcTarget.All, helmetPosition);
    }

    [PunRPC]
    void LoadBody(int bodyPosition)
    {
        Debug.Log("Compartiendo color del pechito");
        body.material.SetColor("_Color", colors[bodyPosition]);
        //body.color = colors[bodyPosition];
    }

    [PunRPC]
    void LoadHelmet(int helmetPosition)
    {
        Debug.Log("Compartiendo yelmo");
        helmet.sprite = helmets[helmetPosition];
    }

    private int PlaceBodyColor()
    {
        if (PhotonNetwork.LocalPlayer.UserId == PhotonNetwork.PlayerList[0].UserId)
        {
            Debug.Log("Soy rojo");
            return 0;
            
        }
        else if (PhotonNetwork.LocalPlayer.UserId == PhotonNetwork.PlayerList[1].UserId)
        {
            Debug.Log("Soy azul");
            return 1;
            
        }
        else if (PhotonNetwork.LocalPlayer.UserId == PhotonNetwork.PlayerList[2].UserId)
        {
            Debug.Log("Soy verde");
            return 2;
        }
        else if (PhotonNetwork.LocalPlayer.UserId == PhotonNetwork.PlayerList[3].UserId)
        {
            Debug.Log("Soy magenta");
            return 3;
        }
        else
        {
            Debug.Log("Soy un error :)");
            return 0;
        }
    }
}
