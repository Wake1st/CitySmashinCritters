using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
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
}
