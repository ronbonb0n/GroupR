using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Animation : MonoBehaviour
{
    public Animator animator;
    
    public void Running(bool isRunning)
    {
        animator.SetBool("IsRunning", isRunning);
    }
    public void Crouching(bool isCrouching)
    {
        animator.SetBool("IsCrouching", isCrouching);
    }
    public void Walking(bool isWalking)
    {
        animator.SetBool("IsWalking", isWalking);
    }


}
