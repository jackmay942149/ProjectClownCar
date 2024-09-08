using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarBasic : MonoBehaviour
{
    [Header("PickUp Information")]
    public GameObject[] clownPositions;
    public GameObject[] clowns;

    [Header("Car Movement Information")]
    private Rigidbody rb;
    private float forwardInput; // Player input
    private float sidewaysInput; // Player Input
    [SerializeField] private float maxSpeed;
    [SerializeField] private float acceleration;
    [SerializeField] private float turnAccel;
    [SerializeField] private float carRotationAccel;

    [Header("Objective Information")]
    private ObjectiveManager objectiveManager;

    // Start is called before the first frame update
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
        sidewaysInput = Input.GetAxis("Horizontal");
    }

    void FixedUpdate(){
        // Apply forward force
        Vector3 inputForce = Vector3.zero;

        if (rb.velocity.magnitude < maxSpeed){
            inputForce.z += (forwardInput * acceleration);
            Debug.Log("Adding Velocity");
        }

        inputForce.x += sidewaysInput * turnAccel * rb.velocity.magnitude;

        // Turn the car
        Vector3 turnVec = Vector3.zero;
        turnVec.y = sidewaysInput * carRotationAccel * rb.velocity.magnitude;
        transform.Rotate(turnVec);

        rb.AddRelativeForce(inputForce);
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
