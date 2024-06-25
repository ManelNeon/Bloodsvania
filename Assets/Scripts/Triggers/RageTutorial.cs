using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RageTutorial : MonoBehaviour
{
    public void ExitMenu()
    {
        Time.timeScale = 1;

        Cursor.lockState = CursorLockMode.Locked;

        Cursor.visible = false;

        GameManager.Instance.canUseAbilities = true;

        gameObject.SetActive(false);
    }
}
