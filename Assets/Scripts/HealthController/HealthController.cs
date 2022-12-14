using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthController : MonoBehaviour
{
    public int maxHealth = 100;
    protected int health;

    protected GameObject parent;

    void Awake() {
        parent = transform.parent.gameObject;

        health = maxHealth;
    }

    public void TakeDamage(int damage) {
        health -= damage;
        Mathf.Clamp(health, 0, maxHealth);

        if (health <= 0) {
            Die();
        }
    }

    public void Heal(int heal) {
        health += heal;
        Mathf.Clamp(health, 0, maxHealth);
    }

    public bool IsAlive() {
        return health > 0;
    }

    protected virtual void Die() {
        print(parent.name + " dies");
    }
}
