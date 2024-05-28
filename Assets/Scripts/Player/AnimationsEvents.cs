using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationsEvents : MonoBehaviour
{
    [SerializeField] ParticleSystem attackParticles;

    [SerializeField] CombatController playerCombatController;

    public void AttackEvent()
    {
        attackParticles.Play();
        playerCombatController.Attack();
    }

    public void AttackEnded()
    {
        playerCombatController.AttackEnd();
    }
}
