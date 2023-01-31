using UnityEngine;

public class AlarmLightController : MonoBehaviour
{
  public float rotationSpeed;

  void Update()
  {
    RotateLight();
  }

  private void RotateLight()
  {
    transform.Rotate(
        new Vector3(
            0,
            rotationSpeed * Time.deltaTime,
            0
        ),
        Space.Self
    );
  }
}
