using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarControl : MonoBehaviour
{
    public Image healthBarFill;
    public GameObject gameOverText;

    public void UpdateHealthBar(float percentage)
    {
        healthBarFill.fillAmount = percentage;
    }

    public void GameOver()
    {
        gameOverText.SetActive(true);
    }

}
