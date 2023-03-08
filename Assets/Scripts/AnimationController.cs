using UnityEngine;

public class AnimationController : MonoBehaviour
{
  private Quaternion upright;

  public float collapseSpeed = 1;
  private bool collapsing = false;

  private bool isHit = false;
  private float shakeAmount;
  private float shakeTime = 0.2f;
  private float shakeCountdown;

  private AttackProfile attackProfile;
  private Transform parent;
  private Vector3 initialAttitude;

  private void Awake()
  {
    upright = this.transform.rotation;
    parent = this.transform.parent;
    initialAttitude = parent.rotation.eulerAngles;

    attackProfile = new AttackProfile();

    shakeCountdown = shakeTime;
  }

  private void Update()
  {
    if (collapsing)
    {
      Shake();
      Sink();
    }
    else if (isHit)
    {
      shakeCountdown -= Time.deltaTime;
      if (shakeCountdown > 0)
      {
        Shake(shakeAmount);
      }
      else
      {
        EndShake();
        isHit = false;
      }
    }
  }

  public void StartCollapse()
  {
    collapsing = true;
  }

  private void Sink()
  {
    float topOfParent = parent.localPosition.y
            + (parent.transform.localScale.y);

    parent.Translate(
        new Vector3(0, 0, -1) * collapseSpeed * Time.deltaTime
    );

    collapsing = topOfParent > 0;
  }

  private void Shake(float shake = 6)
  {
    //  set the building to rotate
    Quaternion localRotate = Quaternion.Euler(
        initialAttitude.x + Random.Range(-shake, shake),
        initialAttitude.y + 0,
        initialAttitude.z + Random.Range(-shake, shake)
    );
    parent.rotation = localRotate;
  }

  public void StartShake(float shake)
  {
    shakeAmount = shake;

    shakeCountdown = shakeTime;
    isHit = true;
  }

  private void EndShake()
  {
    parent.rotation = Quaternion.Euler(initialAttitude);
  }
}
