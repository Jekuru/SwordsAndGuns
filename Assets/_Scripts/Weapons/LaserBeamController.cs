using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Controla únicamente el dibujado del rayo. NO se encarga del daño a jugadores.
public class LaserBeamController : MonoBehaviour
{
    private LineRenderer laserLineRenderer; // Componente que dibuja la linea para la raygun
    private bool laserTimerBool; // Booleana que activa el contador para que desaparezca el dibujo
    [SerializeField] private float timer = 1; // Tiempo para que desaparezca el rayo
    private float laserTimer; // Tiempo actual

    private bool instantiated; // Booleana que indica cuando se ejecuta el rayo, sólo una vez.

    private void Awake()
    {
        laserLineRenderer = gameObject.GetComponent<LineRenderer>();

        Vector3[] initLaserPositions = new Vector3[2] { Vector2.zero, Vector2.zero };
        laserLineRenderer.SetPositions(initLaserPositions);
        laserLineRenderer.startWidth = 0.10f;
        laserLineRenderer.endWidth = 0.10f;
    }

    // Start is called before the first frame update
    void Start()
    {
        instantiated = true;   
    }

    // Update is called once per frame
    void Update()
    {
        InstantiateBeam();
        Timer();
    }

    private void InstantiateBeam()
    {
        if (instantiated)
        {
            laserLineRenderer.SetPosition(1, new Vector3(100f, 0, 0));
            laserLineRenderer.enabled = true;
            laserTimerBool = true;

            RaycastHit2D[] hits = Physics2D.RaycastAll(origin: gameObject.transform.position, direction: gameObject.transform.right, distance: 100F);

            for (int i = 0; i < hits.Length; i++)
            {

                RaycastHit2D hit = hits[i];
                if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Ground"))
                {
                    laserLineRenderer.SetPosition(1, new Vector3(hit.distance, 0, 0));
                    instantiated = false;
                    return;
                }
            }
            instantiated = false;
        }
    }

    private void Timer()
    {
        if (laserTimerBool)
        {
            laserTimer += Time.deltaTime;

            if (laserTimer >= timer)
            {
                laserLineRenderer.enabled = false;
                if(laserTimer >= timer + 2)
                {
                    laserTimer = 0;
                    laserTimerBool = false;
                    Destroy(gameObject);
                }
                
            }
        }
    }
}
