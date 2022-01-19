using UnityEngine;

    public class Move : MonoBehaviour
    {
        [SerializeField] private InputController input = null;
        [SerializeField] private GameObject characterObject = null;
        [SerializeField, Range(0f, 100f)] private float maxSpeed = 4f;
        [SerializeField, Range(0f, 100f)] private float maxAcceleration = 35f;
        [SerializeField, Range(0f, 100f)] private float maxAirAcceleration = 20f;

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
            desiredVelocity = new Vector2(direction.x, 0f) * Mathf.Max(maxSpeed - ground.GetFriction(),0f);
            
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

        private void FaceMoveDirection()
        {
                if (input.RetrieveMoveInput() > 0)
                {
                    characterScale.x = characterScaleX;
                }
                else if (input.RetrieveMoveInput() < 0)
                {
                    characterScale.x = -characterScaleX;
                }
                transform.localScale = characterScale;
            }
        }