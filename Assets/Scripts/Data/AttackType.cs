using UnityEngine;

public class AttackType
{
  public int id;
  public char keybinding;

  public int damage;
  public float cooldown;
  public float shake;

  public AudioSource soundFx;
  public float pitchMultiplier;

  public AttackType(
    int ID,
    char key,
    int dmg,
    string soundClipPath,
    float pitchMultiplier = 1,
    float cool = 0
)
  {
    id = ID;
    keybinding = key;
    damage = dmg;
    shake = dmg * 2.4f;

    if (cool > 0)
    {
      cooldown = cool;
    }
    else
    {
      cooldown = (float)dmg / 10;
    }

    GameObject gameObject = GameObject.Find("AttackSounds");
    soundFx = gameObject.AddComponent<AudioSource>();
    soundFx.clip = Resources.Load<AudioClip>(soundClipPath);
    soundFx.pitch = pitchMultiplier;
  }
}