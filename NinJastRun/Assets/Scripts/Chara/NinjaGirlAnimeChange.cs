using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class NinjaGirlAnimeChange : MonoBehaviour
{
    const string NINJAGIRL_IDLE = "NinjaGirl_Idle";
    const string NINJAGIRL_RUN = "NinjaGirl_Run";
    const string NINJAGIRL_SHOOT = "NinjaGirl_Shoot";
    const string NINJAGIRL_JUMP = "NinjaGirl_Jump";
    const string NINJAGIRL_DEAD = "NinjaGirl_Dead";

    Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();

        animator.Play(NINJAGIRL_IDLE);
    }

    void Update()
    {
        
    }

    public void AnimateIdle()
    {
        animator.Play(NINJAGIRL_IDLE);
    }

    public void AnimateRun()
    {
        animator.Play(NINJAGIRL_RUN);
    }

    public void AnimateShoot()
    {
        animator.Play(NINJAGIRL_SHOOT);
    }

    public void AnimateJump()
    {
        animator.Play(NINJAGIRL_JUMP);
    }

    public void AnimateDead()
    {
        animator.Play(NINJAGIRL_DEAD);
    }
}
