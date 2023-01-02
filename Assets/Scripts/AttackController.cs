using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackType {
    public char keybinding;
    public int damage;
    public float cooldown;

    public AttackType(char key, int dmg, float cool = 0) {
        keybinding = key;
        damage = dmg;

        if (cool > 0) {
            cooldown = cool;
        } else {
            cooldown = (float)dmg/10;
        }
    }
}

public class AttackProfile {
    public AttackType lightPunch;
    public AttackType lightKick;
    public AttackType lightSpecial;
    public AttackType heavyPunch;
    public AttackType heavyKick;
    public AttackType heavySpecial;

    public AttackProfile() {
        lightPunch = new AttackType('l', 6);
        lightKick = new AttackType('k', 8);
        lightSpecial = new AttackType('j', 12);
        heavyPunch = new AttackType('o', 10);
        heavyKick = new AttackType('i', 14);
        heavySpecial = new AttackType('u', 20);
    }
}

public class AttackController : MonoBehaviour
{
    private AttackProfile attackProfile;
    public Animator animator;

    private float nextFire = 0.5F;
    private float myTime = 0.0F;

    private GameObject enemy;

    public delegate void AtkEvt();
    public static event AtkEvt AttackEvent;

    private bool isPlaying;

    private void Awake() {
        attackProfile = new AttackProfile();
    }

    private void Start() {
        LevelController.UpdateIsPlaying += SetIsPlaying;
    }

    void Update()
    {
        if (isPlaying) {
            //  Check for blocking
            if (Input.GetButton("Block")) {
                print("blocking");
                return;
            }

            //  Check for attack
            myTime = myTime + Time.deltaTime;
            if (myTime > nextFire) {
                AttackType attack = CheckAttack();

                if (attack != null) {
                    Attack(attack);
                    myTime = 0.0F;
                    nextFire = attack.cooldown;
                    print(nextFire);
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

    private AttackType CheckAttack() {
        if (Input.GetButtonDown("LightPunch")) {
            print("lightPunch");
            return attackProfile.lightPunch;
        }

        if (Input.GetButtonDown("LightKick")) {
            print("lightKick");
            return attackProfile.lightKick;
        }

        if (Input.GetButtonDown("LightSpecial")) {
            print("lightSpecial");
            return attackProfile.lightSpecial;
        }

        if (Input.GetButtonDown("HeavyPunch")) {
            print("heavyPunch");
            return attackProfile.heavyPunch;
        }

        if (Input.GetButtonDown("HeavyKick")) {
            print("heavyKick");
            return attackProfile.heavyKick;
        }

        if (Input.GetButtonDown("HeavySpecial")) {
            print("heavySpecial");
            return attackProfile.heavySpecial;
        }

        return null;
    }

    private void Attack(AttackType attack) {
        if (enemy != null) {
            GameObject
                .Find(enemy.name + "/Health")
                .GetComponent<DestructableHealthController>()
                .TakeDamage(attack.damage);
        }

        animator.SetBool("Attacking", true);

        AttackEvent();
    }
}
