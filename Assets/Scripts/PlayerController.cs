using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 1.2f;
    public float rotationSpeed = 10;
    public Animator animator;
    
    private bool rotating = false;
    private Quaternion rotationTarget;
    private float targetAngle = 0;
    private float tiltAngle = 90;

    public delegate void TransAtkDir(Vector3 direction);
    public static event TransAtkDir TranslateAttackDirection;
    public delegate void RotAtkDir(float angle);
    public static event RotAtkDir RotateAttackDirection;
    private Vector3 attackDirection;

    private bool isPlaying;

    private void Start() {
        LevelController.UpdateIsPlaying += SetIsPlaying;
    }
    
    void Update()
    {
        if (isPlaying) {
            //  Movement
            Translate();
            Rotate();
        }
    }

    void SetIsPlaying(bool playing) {
        isPlaying = playing;
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
            animator.SetBool("Moving", false);
            return;
        }

        animator.SetBool("Moving", true);

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

                RotateAttackDirection(targetAngle);
            }
        }
    }
}
