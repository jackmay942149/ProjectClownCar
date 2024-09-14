using System.Collections;
using System.Collections.Generic;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.XR;

public class CarPhysics : MonoBehaviour
{
    private Rigidbody rb;
    private float yInput;
    private float xInput;

    public float steeringSpeed;
    public float acceleration;
    public float maxVelocity;
    public float breakingPower;
    public float breakingThreshold;
    public float stoppingThreshold;

    // Start is called before the first frame update
    void Start(){
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update(){
        xInput = Input.GetAxisRaw("Horizontal");
        yInput = Input.GetAxisRaw("Vertical");

        Debug.Log(xInput.ToString() + yInput.ToString());
    }

    void FixedUpdate(){
        // Apply A and D input
        Vector3 steeringRotation = Vector3.zero;
        steeringRotation.y = xInput * steeringSpeed;

        // Apply W and S input
        Vector3 accelerator = Vector3.zero;
        accelerator.z = -yInput * acceleration;

        // Steer
        transform.Rotate(steeringRotation);


        // Accelerate
        transform.Translate(accelerator);

        /*
        if (accelerator.magnitude > breakingThreshold){ // If accellerating, accelerate
            currentVelocity.z += accelerator.z;
        }
        else if (Mathf.Abs(currentVelocity.z) < stoppingThreshold){ // If stopped dont brake
            currentVelocity.z = 0;
        }
        else { // Otherwise break
            if (currentVelocity.z > 0) {currentVelocity.z -= breakingPower;}
            if (currentVelocity.z < 0) {currentVelocity.z += breakingPower;}
        }

        rb.velocity = currentVelocity;
        */
        
    }
    
}
