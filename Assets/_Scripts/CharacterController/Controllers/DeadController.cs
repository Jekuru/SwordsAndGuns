using UnityEngine;

[CreateAssetMenu(fileName = "DeadController", menuName = "InputController/DeadController")]
public class DeadController : InputController
{
    public override bool RetrieveJumpInput()
    {
        return false;
    }

    public override bool RetrieveJumpInputHold()
    {
        return false;
    }

    public override bool RetrieveJumpInputRelease()
    {
        return false;
    }

    public override bool RetrieveFireInput()
    {
        return false;
    }

    public override float RetrieveMoveInput()
    {
        return 0;
    }

    public override float RetrieveVerticalInput()
    {
        return 0;
    }

    public override bool RetrieveThrowInput()
    {
        return true;
    }
}
