using UnityEngine;

public abstract class InputController : ScriptableObject
{
    // SCRIPT PARA REALIZAR LA LLAMADA A LOS CONTROLES
    public abstract float RetrieveMoveInput(); // Movimiento horizontal
    public abstract float RetrieveVerticalInput(); // Movimiento vertical
    public abstract bool RetrieveJumpInput(); // Salto
    public abstract bool RetrieveJumpInputHold(); // Salto (mantener)
    public abstract bool RetrieveJumpInputRelease(); // Salto (soltar)
    public abstract bool RetrieveFireInput(); // Disparar/Ataque
    public abstract bool RetrieveThrowInput(); // Tirar arma
}
