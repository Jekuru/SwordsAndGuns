using UnityEngine;

public abstract class InputController : ScriptableObject
{
    // SCRIPT PARA REALIZAR LA LLAMADA A LOS CONTROLES
    public abstract float RetrieveMoveInput();
    public abstract float RetrieveVerticalInput();
    public abstract bool RetrieveJumpInput();
    public abstract bool RetrieveJumpInputHold();
    public abstract bool RetrieveJumpInputRelease();
    public abstract bool RetrieveFireInput();
}
