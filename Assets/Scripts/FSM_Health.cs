using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FSM_Health : MonoBehaviour
{
    public int maxHealth = 50;
    private int currentHealth = 0;

    public UnityEvent tookDamage;
    public UnityEvent death;

    // Start is called before the first frame update
    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        UpdateHealth(-damage);
    }

    void UpdateHealth(int value)
    {
        currentHealth += value;
        if (currentHealth <= 0)
        {
            death.Invoke();
        }
        else if (value < 0)
        {
            tookDamage.Invoke();
        }
        else
        {
            print(gameObject.name + " was healed");
        }
    }

}
