using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackType {
    public char keybinding;
    public int damage;
    public float cooldown;
    public AudioSource soundFx;
    public float pitchMultiplier;

    public AttackType(char key, int dmg, string soundClipPath, float pitchMultiplier = 1, float cool = 0) {
        keybinding = key;
        damage = dmg;

        if (cool > 0) {
            cooldown = cool;
        } else {
            cooldown = (float)dmg/10;
        }

        GameObject gameObject = GameObject.Find("AttackSounds");
        soundFx = gameObject.AddComponent<AudioSource>();
        soundFx.clip = Resources.Load<AudioClip>(soundClipPath);
        soundFx.pitch = pitchMultiplier;
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
        lightPunch = new AttackType('l', 6, "SoundEffects/Punch");
        lightKick = new AttackType('k', 8, "SoundEffects/Kick");
        lightSpecial = new AttackType('j', 12, "SoundEffects/Special");
        heavyPunch = new AttackType('o', 10, "SoundEffects/Punch", 0.8f);
        heavyKick = new AttackType('i', 14, "SoundEffects/Kick", 0.8f);
        heavySpecial = new AttackType('u', 20, "SoundEffects/Special", 1.2f);
    }
}

public class AttackController : MonoBehaviour
{
    public Animator animator;
    [SerializeField]
    private AttackProfile attackProfile;

    private float nextFire = 0.5F;
    private float myTime = 0.0F;

    private GameObject enemy;

    public delegate void AtkEvt(AudioSource audioSource);
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
            return attackProfile.lightPunch;
        }

        if (Input.GetButtonDown("LightKick")) {
            return attackProfile.lightKick;
        }

        if (Input.GetButtonDown("LightSpecial")) {
            return attackProfile.lightSpecial;
        }

        if (Input.GetButtonDown("HeavyPunch")) {
            return attackProfile.heavyPunch;
        }

        if (Input.GetButtonDown("HeavyKick")) {
            return attackProfile.heavyKick;
        }

        if (Input.GetButtonDown("HeavySpecial")) {
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
        
        AttackEvent(attack.soundFx);
    }
}
