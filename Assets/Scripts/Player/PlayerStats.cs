using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [SerializeField] int hpValue;

    [SerializeField] int bloodValue;

    [SerializeField] int fulguriteValue;

    [SerializeField] int compositionValue;

    int currentHP;

    //for each 100 blood there's 1 bar
    int currentBlood;
    
    // Start is called before the first frame update
    void Start()
    {
        currentHP = hpValue;

        currentBlood = bloodValue;
    }

    public void AddHealth(int healthToAdd)
    {
        hpValue += healthToAdd;

        Debug.Log(hpValue);
    }

    public void AddBlood(int bloodToAdd)
    {
        bloodValue += bloodToAdd;

        Debug.Log(bloodValue);
    }

    public void AddFulgurite(int fulguriteToAdd)
    {
        fulguriteValue += fulguriteToAdd;

        Debug.Log(fulguriteValue);
    }

    public void AddComposition(int compositionToAdd)
    {
        compositionValue += compositionToAdd;

        Debug.Log(compositionValue);
    }

    // Update is called once per frame
    void Update()
    {
        //debug Only
        if (Input.GetKeyDown(KeyCode.L))
        {
            AddHealth(25);
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            AddBlood(25);
        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            AddFulgurite(225);
        }

        if (Input.GetKeyDown(KeyCode.J))
        {
            AddComposition(25);
        }
    }
}
