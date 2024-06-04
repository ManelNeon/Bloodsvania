using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationsEvents : MonoBehaviour //this script exists beacuse the animator and the other scripts are on different objects
{
    //attack particles
    [SerializeField] ParticleSystem attackParticles;

    //getting the player's CombatController
    [SerializeField] CombatController playerCombatController;

    //the event for when the player attacks the enemy, the moment where contact is made
    public void AttackEvent()
    {
        attackParticles.Play();
        playerCombatController.Attack();
    }

    //the event when the animation ends
    public void AttackEnded()
    {
        playerCombatController.AttackEnd();
    }
}
