using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class AnimationController : MonoBehaviour
{
    [SerializeField] private Animator animatorController;
    [SerializeField] private Weapon weaponController;
    [SerializeField] private InputController input = null;
    [SerializeField] private Joystick joystick;

    private PhotonView photonView;

    private void Awake()
    {
        photonView = GetComponent<PhotonView>();
        if (!photonView.IsMine)
            return;

        animatorController = GetComponent<Animator>();
        weaponController = GetComponent<Weapon>();
        input = GetComponent<Controller>().input;
    }

    // Update is called once per frame
    void Update()
    {
        if (!photonView.IsMine)
            return;
        walkAnimation();
        meleeAnimation();
    }

    private void walkAnimation()
    {
        if(input.RetrieveMoveInput() != 0 || joystick.Horizontal != 0)
        {
            animatorController.SetBool("isMoving", true);
        } else
        {
            animatorController.SetBool("isMoving", false);
        }
    }

    private void meleeAnimation()
    {
        if (weaponController.meleeAttack)
        {
            animatorController.SetTrigger("attack");
            weaponController.meleeAttack = false;
        }
    }
}
