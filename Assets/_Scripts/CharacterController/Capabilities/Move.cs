using UnityEngine;

public class Move : MonoBehaviour
{
    [SerializeField] private InputController input = null;
    [SerializeField, Range(0f, 100f)] private float maxSpeed = 4f; // VELOCIDAD HORIZONTAL
    [SerializeField, Range(0f, 100f)] private float maxAcceleration = 35f; // ACCELERACION M�XIMA
    [SerializeField, Range(0f, 100f)] private float maxAirAcceleration = 20f; // ACCELERACI�N M�XIMA VERTICAL

    private Vector2 direction;
    private Vector2 desiredVelocity;
    private Vector2 velocity;
    private Rigidbody2D body;
    private Ground ground;

    private float maxSpeedChange;
    private float acceleration;
    private bool onGround;

    private Vector3 characterScale;
    private float characterScaleX;


    // Start is called before the first frame update
    void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        ground = GetComponent<Ground>();
        characterScale = transform.localScale;
        characterScaleX = characterScale.x;
    }

    // Update is called once per frame
    void Update()
    {
        direction.x = input.RetrieveMoveInput();
        desiredVelocity = new Vector2(direction.x, 0f) * Mathf.Max(maxSpeed - ground.GetFriction(), 0f);
    }

    private void FixedUpdate()
    {
        FaceMoveDirection();

        onGround = ground.GetOnGround();
        velocity = body.velocity;

        acceleration = onGround ? maxAcceleration : maxAirAcceleration;
        maxSpeedChange = acceleration * Time.deltaTime;
        velocity.x = Mathf.MoveTowards(velocity.x, desiredVelocity.x, maxSpeedChange);

        body.velocity = velocity;
    }

    /**
     * INVIERTE LA ROTACI�N DEL PERSONAJE AL DESPLAZARSE HACIA LA IZQUIERDA Y DERECHA
     */
    private void FaceMoveDirection()
    {
        if (direction.x > 0f)
        {
            transform.localRotation = Quaternion.Euler(0, 0, 0);
        }
        else if (direction.x < 0f)
        {
            transform.localRotation = Quaternion.Euler(0, 180, 0);
        }
    }
}