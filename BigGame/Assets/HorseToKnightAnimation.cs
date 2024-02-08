using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorseToKnightAnimation : MonoBehaviour
{
    [SerializeField] private Animator knightAnimator;
    private int _test = 0;
    public void PlayKnightWalk()
    {
        if (_test != 1)
        {
            Debug.Log("walk");
            _test = 1;
            knightAnimator.SetTrigger("walk");
        }
       
    }
    public void PlayKnightAttack()
    {
        if (_test != 2)
        {
            Debug.Log("Attack");
            _test = 2;
            knightAnimator.SetTrigger("attack");

        }


    }
    public void PlayKnightHurt(bool death)
    {
        if (_test != 3)
        {
            Debug.Log("Boli");
            _test = 3;
            knightAnimator.SetTrigger("hurt");

        }



    }

    public void PlayKnightIdle()
    {
        if (_test != 4)
        {
            Debug.Log("iddle");
            _test = 4;
            knightAnimator.SetTrigger("idle");

        }



    }


}
