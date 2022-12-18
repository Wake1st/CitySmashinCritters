using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 1.2f;
    public float rotationSpeed = 10;
    
    private bool rotating = false;
    private Quaternion rotationTarget;
    private float targetAngle = 0;
    private float tiltAngle = 90;

    public delegate void UpdateAtkDir(Vector3 direction);
    public static event UpdateAtkDir UpdateAttackDirection;
    private Vector3 attackDirection;

    void Update()
    {
        //  Movement
        Translate();
        Rotate();
    }

    private void Translate() {
        float horizontalInput = Input.GetAxis("Horizontal");    
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(
            horizontalInput, 
            0,
            verticalInput
        );

        transform.Translate(
            direction * moveSpeed * Time.deltaTime
        );

        Vector3 normalizeDirection = Vector3.Normalize(direction);
        UpdateAttackDirection(normalizeDirection);
        // if (normalizeDirection != attackDirection) {
        //     attackDirection = normalizeDirection;
        // }
    }

    private void Rotate() {
        if (rotating) {
            transform.rotation = Quaternion.Slerp(
                transform.rotation, 
                rotationTarget, 
                Time.deltaTime * rotationSpeed
            );

            rotating = !(transform.rotation == rotationTarget);
        } else {
            rotating = Input.GetButton("RotateCW") || Input.GetButton("RotateCCW");

            if (rotating) {
                float rotationDirection = Input.GetButton("RotateCW") ? 1 : -1;
                targetAngle += (tiltAngle * rotationDirection);                
                rotationTarget = Quaternion.Euler(0, targetAngle, 0);

                UpdateAttackDirection(new Vector3(
                    0,
                    targetAngle,
                    0
                ));
            }
        }
    }
}
