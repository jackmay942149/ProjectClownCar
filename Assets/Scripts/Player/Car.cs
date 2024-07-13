using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{
    [Header("PickUp Information")]
    public GameObject[] clownPositions;
    public GameObject[] clowns;

    [Header("Car Movement Information")]
    private Rigidbody rb;
    private float forwardInput; // Player input
    private float sidewaysInput; // Player Input

    [Header("Updated Car Movement Mechanics")]
    [SerializeField] private Suspension[] suspensions;
    [SerializeField] private float accelerationFactor = 1.0f;
    [SerializeField] private float turningFactor = 30.0f;
 
    [Header("Objective Information")]
    private ObjectiveManager objectiveManager;

    void Start(){
        rb = GetComponent<Rigidbody>();
        objectiveManager = GameManager.GetInstance().objectiveManager.GetComponent<ObjectiveManager>();
    }

    void Update(){
        // Enable Objective Text
        if (Input.GetKeyDown("o"))
        {
            objectiveManager.EnableText();
        }

        // Get input from arrow keys
        forwardInput = Input.GetAxisRaw("Vertical");
        sidewaysInput = Input.GetAxisRaw("Horizontal");
    }

    void FixedUpdate(){

        float forwardMovement = forwardInput * Time.fixedDeltaTime * accelerationFactor * 10000;
        float sidewaysMovement = sidewaysInput * Time.fixedDeltaTime * turningFactor;

        foreach (Suspension suspension in suspensions){
            bool inputForward = true;
            if (forwardInput == 0.0f) {inputForward = false;}
            suspension.UpdateCurrentForce(forwardMovement, inputForward);
        }

        foreach (Suspension suspension in suspensions){
            Vector3 forceToApply = transform.TransformDirection(suspension.currentForce);
            if (suspension.steeringWheel){forceToApply = Quaternion.Euler(0, turningFactor * sidewaysInput, 0) * forceToApply;}
            if (suspension.isTouchingGround){
                rb.AddForceAtPosition(forceToApply, suspension.transform.position);
            }
            
        }     
    }

    public void PickupClown(GameObject clown, int positionInCar)
    {
        clown.transform.position =  new Vector3 (0.0f, 0.0f, 0.0f);
        clown.transform.rotation = clownPositions[positionInCar].transform.rotation;
        clown.transform.SetParent(clownPositions[positionInCar].transform, false);
        clowns[positionInCar] = clown;
    }

    public void DropOff(string dropOffName)
    {
        // Count the number of clowns
        int clownCounter = 0;
        int cashCounter = 0;
        foreach (GameObject clown in clowns) {
            if (clown != null) {
                clownCounter += 1;
                cashCounter += clown.GetComponent<Clown>().cashHeld;
            }
            Destroy(clown);
        }
        objectiveManager.CheckObjectives(clownCounter, dropOffName, cashCounter);
    }
}
