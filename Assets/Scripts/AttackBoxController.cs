using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackBoxController : MonoBehaviour
{
    private Vector3 parentPosition;

    void Start()
    {
        PlayerController.UpdateAttackDirection += MoveAttackBox;
    }

    void Update() {
        parentPosition = gameObject.transform.parent.transform.position;
    }

    private void MoveAttackBox(Vector3 direction) {
        print(direction);
        this.transform.position = parentPosition 
            + direction * 0.5f 
            + new Vector3(
                0,
                0.5f,
                0
            );
    }
}
