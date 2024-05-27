using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationsEvents : MonoBehaviour
{
    [SerializeField] ParticleSystem attackParticles;

    [SerializeField] PlayerController playerController;

    public void AttackEvent()
    {
        attackParticles.Play();
    }

    public void AttackEnded()
    {
        playerController.canAttack = true;
    }
}
