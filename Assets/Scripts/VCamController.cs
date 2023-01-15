using UnityEngine;
using Cinemachine;

public class VCamController : MonoBehaviour
{
    public CinemachineDollyCart cart;
    public CinemachineSmoothPath path;

    private float tiltStartPoint;
    private float tiltEndPoint;
    public float tiltSpeed = 2.8f;

    void Awake() {
        tiltStartPoint = path.m_Waypoints[1].position.z;    
        tiltEndPoint = path.m_Waypoints[2].position.z;    
    }
    
    void Update() {
        bool isTilting = 
            (cart.m_Position > -tiltStartPoint) && 
            (cart.m_Position < -tiltEndPoint);

        if (isTilting) {
            TiltUp();
        }
    }

    private void TiltUp() {
        transform.RotateAround(
            transform.position, 
            Vector3.right, 
            Time.deltaTime * tiltSpeed
        );
    }
}
