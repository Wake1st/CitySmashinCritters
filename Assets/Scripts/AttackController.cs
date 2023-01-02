using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackController : MonoBehaviour
{
    public int damage = 10;
    public Animator animator;

    private float nextFire = 0.5F;
    private float myTime = 0.0F;

    private GameObject enemy;

    public delegate void AtkEvt();
    public static event AtkEvt AttackEvent;

    private bool isPlaying;

    private void Start() {
        LevelController.UpdateIsPlaying += SetIsPlaying;
    }

    void Update()
    {
        if (isPlaying) {
            myTime = myTime + Time.deltaTime;

            if (myTime > nextFire) {
                if (Input.GetButton("Fire1")) {
                    Attack();
                    myTime = 0.0F;
                } else {
                    animator.SetBool("Attacking", false);
                }
            }
        }
    }

    void SetIsPlaying(bool playing) {
        isPlaying = playing;
    }

    void OnTriggerEnter(Collider collider) {
        if (collider.gameObject.layer == LayerMask.NameToLayer("destructable")) {
            enemy = collider.gameObject;
        }
    }

    void OnTriggerExit(Collider collider) {
        enemy = null;
    }

    private void Attack() {
        if (enemy != null) {
            GameObject
                .Find(enemy.name + "/Health")
                .GetComponent<DestructableHealthController>()
                .TakeDamage(damage);
        }

        animator.SetBool("Attacking", true);

        AttackEvent();
    }
}
