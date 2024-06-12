using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    //this bool controls either if the player can or cannot control
    public bool isControlable;

    [Header("Player Stats")]
    [HideInInspector] public float hpValue;

    [HideInInspector] public float bloodValue;

    [HideInInspector] public float dmgValue;

    [HideInInspector] public float compositionValue;

    [HideInInspector] public float reflexValue;

    [HideInInspector] public int fulguriteValue;

    [HideInInspector] public int level;

    //Somehow increase the scale of the HP in the UI???
    [HideInInspector]public float currentHP;

    //Somehow increase the scale of the rageBar in the UI??
    [HideInInspector] public float currentRage;

    [HideInInspector] public int fulguriteToLevel;

    [Header("ReferencesHealthBar")]
    public Image healthBarSprite;

    public RectTransform healthBarMaskTransform;

    public RectTransform healthBarDividerTransform;

    [Header("ReferencesRageBar")]
    public Image rageBarSprite;

    public TextMeshProUGUI fulguriteSlot;

    float barMaskWidth;

    // Start is called before the first frame update
    void Start()
    {
        //setting the Instnace
        if (Instance != null)
        {
            Destroy(this.gameObject);

            return;
        }

        Instance = this;
        
        DontDestroyOnLoad(this.gameObject);

        ResetStats();

        isControlable = true;
    }

    // Update is called once per frame
    void Update()
    {
        //Debugging only
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            MainMenuSequenceFunction();
        }
        
        if (Input.GetKeyDown(KeyCode.X))
        {
            FindAnyObjectByType<CombatController>().TakeDamage(1);

            AddFulgurite(100000);
        }
    }

    //function to fade back to the Main Menu
    public void MainMenuSequenceFunction()
    {
        StartCoroutine(MainMenuSequence());
    }

    //Coroutine to fade back to the Main Menu
    IEnumerator MainMenuSequence()
    {
        FadeManager.Instance.StartFadeOut();

        MusicManager.Instance.PlayMenuMusic();

        yield return new WaitForSeconds(1.5f);

        isControlable = true;

        ResetStats();

        SceneManager.LoadScene(0);

        yield break;
    }


    void ResetStats()
    {
        //Initial Values
        hpValue = 100;

        bloodValue = 50;

        dmgValue = 50;

        compositionValue = 35;

        reflexValue = 50;

        fulguriteValue = 0;

        level = 1;

        currentHP = hpValue;

        currentRage = bloodValue;

        //fulguriteSlot = null;

        barMaskWidth = healthBarMaskTransform.sizeDelta.x;

        FulguriteNeeded();
    }

    public void HealFunction()
    {
        if (hpValue - currentHP > currentRage)
        {
            currentHP += currentRage;

            currentRage = 0;
        }
        else
        {
            currentRage -= (hpValue - currentHP);

            currentHP = hpValue;
        }

        ChangingHPUI();

        ChangingRageUI();
    }

    public void ChangingHPUI()
    {
        //healthBarSprite.fillAmount = (currentHP / hpValue);

        Vector2 barMaskSizeDelta = healthBarMaskTransform.sizeDelta;

        barMaskSizeDelta.x = (currentHP / hpValue) * barMaskWidth;

        healthBarMaskTransform.sizeDelta = barMaskSizeDelta;
    }

    public void ChangingRageUI()
    {
        rageBarSprite.fillAmount = currentRage / bloodValue;
    }

    //Adding Health Function
    public void AddHealth(int healthToAdd)
    {
        hpValue += healthToAdd;

        currentHP = hpValue;

        //healthBarSprite.transform.localScale = new Vector3(healthBarSprite.transform.localScale.x + .03f, healthBarSprite.transform.localScale.y, healthBarSprite.transform.localScale.z);

        healthBarMaskTransform.sizeDelta = new Vector2(barMaskWidth + 15f, healthBarMaskTransform.sizeDelta.y);

        healthBarSprite.rectTransform.sizeDelta = new Vector2(barMaskWidth + 15f, healthBarMaskTransform.sizeDelta.y);

        healthBarDividerTransform.sizeDelta = new Vector2(healthBarDividerTransform.sizeDelta.x + 43f, healthBarDividerTransform.sizeDelta.y);

        barMaskWidth = healthBarMaskTransform.sizeDelta.x;

        level++;

        FulguriteNeeded();
    }

    //adding Blood Function
    public void AddBlood(int bloodToAdd)
    {
        bloodValue += bloodToAdd;

        currentRage = bloodValue;

        rageBarSprite.transform.localScale = new Vector3(rageBarSprite.transform.localScale.x + .01f, rageBarSprite.transform.localScale.y, rageBarSprite.transform.localScale.z);

        level++;

        FulguriteNeeded();
    }

    public void AddDamage(int damageToAdd)
    {
        dmgValue += damageToAdd;

        level++;

        FulguriteNeeded();
    }

    //Adding Compostion Function
    public void AddComposition(int compositionToAdd)
    {
        compositionValue += compositionToAdd;

        level++;

        FulguriteNeeded();
    }

    public void AddReflex(int reflexToAdd)
    {
        reflexValue += reflexToAdd;

        level++;

        FulguriteNeeded();
    }

    //Adding Fulgurite Function
    public void AddFulgurite(int fulguriteToAdd)
    {
        fulguriteValue += fulguriteToAdd;

        fulguriteSlot.text = fulguriteValue.ToString();
    }

    

    //Code that checks how much fulgurite is needed to level and also updates the value on the text
    public void FulguriteNeeded()
    {
        fulguriteToLevel = (level * 60 + level + 356);

        if (fulguriteSlot != null)
        {
            fulguriteSlot.text = fulguriteValue.ToString();
        }
    }
}
