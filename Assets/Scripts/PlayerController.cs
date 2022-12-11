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

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //  Movement
        Translate();
        Rotate();
    }

    private void Translate() {
        float horizontalInput = Input.GetAxis("Horizontal");    
        float verticalInput = Input.GetAxis("Vertical");

        transform.Translate(
            new Vector3(
                horizontalInput, 
                0,
                verticalInput
            ) * moveSpeed * Time.deltaTime
        );
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
                print("direction: " + rotationDirection + ", target: " + targetAngle);
                
                rotationTarget = Quaternion.Euler(0, targetAngle, 0);
            }
        }
    }
}
