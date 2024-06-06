using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    [Header("Players Stats Variables")]
    public float hpValue;

    public float bloodValue;

    public float compositionValue;

    public float reflexValue;

    public float damageValue;

    public int fulguriteValue;

    int level;

    [HideInInspector] public float currentHP;

    //for each 100 blood there's 1 bar
    float currentBlood;

    [HideInInspector] public int fulguriteToLevel;

    [Header("References")]
    Animator playerAnimator;

    PlayerController playerController;

    CombatController playerCombat;

    TextMeshProUGUI fulguriteSlot;

    Image healthBarSprite;
    
    // Start is called before the first frame update
    void Start()
    {
        playerAnimator = GetComponentInChildren<Animator>();

        playerController = GetComponent<PlayerController>();

        playerCombat = GetComponent<CombatController>();

        fulguriteSlot = GameObject.Find("FulguriteValue").GetComponent<TextMeshProUGUI>();

        healthBarSprite = GameObject.Find("HealthBar").GetComponent<Image>();

        //setting the stats to these values for protyping purposes
        level = 1;

        hpValue = 100;

        bloodValue = 100;

        compositionValue = 100;

        FulguriteNeeded();

        currentHP = hpValue;

        ChangingHPUI();

        currentBlood = bloodValue;
    }

    public void ChangingHPUI() 
    {
        healthBarSprite.fillAmount = currentHP / hpValue;
    }

    //taking damage function
    public void TakeDamage(float damage)
    {
        //we play the punch sfx
        SFXManager.Instance.PlayPunched();

        if (currentHP - damage > 0)
        {
            playerAnimator.Play("Punched");

            StartCoroutine(GotHit());

            currentHP -= damage;

            ChangingHPUI();

        }
        else
        {
            currentHP = 0;

            ChangingHPUI();

            playerAnimator.Play("Death");

            playerController.enabled = false;

            StartCoroutine(Death());
        }
    }

    IEnumerator Death()
    {
        yield return new WaitForSeconds(2);

        GameManager.Instance.MainMenuSequenceFunction();

        yield break;
    }

    //the GotHit sequence
    IEnumerator GotHit()
    {
        playerController.enabled = false;

        yield return new WaitForSeconds(1);

        playerCombat.canAttack = true;

        playerController.enabled = true;
    }

    //Adding Health Function
    public void AddHealth(int healthToAdd)
    {
        hpValue += healthToAdd;

        level++;

        FulguriteNeeded();
    }

    //adding Blood Function
    public void AddBlood(int bloodToAdd)
    {
        bloodValue += bloodToAdd;

        level++;

        FulguriteNeeded();
    }

    //Adding Fulgurite Function
    public void AddFulgurite(int fulguriteToAdd)
    {
        fulguriteValue += fulguriteToAdd;

        fulguriteSlot.text = fulguriteValue.ToString();
    }

    //Adding Compostion Function
    public void AddComposition(int compositionToAdd)
    {
        compositionValue += compositionToAdd;

        level++;

        FulguriteNeeded();
    }

    //Code that checks how much fulgurite is needed to level and also updates the value on the text
    public void FulguriteNeeded() 
    {
        fulguriteToLevel = (level * 60 + level + 356);

        fulguriteSlot.text = fulguriteValue.ToString();
    }
}
