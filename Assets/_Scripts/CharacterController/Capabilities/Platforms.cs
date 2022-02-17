using System.Collections;
using UnityEngine.Tilemaps;
using UnityEngine;

public class Platforms : MonoBehaviour
{
    private Controller controller;
    private GameObject platform; // Plataforma
    private CapsuleCollider2D playerCollider; // Collider2D del jugador

    /**
     * El script identifica la plataforma y al pulsar "Abajo" se deshabilita la colisión entre la plataforma y el jugador
     */

    private void Awake()
    {
        controller = GetComponent<Controller>();
        playerCollider = GetComponent<CapsuleCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (controller.input.RetrieveVerticalInput() == -1)
        {
            if (platform != null)
            {
                StartCoroutine(DisableCollision());
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Platform"))
        {
            platform = collision.gameObject;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Platform"))
        {
            platform = null;
        }
    }

    private IEnumerator DisableCollision()
    {
        TilemapCollider2D platformCollider = platform.GetComponent<TilemapCollider2D>();

        Physics2D.IgnoreCollision(playerCollider, platformCollider);
        yield return new WaitForSeconds(0.25f);
        Physics2D.IgnoreCollision(playerCollider, platformCollider, false);
    }
}
