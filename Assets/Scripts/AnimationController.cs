using UnityEngine;

public class AnimationController : MonoBehaviour
{
  public float collapseSpeed = 1;

  private Quaternion upright;
  private bool collapsing = false;

  private AttackProfile attackProfile;
  private Transform parent;

  private void Awake()
  {
    upright = this.transform.rotation;
    parent = this.transform.parent;

    attackProfile = new AttackProfile();
  }

  private void Update()
  {
    if (collapsing)
    {
      Sink();
      Shake();
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

  public void Shake(float shake = 8)
  {
    //  set the building to rotate
    Quaternion localRotate = Quaternion.Euler(
        Random.Range(-shake, shake),
        0,
        Random.Range(-shake, shake)
    );
    parent.rotation = upright;
    parent.rotation = localRotate;
    print(parent.name + " | " + parent.rotation.eulerAngles);

    //  set the colider to the upright position
    parent.GetComponent<BoxCollider>().transform.rotation = upright;
  }
}
