using UnityEngine;

public class AttackController : MonoBehaviour
{
  public Animator animator;
  private AttackProfile attackProfile;

  private float nextFire = 0.5F;
  private float myTime = 0.0F;

  private GameObject enemy;

  public delegate void AtkEvt(int attackId);
  public static event AtkEvt AttackEvent;

  private bool isPlaying;

  private void Awake()
  {
    attackProfile = new AttackProfile();
  }

  private void Start()
  {
    LevelController.UpdateIsPlaying += SetIsPlaying;
  }

  void Update()
  {
    if (isPlaying)
    {
      //  Check for blocking
      if (Input.GetButton("Block"))
      {
        print("blocking");
        return;
      }

      //  Check for attack
      myTime = myTime + Time.deltaTime;
      if (myTime > nextFire)
      {
        AttackType attack = CheckAttack();

        if (attack != null)
        {
          Attack(attack);
          myTime = 0.0F;
          nextFire = attack.cooldown;
        }
        else
        {
          animator.SetBool("Attacking", false);
        }
      }
    }
  }

  void SetIsPlaying(bool playing)
  {
    isPlaying = playing;
  }

  void OnTriggerEnter(Collider collider)
  {
    if (collider.gameObject.layer == LayerMask.NameToLayer("destructable"))
    {
      enemy = collider.gameObject;
    }
  }

  void OnTriggerExit(Collider collider)
  {
    enemy = null;
  }

  private AttackType CheckAttack()
  {
    if (Input.GetButtonDown("LightPunch"))
    {
      return attackProfile.lightPunch;
    }

    if (Input.GetButtonDown("LightKick"))
    {
      return attackProfile.lightKick;
    }

    if (Input.GetButtonDown("LightSpecial"))
    {
      return attackProfile.lightSpecial;
    }

    if (Input.GetButtonDown("HeavyPunch"))
    {
      return attackProfile.heavyPunch;
    }

    if (Input.GetButtonDown("HeavyKick"))
    {
      return attackProfile.heavyKick;
    }

    if (Input.GetButtonDown("HeavySpecial"))
    {
      return attackProfile.heavySpecial;
    }

    return null;
  }

  private void Attack(AttackType attack)
  {
    if (enemy != null)
    {
      GameObject enemyHealth = GameObject.Find(enemy.name + "/Health");

      enemyHealth.GetComponent<DestructableHealthController>()
        .TakeDamage(attack.damage);

      enemyHealth.GetComponent<AnimationController>()
        .Shake(attack.shake);
    }

    animator.SetBool("Attacking", true);



    AttackEvent(attack.id);
  }
}
