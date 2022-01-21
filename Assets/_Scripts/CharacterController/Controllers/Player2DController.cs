using UnityEngine;

[CreateAssetMenu(fileName = "Player2DController", menuName = "InputController/Player2DController")]
public class Player2DController : InputController
{
    // SCRIPT CONTROLES
    public override bool RetrieveJumpInput()
    {
        return Input.GetButtonDown("Jump");
    }

    public override bool RetrieveJumpInputHold()
    {
        return Input.GetButton("Jump");
    }

    public override bool RetrieveJumpInputRelease()
    {
        return Input.GetButtonUp("Jump");
    }
    public override float RetrieveMoveInput()
    {
        return Input.GetAxisRaw("Horizontal");
    }
}
