using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Suspension : MonoBehaviour {
    [SerializeField] public Vector3 currentForce = new Vector3(0.0f, 0.0f, 0.0f);
    [SerializeField] public float maxSpeed = 60.0f;
    [SerializeField] public float maxTurningAngle = 60.0f;
    [SerializeField] public float maxSuspensionLength;
    [SerializeField] public float distToGround;
    [SerializeField] public bool isTouchingGround;
    [SerializeField] public float dragFactor = 0.9f;
    [SerializeField] public float maxGroundCheckDist = 1.0f;
    [SerializeField] public bool steeringWheel;

    public void UpdateCurrentForce(float forwardMovement, bool inputForward){
        CheckGround();

        // Calculate curent forward force
        if (!inputForward){ 
            currentForce.z = 0.0f; // Reset force if player is not pressing forward/backward
        }
        else if (isTouchingGround) {
            currentForce.z += (forwardMovement * dragFactor); // with drag
        }
        else {
            currentForce.z += forwardMovement; // without drag
            
        }

        // Max out forward force
        if (currentForce.z > maxSpeed * 1000) {currentForce.z = maxSpeed * 1000;}
        if (currentForce.z < maxSpeed * -1000) {currentForce.z = -maxSpeed * 1000;}  
               
    }

    public void CheckGround(){
        RaycastHit hit;
        LayerMask mask = LayerMask.GetMask("Ground");
        if (Physics.Raycast(transform.position, Vector3.down, out hit, maxGroundCheckDist, mask)){
            distToGround = hit.distance;
            isTouchingGround = true;
        }
        else{
            distToGround = maxGroundCheckDist;
            isTouchingGround = false;
        }
    }
}