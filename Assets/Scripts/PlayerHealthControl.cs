using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerHealthControl : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth = 0;
    public HealthBarControl healthGUI;
    public UnityEvent death;
    bool dead = false;
    public List<Component> deathComponentClearList = new List<Component>();
    public List<GameObject> deathObjDestroyList = new List<GameObject>();

    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int value)
    {
        UpdateHealth(-value); // pass in negative value to take damage, not healing
    }

    void UpdateHealth(int value)
    {
        if (dead)
        {
            return;
        }

        currentHealth += value;

        currentHealth = Mathf.Clamp(currentHealth += value, 0, maxHealth);

        healthGUI.UpdateHealthBar((float)currentHealth / (float)maxHealth);
        if(currentHealth <= 0)
        {
            dead = true;
            PauseControl.instance.GameOver();
            //healthGUI.GameOver(); // put this in the event
            Cursor.lockState = CursorLockMode.Confined;
            
            death.Invoke();
            
            for(int i = 0; i < deathComponentClearList.Count; i++)
            {
                Destroy(deathComponentClearList[i]);
            }
            for (int i = 0; i < deathObjDestroyList.Count; i++)
            {
                Destroy(deathObjDestroyList[i]);
            }
        }
    }
}
