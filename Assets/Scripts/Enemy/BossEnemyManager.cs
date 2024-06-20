using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class BossEnemyManager : MonoBehaviour
{
    [Header("Boss Variables")]
    [SerializeField] float maxHP;

    [HideInInspector] public float currentHP;

    [HideInInspector] public bool canDamage;

    [HideInInspector] public int hitsOnArm;

    [Header("Boss Bar")]
    [SerializeField] RectTransform healthBarMaskTransform;

    [SerializeField] RectTransform damageBarMaskTransform;

    float healthBarMaskWidth;

    [Header("Boss Bar Fade")]
    [SerializeField] CanvasGroup bossHealthbar;

    [SerializeField] float fadeDuraction;

    bool isFading;

    float m_Timer;

    void Awake()
    {
        hitsOnArm = 3;

        healthBarMaskWidth = healthBarMaskTransform.sizeDelta.x;

        currentHP = maxHP;

        StartCoroutine(DamageCycle());
    }

    void Update()
    {
        Fades();

        if (damageBarMaskTransform.sizeDelta.x != healthBarMaskTransform.sizeDelta.x)
        {
            damageBarMaskTransform.sizeDelta = new Vector2(Mathf.Lerp(damageBarMaskTransform.sizeDelta.x, (currentHP / maxHP) * healthBarMaskWidth, .05f), damageBarMaskTransform.sizeDelta.y);
        }

        if (Input.GetKeyDown(KeyCode.V))
        {
            canDamage = true;

            FindObjectOfType<CombatController>().isFighting = true;
        }
    }

    void Fades()
    {
        if (isFading && bossHealthbar.alpha == 0)
        {
            m_Timer += Time.deltaTime;

            bossHealthbar.alpha = m_Timer / fadeDuraction;

            if (bossHealthbar.alpha == 1)
            {
                isFading = false;

                m_Timer = fadeDuraction;
            }
        }

        if (isFading && bossHealthbar.alpha == 1)
        {
            m_Timer -= Time.deltaTime;

            bossHealthbar.alpha = m_Timer / fadeDuraction;

            if (bossHealthbar.alpha == 0)
            {
                isFading = false;
            }
        }
    }

    IEnumerator DamageCycle()
    {
        while (true)
        {
            if (canDamage)
            {
                yield return new WaitForSeconds(5);

                Debug.Log("Cant Attack");

                canDamage = false;

                hitsOnArm = 3;
            }

            yield return null;
        }
    }
}
