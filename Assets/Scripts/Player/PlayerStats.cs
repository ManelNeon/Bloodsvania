using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    int level;

    public int hpValue;

    public int bloodValue;

    public int fulguriteValue;

    public int compositionValue;

    [HideInInspector] public int fulguriteToLevel;

    int currentHP;

    //for each 100 blood there's 1 bar
    int currentBlood;
    
    // Start is called before the first frame update
    void Start()
    {
        level = 1;

        hpValue = 100;

        bloodValue = 100;

        compositionValue = 100;

        fulguriteValue = 0;

        FulguriteNeeded();

        currentHP = hpValue;

        currentBlood = bloodValue;
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

        level++;

        FulguriteNeeded();
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
