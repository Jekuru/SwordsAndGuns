using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    [SerializeField] private Animator animatorController;
    [SerializeField] private Weapon weaponController;
    [SerializeField] private InputController input = null;

    private void Awake()
    {
        animatorController = GetComponent<Animator>();
        weaponController = GetComponent<Weapon>();
    }

    // Update is called once per frame
    void Update()
    {
        walkAnimation();
        meleeAnimation();
    }

    private void walkAnimation()
    {
        if(input.RetrieveMoveInput() != 0)
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
