using UnityEngine;

[CreateAssetMenu(fileName = "Player2DController", menuName = "InputController/Player2DController")]
public class Player2DController : InputController
{
    public override bool RetrieveJumpInput()
    {
        return Input.GetButtonDown("Jump");
    }

    public override float RetrieveMoveInput()
    {
        return Input.GetAxisRaw("Horizontal");
    }
}
