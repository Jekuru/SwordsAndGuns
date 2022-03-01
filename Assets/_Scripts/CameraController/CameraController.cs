using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraController : MonoBehaviour
{

    [SerializeField] private List<Transform> players; // Establecer desde el inspector

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
    }

    private void LateUpdate()
    {
        if (players.Count == 0)
        {
            Debug.LogError("A�adir jugadores desde el inspector!");
            return;
        }

        CameraMovement();
        CameraZoom();
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
     * Controla el FOV de la c�mera seg�n la posici�n del jugador que est� m�s a la izquierda de la pantalla y m�s a la derecha
     */
    private void CameraZoom()
    {
        float newZoom = Mathf.Lerp(maxZoom, minZoom, GetGreatestDistance() / zoomLimiter);
        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, newZoom, Time.deltaTime);
    }

    /**
     * Mueve la c�mara al centro de la pantalla, siendo el centro la posici�n entre todos los jugadores establecidos.
     */
    private void CameraMovement()
    {
        Vector3 centerPoint = GetCenterPoint();

        Vector3 newPosition = centerPoint + offset;

        transform.position = Vector3.SmoothDamp(transform.position, newPosition, ref velocity, smoothTime);
    }

    /**
     * Obtiene la distancia entre el jugador m�s a la izquierda de la pantalla del que est� m�s a la derecha. Se utiliza para calcular el zoom.
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
     * Calcula el centro entre todos los jugadores a�adidos a la lista.
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
