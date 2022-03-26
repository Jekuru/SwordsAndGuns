using UnityEngine;
using Photon.Pun;

public class Move : MonoBehaviour
{
    private InputController input = null;
    [SerializeField, Range(0f, 100f)] private float maxSpeed = 4f; // VELOCIDAD HORIZONTAL
    [SerializeField, Range(0f, 100f)] private float maxAcceleration = 35f; // ACCELERACION MÁXIMA
    [SerializeField, Range(0f, 100f)] private float maxAirAcceleration = 20f; // ACCELERACIÓN MÁXIMA VERTICAL

    private Vector2 direction;
    private Vector2 desiredVelocity;
    private Vector2 velocity;
    private Rigidbody2D body;
    private Ground ground;

    private float maxSpeedChange;
    private float acceleration;
    public bool onGround;

    [SerializeField] private Joystick joystick;

    private PhotonView photonView;

    // Start is called before the first frame update
    void Awake()
    {
        photonView = GetComponent<PhotonView>();
        if (!photonView.IsMine)
            return;

        input = GetComponent<Controller>().input;
        body = GetComponent<Rigidbody2D>();
        ground = GetComponent<Ground>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!photonView.IsMine)
            return;

        MoveInputs();
    }

    private void FixedUpdate()
    {
        if (!photonView.IsMine)
            return;

        MoveChecks();
    }

    private void MoveInputs()
    {
        if (SystemInfo.deviceType == DeviceType.Handheld)
            direction.x = joystick.Horizontal;

        if (SystemInfo.deviceType != DeviceType.Handheld)
            direction.x = input.RetrieveMoveInput();

        desiredVelocity = new Vector2(direction.x, 0f) * Mathf.Max(maxSpeed - ground.GetFriction(), 0f);
    }

    private void MoveChecks()
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
     * INVIERTE LA ROTACIÓN DEL PERSONAJE AL DESPLAZARSE HACIA LA IZQUIERDA Y DERECHA
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