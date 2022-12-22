using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackBoxController : MonoBehaviour
{
    Transform parentTransform;
    private float currentAngle;

    void Start()
    {
        PlayerController.TranslateAttackDirection += TranslateAttackBox;
        PlayerController.RotateAttackDirection += RotateAttackBox;
    }

    void Update() {
        parentTransform = this.transform.parent.transform;
    }

    private void TranslateAttackBox(Vector3 direction) {
        Vector3 correctedDirection = RepositionDirection(direction);

        this.transform.position = parentTransform.position
            + correctedDirection * 0.5f 
            + new Vector3(
                0,
                0.5f,
                0
            );
    }

    private void RotateAttackBox(float angle) {
        // currentAngle = angle % 360;
        currentAngle = (int)(angle / 90) % 4;
        print(currentAngle);
    }

    private Vector3 RepositionDirection(Vector3 direction) {
        Vector3 correctedDirection;

        switch (currentAngle) {
            case 1:
            case -3:
                correctedDirection = new Vector3(
                    direction.z,
                    0,
                    -direction.x
                );
                break;
            case 2:
            case -2:
                correctedDirection = new Vector3(
                    -direction.x,
                    0,
                    -direction.z
                );
                break;
            case 3:
            case -1:
                correctedDirection = new Vector3(
                    -direction.z,
                    0,
                    direction.x
                );
                break;
            case 0:
            default:
                correctedDirection = direction;
                break;
        }
        
        return correctedDirection;
    }
}
