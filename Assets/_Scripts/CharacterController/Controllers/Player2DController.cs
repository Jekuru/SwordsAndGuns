using UnityEngine;

[CreateAssetMenu(fileName = "Player2DController", menuName = "InputController/Player2DController")]
public class Player2DController : InputController
{
    // SCRIPT CONTROLES
    public enum Player
    {
        Player01,
        Player02,
        Player03,
        Player04
    }

    public Player selectedPlayer;

    public override bool RetrieveJumpInput()
    {
        switch (selectedPlayer){
            case Player.Player01:
                return Input.GetButtonDown("P1_Jump");
            case Player.Player02:
                return Input.GetButtonDown("P2_Jump");
            case Player.Player03:
                return Input.GetButtonDown("P3_Jump");
            case Player.Player04:
                return Input.GetButtonDown("P4_Jump");
            default:
                return Input.GetButtonDown("P1_Jump");
        }
     }

    public override bool RetrieveJumpInputHold()
    {
        switch (selectedPlayer)
        {
            case Player.Player01:
                return Input.GetButton("P1_Jump");
            case Player.Player02:
                return Input.GetButton("P2_Jump");
            case Player.Player03:
                return Input.GetButton("P3_Jump");
            case Player.Player04:
                return Input.GetButton("P4_Jump");
            default:
                return Input.GetButton("P1_Jump");
        }
    }

    public override bool RetrieveJumpInputRelease()
    {
        switch (selectedPlayer)
        {
            case Player.Player01:
                return Input.GetButtonUp("P1_Jump");
            case Player.Player02:
                return Input.GetButtonUp("P2_Jump");
            case Player.Player03:
                return Input.GetButtonUp("P3_Jump");
            case Player.Player04:
                return Input.GetButtonUp("P4_Jump");
            default:
                return Input.GetButtonUp("P1_Jump");
        }
    }

    public override float RetrieveMoveInput()
    {
        switch (selectedPlayer)
        {
            case Player.Player01:
                return Input.GetAxisRaw("P1_Horizontal");
            case Player.Player02:
                return Input.GetAxisRaw("P2_Horizontal");
            case Player.Player03:
                return Input.GetAxisRaw("P3_Horizontal");
            case Player.Player04:
                return Input.GetAxisRaw("P4_Horizontal");
            default:
                return Input.GetAxisRaw("P1_Horizontal");
        }
    }

    public override float RetrieveVerticalInput()
    {
        switch (selectedPlayer)
        {
            case Player.Player01:
                return Input.GetAxisRaw("P1_Vertical");
            case Player.Player02:
                return Input.GetAxisRaw("P2_Vertical");
            case Player.Player03:
                return Input.GetAxisRaw("P3_Vertical");
            case Player.Player04:
                return Input.GetAxisRaw("P4_Vertical");
            default:
                return Input.GetAxisRaw("P1_Vertical");
        }
        
    }

    public override bool RetrieveFireInput()
    {
        switch (selectedPlayer)
        {
            case Player.Player01:
                return Input.GetButtonDown("P1_Attack");
            case Player.Player02:
                return Input.GetButtonDown("P2_Attack");
            case Player.Player03:
                return Input.GetButtonDown("P3_Attack");
            case Player.Player04:
                return Input.GetButtonDown("P4_Attack");
            default:
                return Input.GetButtonDown("P1_Attack");
        }        
    }

    public override bool RetrieveThrowInput()
    {
        switch (selectedPlayer)
        {
            case Player.Player01:
                return Input.GetButton("P1_Drop");
            case Player.Player02:
                return Input.GetButton("P2_Drop");
            case Player.Player03:
                return Input.GetButton("P3_Drop");
            case Player.Player04:
                return Input.GetButton("P4_Drop");
            default:
                return Input.GetButton("P1_Drop");
        }
    }
}
