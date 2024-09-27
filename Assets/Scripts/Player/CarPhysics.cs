using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


[System.Serializable]
class Wheel {
    public GameObject wheel;
    public bool isSteering;
    public bool isDriving;
    public bool isGrounded;
    [HideInInspector] public float startingHeight;
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

        Vector3 newPos = transform.position;
        foreach (Wheel w in wheels){
            if (w.isGrounded && w.isDriving){
                newPos += transform.forward * (wsInput * speed);
                break;
            }
        }
        rb.MovePosition(newPos);

        // Adjust Car Body Rotation .. need to calculate the tilt (average front wheel height vs back heel height), yaw (wheel speed and direction), roll all based of the wheel locations (average left wheel height vs right wheel height)
        float steeringDirection = 0;
        
        foreach (Wheel w in wheels){
            if (w.isGrounded && w.isSteering){
                
            }
        }

        if (Mathf.Abs(steeringDirection - (360 * steeringSpeed)) >= 0.1 && steeringDirection != 0){
            Debug.Log(steeringDirection - (360 * steeringSpeed));
            Quaternion deltaRotation = Quaternion.Euler(0, steeringDirection, 0);
            rb.MoveRotation(rb.rotation * deltaRotation);
        }
        

        // Calculate the rotation based on the steering input
        float steeringInput = wsInput * steeringSpeed; // Assuming wsInput is -1 for left, +1 for right

        // Create a quaternion representing the steering rotation
        Quaternion steeringRotation = Quaternion.Euler(0, steeringInput * Time.deltaTime, 0);

        // Apply the rotation to the rigidbody
        rb.MoveRotation(rb.rotation * steeringRotation);
        
        // Adjust Car Position ... adjust forward movement based of wheel speed
    }
    
}
