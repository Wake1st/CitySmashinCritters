using UnityEngine;

public class AttackSoundController : MonoBehaviour
{
  private AttackProfile attackProfile;

  void Awake()
  {
    attackProfile = new AttackProfile();
  }

  void Start()
  {
    AttackController.AttackEvent += AttackSoundHandler;
  }

  void AttackSoundHandler(int soundIndex)
  {
    attackProfile.attackTypes[soundIndex].soundFx.Play();
  }
}
