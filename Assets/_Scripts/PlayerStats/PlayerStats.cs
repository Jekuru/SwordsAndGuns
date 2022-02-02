using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public Controller characterController;
    public InputController characterInput;
    public InputController deadInput;

    public Move moveController;

    public int healthPoints = 1;
    public bool isDead;

    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<Controller>();
        moveController = GetComponent<Move>();
    }

    // Update is called once per frame
    void Update()
    {
        IsDead();
    }

    public void IsDead()
    {
        if(healthPoints <= 0)
        {
            isDead = true;
            characterController.input = deadInput;
            moveController.enabled = false;
        } else
        {
            isDead = false;
            characterController.input = characterInput;
            moveController.enabled = true;
        }
    }
}
