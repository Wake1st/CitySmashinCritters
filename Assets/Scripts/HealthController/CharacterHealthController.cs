using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterHealthController : HealthController {
  
    public delegate void UpdateHealthBar(int amount);
    public static event UpdateHealthBar UpdateHealth;

    
    void Update() {
        if (Input.GetKeyDown("g") && IsAlive()) {
          TakeDamage(10);
          UpdateHealth(health);
        }
        if (Input.GetKeyDown("h") && IsAlive() && health < maxHealth) {
          Heal(10);
          UpdateHealth(health);
        }
    }
}