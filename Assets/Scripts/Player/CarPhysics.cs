using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Experimental.AI;
using UnityEngineInternal.XR.WSA;


[System.Serializable]
class Wheel {
    public GameObject wheel;
    public bool isSteering;
    public bool isDriving;
    public bool isGrounded;
    [HideInInspector] public float startingHeight;
    [HideInInspector] public float suspensionLengthLastFrame = 0.0f;
}

public class CarPhysics : MonoBehaviour
{
    // Object References
    [Header("Object References")]
    [SerializeField] private List<Wheel> wheels;
    [SerializeField] private GameObject steeringWheel;

    // Component References
    private Rigidbody rb;

    // Input Variables
    private float adInput;
    private float wsInput;

    // Adjustable Values
    [Header("Wheel Settings")]
    public float wheelRotationSpeed;
    public float maxWheelTurnAngleDegrees;
    public float minWheelHeight;
    public float maxWheelHeight;
    public float axleHeight;

    [Header("Steering Wheel Settings")]
    public float maxSteeringWheelTurnAngleDegrees;

    [Header("Car Settings")]
    public float speed;
    public float steeringSpeed;
    public float suspensionStrength;
    public float suspensionDamping;
    public float stationaryTurnSpeed;


    

    // Start is called before the first frame update
    void Start(){
        rb = GetComponent<Rigidbody>();

        foreach (Wheel w in wheels){
            w.startingHeight = w.wheel.transform.localPosition.y;
            Debug.Log(w.startingHeight);
        }

        
    }

    // Update is called once per frame
    void Update(){
        adInput = Input.GetAxis("Horizontal");
        wsInput = -Input.GetAxisRaw("Vertical");
    }

    void FixedUpdate(){
        // Adjust Wheels Rotation ... need to calculate the tilt (moving forward and backward) as well as yaw (steering left and right)
        foreach (Wheel w in wheels){
            if (w.isSteering) {
                Quaternion targetRot = Quaternion.Euler(w.wheel.transform.localRotation.eulerAngles.x, maxWheelTurnAngleDegrees * adInput, w.wheel.transform.localRotation.eulerAngles.z);
                w.wheel.transform.localRotation = targetRot;
                targetRot = Quaternion.Euler(wsInput * wheelRotationSpeed, 0, 0);
                w.wheel.transform.localRotation *= targetRot;
            }
            else {
                Quaternion targetRot = Quaternion.Euler(wsInput * wheelRotationSpeed, 0, 0);
                w.wheel.transform.localRotation *= targetRot;
            }
        }

        // Adjust Steering Wheel Rotation
        if (steeringWheel) {
            steeringWheel.transform.SetLocalPositionAndRotation(steeringWheel.transform.localPosition,
                Quaternion.Euler(0, 0, adInput * maxSteeringWheelTurnAngleDegrees));
        }
        

        // Adjust Wheels Movement .. need to calculate height (raycast to ground)
        foreach (Wheel w in wheels){
            // Checks Gound
            RaycastHit hit;
            float raycastDist = maxWheelHeight + minWheelHeight + axleHeight;
            Vector3 raycastPos = new Vector3(w.wheel.transform.position.x, w.wheel.transform.position.y + w.startingHeight + minWheelHeight - w.wheel.transform.localPosition.y, w.wheel.transform.position.z);
            w.isGrounded = Physics.Raycast(raycastPos, Vector3.down, out hit, raycastDist, LayerMask.GetMask("Ground"));

            if (!w.isGrounded){
                w.wheel.transform.localPosition = new Vector3(w.wheel.transform.localPosition.x, w.startingHeight - maxWheelHeight, w.wheel.transform.localPosition.z);       
            } 
            else {
                w.wheel.transform.localPosition = new Vector3(w.wheel.transform.localPosition.x, w.startingHeight + minWheelHeight - hit.distance + axleHeight, w.wheel.transform.localPosition.z);
            }
            
        }

        // Apply suspension force
        foreach(Wheel w in wheels) {
            Vector3 suspensionForce = Vector3.zero;
            suspensionForce.y = ((w.wheel.transform.localPosition.y - w.startingHeight)  * suspensionStrength) - (suspensionDamping * (- w.wheel.transform.localPosition.y + w.suspensionLengthLastFrame));
            
            Vector3 endLinePos = w.wheel.transform.position + suspensionForce;

            Debug.DrawLine(w.wheel.transform.position, endLinePos, Color.green, Time.fixedDeltaTime);

            w.suspensionLengthLastFrame = w.wheel.transform.localPosition.y;
            rb.AddForceAtPosition(suspensionForce, w.wheel.transform.position);
        }


        // Move Car if driving wheels on the ground 
        Vector3 newPos = Vector3.zero;
        foreach (Wheel w in wheels){
            if (w.isGrounded && w.isDriving){
                Vector3 accelForce = transform.forward * (wsInput * speed);


                if (w.isSteering){
                    if (wsInput == 0 && Mathf.Abs(adInput) == 1){accelForce = transform.forward * (-stationaryTurnSpeed * speed);}

                    Quaternion rot = Quaternion.AngleAxis(adInput * steeringSpeed, transform.up);
                    accelForce = rot * accelForce;
                }
                
                rb.AddForceAtPosition(accelForce, w.wheel.transform.position);
                Debug.DrawLine(w.wheel.transform.position, w.wheel.transform.position + accelForce, Color.blue, Time.fixedDeltaTime);
            }
        }

        /*

        // Rotate Car if steering wheels on the ground TODO: Adjust steering speed based of cuurent rb velocity and make it a force
        Quaternion steeringDirection = Quaternion.identity;
        
        foreach (Wheel w in wheels){
            if (w.isGrounded && w.isSteering){
                Quaternion wheelAngle = Quaternion.Euler(0, adInput * steeringSpeed, 0);
                steeringDirection = transform.rotation * wheelAngle;
            }
        }
        rb.MoveRotation(steeringDirection); // or just put in steering direction

        // TODO: Adjust Car Body Rotation .. need to calculate the tilt (average front wheel height vs back heel height), roll all based of the wheel locations (average left wheel height vs right wheel height)
        */

        // Add drag
    }
    
}
