using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DestructableHealthController : HealthController {
    [SerializeField]
    private RectTransform panel;

    public override void TakeDamage(int damage) {
        health -= damage;
        Mathf.Clamp(health, 0, maxHealth);

        UpdateUI(health);

        this.GetComponent<BuildingSoundsController>().PlayHitSound();

        if (health <= 0) {
            Die();
        }
    }

    private void UpdateUI(int damage) {
        panel.GetComponent<OverheadController>().ChangeValue(damage);
    }

    protected override void Die() {
        this.GetComponent<AnimationController>().StartCollapse();
        this.GetComponent<BuildingSoundsController>().PlayDeathSound();
    }
}