using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraController : MonoBehaviour
{

    [SerializeField] private List<Transform> players;

    [SerializeField] private Vector3 offset;
    [SerializeField] private float smoothTime = 0.5f;

    [SerializeField] private float minZoom = 40f;
    [SerializeField] private float maxZoom = 10f;
    
    private float zoomLimiter = 50f;
    private Vector3 velocity;
    private Camera cam;

    private void Start()
    {
        cam = GetComponent<Camera>();
    }

    private void Update()
    {
        RemoveDead();
        AddPlayers();
    }

    private void LateUpdate()
    {
        if (players.Count == 0)
        {
            return;
        }

        CameraMovement();
        CameraZoom();
    }

    private void AddPlayers()
    {
        PlayerStats[] playerStats = FindObjectsOfType<PlayerStats>();
        foreach (var player in playerStats)
        {
            if(!player.isDead && !players.Contains(player.transform))
                players.Add(player.transform);
        }
    }

    /**
     * Elimina de la lista a los jugadores muertos. Evita que siga a jugadores que salgan despedidos y fallos en el script.
     */
    private void RemoveDead()
    {
        for (int i = 0; i < players.Count; i++)
        {
            if (players[i].GetComponent<PlayerStats>().isDead)
            {
                players.Remove(players[i]);
            }
        }
    }

    /**
     * Controla el FOV de la cámera según la posición del jugador que está más a la izquierda de la pantalla y más a la derecha
     */
    private void CameraZoom()
    {
        float newZoom = Mathf.Lerp(maxZoom, minZoom, GetGreatestDistance() / zoomLimiter);
        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, newZoom, Time.deltaTime);
    }

    /**
     * Mueve la cámara al centro de la pantalla, siendo el centro la posición entre todos los jugadores establecidos.
     */
    private void CameraMovement()
    {
        Vector3 centerPoint = GetCenterPoint();

        Vector3 newPosition = centerPoint + offset;

        transform.position = Vector3.SmoothDamp(transform.position, newPosition, ref velocity, smoothTime);
    }

    /**
     * Obtiene la distancia entre el jugador más a la izquierda de la pantalla del que está más a la derecha. Se utiliza para calcular el zoom.
     */
    private float GetGreatestDistance()
    {
        var bounds = new Bounds(players[0].position, Vector3.zero);
        for (int i = 0; i < players.Count; i++)
        {
            bounds.Encapsulate(players[i].position);
        }

        return bounds.size.x;
    }

    /**
     * Calcula el centro entre todos los jugadores añadidos a la lista.
     */
    private Vector3 GetCenterPoint()
    {
        if(players.Count == 1)
        {
            return players[0].position;
        }

        var bounds = new Bounds(players[0].position, Vector3.zero);
        for (int i = 0; i < players.Count; i++)
        {
            bounds.Encapsulate(players[i].position);
        }

        return bounds.center;
    }
}
