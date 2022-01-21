using UnityEngine;

public class TeleportBack : MonoBehaviour
{
    // Start is called before the first frame update

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.transform.SetPositionAndRotation(new Vector2(-1.96f, -2.36f), Quaternion.identity);
            Debug.Log("Trigger");
        }
    }
}
