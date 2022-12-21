using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackBoxController : MonoBehaviour
{
    private Vector3 characterPosition;
    private Quaternion characterRotation;

    void Start()
    {
        PlayerController.TranslateAttackDirection += TranslateAttackBox;
    }

    void Update() {
        Transform parentTransform = this.transform.parent.transform;

        characterPosition = parentTransform.position;
        characterRotation = parentTransform.rotation;
    }

    private void TranslateAttackBox(Vector3 direction) {
        // print(direction);
        this.transform.position = characterPosition
            + direction * 0.5f 
            + new Vector3(
                0,
                0.5f,
                0
            );
    }
}
