using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    int level;

    public float hpValue;

    public float bloodValue;

    public float compositionValue;

    public float reflexValue;

    public float damageValue;

    public int fulguriteValue;

    [HideInInspector] public int fulguriteToLevel;

    Animator playerAnimator;

    PlayerController playerController;

    float currentHP;

    TextMeshProUGUI fulguriteSlot;

    Image healthBarSprite;

    //for each 100 blood there's 1 bar
    float currentBlood;
    
    // Start is called before the first frame update
    void Start()
    {
        playerAnimator = GetComponentInChildren<Animator>();

        playerController = GetComponent<PlayerController>();

        fulguriteSlot = GameObject.Find("FulguriteValue").GetComponent<TextMeshProUGUI>();

        healthBarSprite = GameObject.Find("HealthBar").GetComponent<Image>();

        level = 1;

        hpValue = 100;

        bloodValue = 100;

        compositionValue = 100;

        FulguriteNeeded();

        currentHP = hpValue;

        healthBarSprite.fillAmount = currentHP / hpValue;

        currentBlood = bloodValue;
    }

    public void TakeDamage(float damage)
    {
        if (currentHP - damage > 0)
        {
            playerAnimator.Play("Punched");

            StartCoroutine(GotHit());

            currentHP -= damage;

            healthBarSprite.fillAmount = currentHP / hpValue;
        }
        else
        {
            currentHP = 0;

            healthBarSprite.fillAmount = currentHP / hpValue;

            playerAnimator.Play("Death");

            Debug.Log("Dead");
        }
    }

    IEnumerator GotHit()
    {
        playerController.enabled = false;

        yield return new WaitForSeconds(1);

        playerController.enabled = true;
    }

    public void AddHealth(int healthToAdd)
    {
        hpValue += healthToAdd;

        level++;

        FulguriteNeeded();
    }

    public void AddBlood(int bloodToAdd)
    {
        bloodValue += bloodToAdd;

        level++;

        FulguriteNeeded();
    }

    public void AddFulgurite(int fulguriteToAdd)
    {
        fulguriteValue += fulguriteToAdd;

        fulguriteSlot.text = fulguriteValue.ToString();
    }

    public void AddComposition(int compositionToAdd)
    {
        compositionValue += compositionToAdd;

        level++;

        FulguriteNeeded();
    }

    public void FulguriteNeeded() 
    {
        fulguriteToLevel = (level * 60 + level + 356);

        fulguriteSlot.text = fulguriteValue.ToString();
    }
}
