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
    public float hpValue;

    public float bloodValue;

    public float dmgValue;

    public float compositionValue;

    public float reflexValue;

    public int fulguriteValue;

    public int level;

    [HideInInspector]public float currentHP;

    [HideInInspector] public float currentRage;

    [HideInInspector] public int fulguriteToLevel;

    [Header("ReferencesHealthBar")]
    public Image healthBarSprite;

    public RectTransform healthBarMaskTransform;

    public RectTransform healthBarDividerTransform;

    public RectTransform damageBarMaskTransform;

    public RectTransform damageBarTransform;

    [SerializeField] float lerpSpeed = 0.05f;

    bool isHealing;

    [Header("ReferencesRageBar")]
    public Image rageBarSprite;

    public RectTransform rageBarMaskTransform;

    public RectTransform rageBarDividerTransform;

    [Header("ReferencesFulgurite")]
    public TextMeshProUGUI fulguriteSlot;

    float healthBarMaskWidth;

    float rageBarMaskWidth;

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

        //for debugging test
        currentHP = hpValue;

        currentRage = bloodValue;

        healthBarMaskWidth = healthBarMaskTransform.sizeDelta.x;

        rageBarMaskWidth = rageBarMaskTransform.sizeDelta.x;

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
            FindAnyObjectByType<CombatController>().TakeDamage(25);

            AddFulgurite(100000);
        }

        if (damageBarMaskTransform != null && damageBarMaskTransform.sizeDelta.x != healthBarMaskTransform.sizeDelta.x)
        {
            damageBarMaskTransform.sizeDelta = new Vector2(Mathf.Lerp(damageBarMaskTransform.sizeDelta.x, (currentHP / hpValue) * healthBarMaskWidth, lerpSpeed), damageBarMaskTransform.sizeDelta.y );
        }

        if (damageBarMaskTransform != null && healthBarMaskTransform.sizeDelta.x != damageBarMaskTransform.sizeDelta.x & isHealing)
        {
            healthBarMaskTransform.sizeDelta = new Vector2(Mathf.Lerp(healthBarMaskTransform.sizeDelta.x, (currentHP / hpValue) * healthBarMaskWidth, lerpSpeed), healthBarMaskTransform.sizeDelta.y);
        }
        else if (damageBarMaskTransform != null && healthBarMaskTransform.sizeDelta.x == damageBarMaskTransform.sizeDelta.x & isHealing)
        {
            isHealing = false;
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

        AudioManager.Instance.PlayMusic(AudioManager.Instance.mainMenuMusic);

        yield return new WaitForSeconds(1.5f);

        isControlable = true;

        SceneManager.LoadScene(0);

        yield break;
    }


    public void ResetStats()
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

        //Find and Establishing Hud
        FindHUD();

        healthBarMaskWidth = healthBarMaskTransform.sizeDelta.x;

        rageBarMaskWidth = rageBarMaskTransform.sizeDelta.x;

        FulguriteNeeded();
    }

    void FindHUD()
    {
        healthBarSprite = GameObject.Find("HealthBar").GetComponent<Image>();

        healthBarMaskTransform = GameObject.Find("HealthBarMask").GetComponent<RectTransform>();

        healthBarDividerTransform = GameObject.Find("DividerHealth").GetComponent<RectTransform>();

        damageBarMaskTransform = GameObject.Find("DamageBarMask").GetComponent<RectTransform>();

        damageBarTransform = GameObject.Find("DamageBar").GetComponent<RectTransform>();

        rageBarSprite = GameObject.Find("RageBar").GetComponent<Image>();

        rageBarMaskTransform = GameObject.Find("RageBarMask").GetComponent<RectTransform>();

        rageBarDividerTransform = GameObject.Find("DividerRage").GetComponent<RectTransform>();

        fulguriteSlot = GameObject.Find("FulguriteValue").GetComponent<TextMeshProUGUI>();
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
            currentRage -= Mathf.Round(hpValue - currentHP);

            currentHP = hpValue;
        }

        isHealing = true;

        Vector2 barMaskSizeDelta = damageBarMaskTransform.sizeDelta;

        barMaskSizeDelta.x = (currentHP / hpValue) * healthBarMaskWidth;

        damageBarMaskTransform.sizeDelta = barMaskSizeDelta;
    }

    public void ChangingHPUI()
    {
        Vector2 barMaskSizeDelta = healthBarMaskTransform.sizeDelta;

        barMaskSizeDelta.x = (currentHP / hpValue) * healthBarMaskWidth;

        healthBarMaskTransform.sizeDelta = barMaskSizeDelta;
    }

    public void ChangingRageUI()
    {
        Vector2 barMaskSizeDelta = rageBarMaskTransform.sizeDelta;

        barMaskSizeDelta.x = (currentRage / bloodValue) * rageBarMaskWidth;

        rageBarMaskTransform.sizeDelta = barMaskSizeDelta;
    }

    //Adding Health Function
    public void AddHealth(int healthToAdd)
    {
        hpValue += healthToAdd;

        currentHP = hpValue;

        healthBarMaskTransform.sizeDelta = new Vector2(healthBarMaskWidth + 150f, healthBarMaskTransform.sizeDelta.y);

        healthBarSprite.rectTransform.sizeDelta = new Vector2(healthBarMaskWidth + 150f, healthBarMaskTransform.sizeDelta.y);

        healthBarDividerTransform.sizeDelta = new Vector2(healthBarDividerTransform.sizeDelta.x + 140f, healthBarDividerTransform.sizeDelta.y);

        damageBarMaskTransform.sizeDelta = new Vector2(healthBarMaskWidth + 150f, damageBarMaskTransform.sizeDelta.y);

        damageBarTransform.sizeDelta = new Vector2(healthBarMaskWidth + 150f, damageBarTransform.sizeDelta.y);

        healthBarMaskWidth = healthBarMaskTransform.sizeDelta.x;

        level++;

        FulguriteNeeded();
    }

    //adding Blood Function
    public void AddBlood(int bloodToAdd)
    {
        bloodValue += bloodToAdd;

        currentRage = bloodValue;

        rageBarMaskTransform.sizeDelta = new Vector2(rageBarMaskWidth + 60f, rageBarMaskTransform.sizeDelta.y);

        rageBarSprite.rectTransform.sizeDelta = new Vector2(rageBarMaskWidth + 60f, rageBarMaskTransform.sizeDelta.y);

        rageBarDividerTransform.sizeDelta = new Vector2(rageBarDividerTransform.sizeDelta.x + 56f, rageBarDividerTransform.sizeDelta.y);

        rageBarMaskWidth = rageBarMaskTransform.sizeDelta.x;

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

    public void GainRage(float rageToAdd)
    {
        if (currentRage + rageToAdd > bloodValue)
        {
            currentRage = bloodValue;
        }
        else
        {
            currentRage += rageToAdd;
        }

        ChangingRageUI();
    }
}
