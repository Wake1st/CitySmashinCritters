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

  private void Awake()
  {
    upright = this.transform.rotation;
    parent = this.transform.parent;

    attackProfile = new AttackProfile();

    shakeCountdown = shakeTime;
  }

  private void Update()
  {
    if (collapsing)
    {
      Sink();
      Shake();
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
    float topOfParent = topOfParent = parent.localPosition.y
            + (parent.transform.localScale.y);

    parent.Translate(
        new Vector3(0, -1, 0) * collapseSpeed * Time.deltaTime
    );

    collapsing = topOfParent > 0;
  }

  private void Shake(float shake = 6)
  {
    //  set the building to rotate
    Quaternion localRotate = Quaternion.Euler(
        Random.Range(-shake, shake),
        0,
        Random.Range(-shake, shake)
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
    parent.rotation = upright;
  }
}
