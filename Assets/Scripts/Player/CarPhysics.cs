using System.Collections;
using System.Collections.Generic;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.XR;


[System.Serializable]
class Wheel {
    public GameObject wheel;
    public bool isSteering;
    public bool isDriving;
    public bool isGrounded;
}

public class CarPhysics : MonoBehaviour
{
    // Object References
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

    [Header("Steering Wheel Settings")]
    public float maxSteeringWheelTurnAngleDegrees;


    

    // Start is called before the first frame update
    void Start(){
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update(){
        adInput = Input.GetAxis("Horizontal");
        wsInput = Input.GetAxisRaw("Vertical");

        // Debug.Log(adInput.ToString() + wsInput.ToString());
    }

    void FixedUpdate(){
        // Adjust Wheels Rotation ... need to calculate the tilt (moving forward and backward) as well as yaw (steering left and right)
        foreach (Wheel w in wheels){
            if (w.isSteering) {
                w.wheel.transform.SetLocalPositionAndRotation(w.wheel.transform.localPosition, 
                    Quaternion.Euler(w.wheel.transform.localRotation.eulerAngles.x + wsInput * wheelRotationSpeed, maxWheelTurnAngleDegrees * adInput, 0));
            }
            else {
                w.wheel.transform.SetLocalPositionAndRotation(w.wheel.transform.localPosition,
                    Quaternion.Euler(w.wheel.transform.localRotation.eulerAngles.x + wsInput * wheelRotationSpeed, 0, 0));
            }
        }

        /* Adjust Steering Wheel Rotation
        if (steeringWheel) {
            steeringWheel.transform.SetLocalPositionAndRotation(steeringWheel.transform.position,
                Quaternion.Euler(0, steeringWheel.transform.localRotation.eulerAngles.y + adInput * maxSteeringWheelTurnAngleDegrees, 0));
        }
        */

        // Adjust Wheels Movement .. need to calculate height (raycast to ground)
        foreach (Wheel w in wheels){
            RaycastHit hit;
            w.isGrounded = Physics.Raycast(w.wheel.transform.position, Vector3.down, out hit, maxWheelHeight, LayerMask.GetMask("Ground"));
        }

        // Adjust Car Body Rotation .. need to calculate the tilt (average front wheel height vs back heel height), yaw (wheel speed and direction), roll all based of the wheel locations (average left wheel height vs right wheel height)

        // Adjust Car Position ... adjust forward movement based of wheel speed
    }
    
}
