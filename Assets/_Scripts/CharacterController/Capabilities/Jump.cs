using UnityEngine;

[RequireComponent(typeof(Controller))]
public class Jump : MonoBehaviour
{
    [SerializeField, Range(0f, 10f)] private float jumpHeight = 1f; // ALTURA/FUERZA DEL SALTO
    [SerializeField, Range(0, 5)] public int maxAirJumps = 0; // N�MERO DE SALTOS EXTRA QUE PUEDE DAR EL PERSONAJE
    [SerializeField, Range(0f, 5f)] private float downwardMovementMultiplier = 3f; // GRAVEDAD AL CAER
    [SerializeField, Range(0f, 5f)] private float upwardMovementMultiplier = 1.7f; // GRAVEDAD AL SUBIR

    // DECLARACI�N VARIABLES
    private Controller controller;
    private Rigidbody2D body;
    private Ground ground;
    private Vector2 velocity;

    private int jumpPhase; // N�MERO DE SALTOS REALIZADOS EN EL AIRE
    private float defaultGravityScale; // GRAVEDAD POR DEFECTO (inicializaci�n en Awake())

    // VARIABLES PUBLICAS PARA USARLAS MAS ADELANTE CON LAS ANIMACIONES
    public bool isJumping; // INDICA SI EST� SALTANDO
    public bool onGround; // INDICA SI EST� TOCANDO EL SUELO

    public bool holdingJump; // PRESIONANDO TECLA DE SALTO
    public float holdingJumpStart;
    public float holdingJumpTime = 0.1f;
    public float holdForce;

    // Start is called before the first frame update
    void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        ground = GetComponent<Ground>();
        controller = GetComponent<Controller>();

        defaultGravityScale = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        isJumping |= controller.input.RetrieveJumpInput();
        holdingJump |= controller.input.RetrieveJumpInputHold();

        // SI SE SUELTA LA TECLA DE SALTO...
        if (controller.input.RetrieveJumpInputRelease())
        {
            holdingJump = false;
        }
    }

    private void FixedUpdate()
    {
        onGround = ground.GetOnGround();
        velocity = body.velocity;

        // SI SE EST� EN EL SUELO...
        if (onGround)
        {
            jumpPhase = 0;
        }

        // SI SE PULSA LA TECLA DE SALTO...
        if (isJumping)
        {
            JumpAction();
            isJumping = false;

        }

        // SI SE MANTIENE PULSADA LA TECLA DE SALTO... SE A�ADE FUERZA PARA IMPULSAR Y REALIZAR UN SALTO M�S ALTO
        if (holdingJump && holdingJumpStart < holdingJumpTime)
        {
            holdingJumpStart += Time.deltaTime;
            body.AddForce(new Vector2(0, holdForce));
        }

        // APLICAR GRAVEDAD EN LA SUBIDA
        if (body.velocity.y > 0)
        {
            body.gravityScale = upwardMovementMultiplier;
        }
        // APLICAR GRAVEDAD EN LA BAJADA
        else if (body.velocity.y < 0)
        {
            body.gravityScale = downwardMovementMultiplier;
        }
        // GRAVEDAD POR DEFECTO SI NO SE EST� SUBIENDO O BAJANDO
        else if (body.velocity.y == 0)
        {
            body.gravityScale = defaultGravityScale;
        }

        body.velocity = velocity;
    }
    /**
     * Funci�n de salto
     */
    private void JumpAction()
    {
        // SI SE EST� EN EL SUELO O SE TIENEN SALTOS A�REOS RESTANTES, SE PUEDE SALTAR
        if (onGround || jumpPhase < maxAirJumps)
        {
            holdingJumpStart = 0;
            jumpPhase += 1;
            float jumpSpeed = Mathf.Sqrt(-2f * Physics2D.gravity.y * jumpHeight);

            if (velocity.y > 0f)
            {
                jumpSpeed = Mathf.Max(jumpSpeed - velocity.y, 0f);
            }
            else if (velocity.y < 0f)
            {
                jumpSpeed += Mathf.Abs(body.velocity.y);
            }

            velocity.y += jumpSpeed;
        }
    }
}