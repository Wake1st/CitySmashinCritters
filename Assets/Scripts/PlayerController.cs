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

    public delegate void TransAtkDir(Vector3 direction);
    public static event TransAtkDir TranslateAttackDirection;
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

        if (direction.x == 0 && direction.z == 0) {
            return;
        }

        Vector3 normalizeDirection = Vector3.Normalize(direction);
        TranslateAttackDirection(normalizeDirection);
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
            }
        }
    }
}
