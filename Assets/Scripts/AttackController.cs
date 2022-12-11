using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackController : MonoBehaviour
{
    public int damage = 10;

    private float nextFire = 0.5F;
    private float myTime = 0.0F;

    private GameObject enemy;

    void Update()
    {
        myTime = myTime + Time.deltaTime;

        if (Input.GetButton("Fire1") && myTime > nextFire)
        {
            Attack();
            myTime = 0.0F;
        }
    }

    void OnTriggerEnter(Collider collision) {
        if (collision.gameObject.layer == LayerMask.NameToLayer("destructable")) {
            enemy = collision.gameObject;
            print("found enemy: " + enemy.name);
        }
    }

    private void Attack() {
        if (enemy != null) {
            GameObject
                .Find(enemy.name + "/Health")
                .GetComponent<HealthController>()
                .TakeDamage(damage);
        }
    }
}
