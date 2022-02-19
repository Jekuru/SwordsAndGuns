using UnityEngine;

[CreateAssetMenu(fileName = "AIController", menuName = "InputController/AIController")]
public class AIController : InputController
{
    // SCRIPT PRUEBAS CON IA
    public override bool RetrieveJumpInput()
    {
        return true;
    }

    public override bool RetrieveJumpInputHold()
    {
        return true;
    }

    public override bool RetrieveJumpInputRelease()
    {
        return true;
    }

    public override bool RetrieveFireInput()
    {
        return true;
    }

    public override float RetrieveMoveInput()
    {
        return 1f;
    }

    public override float RetrieveVerticalInput()
    {
        return 1f;
    }

    public override bool RetrieveThrowInput()
    {
        return false;
    }
}
