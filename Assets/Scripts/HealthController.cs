using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthController : MonoBehaviour
{
    private int maxHealth;
    public int health;

    private GameObject parent;

    void Awake() {
        parent = transform.parent.gameObject;
    }

    public HealthController(int value) {
        health = value;
        maxHealth = value;
    }

    public int Health { get; }

    public void TakeDamage(int damage) {
        health -= damage;
        print(parent.name + " health " + health);

        if (health <= 0) {
            Die();
        }
    }

    public void Heal(int heal) {
        health += heal;
        System.Math.Clamp(health, 0, maxHealth);
    }

    public bool IsAlive() {
        return health > 0;
    }

    private void Die() {
        Destroy(parent);
    }
}
