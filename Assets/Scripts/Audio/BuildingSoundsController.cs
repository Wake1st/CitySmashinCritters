using UnityEngine;

public class BuildingSoundsController : MonoBehaviour
{
  private AudioSource deathSound;
  private AudioSource hitSound;

  void Awake()
  {
    deathSound = gameObject.AddComponent<AudioSource>();
    deathSound.clip = Resources.Load<AudioClip>("SoundEffects/BuildingDeath");

    hitSound = gameObject.AddComponent<AudioSource>();
    hitSound.clip = Resources.Load<AudioClip>("SoundEffects/BuildingHit");
  }

  public void PlayDeathSound()
  {
    deathSound.Play();
  }

  public void PlayHitSound()
  {
    hitSound.Play();
  }
}
