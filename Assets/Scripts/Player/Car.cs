using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{
    [Header("PickUp Information")]
    public GameObject[] clownPositions;
    public GameObject[] clowns;

    [Header("Car Movement Information")]
    [SerializeField] private float speed;
    [SerializeField] private float rotationSpeed;
    private Rigidbody rb;
    private float translation; // Player input
    private float rotation; // Player Input

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
        translation = Input.GetAxis("Vertical") * speed;
        rotation = Input.GetAxis("Horizontal") * rotationSpeed;
    }

    void FixedUpdate(){
        // Move the car forward/backward
        Vector3 movement = transform.forward * translation * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + movement);

        // Rotate the car left/right
        Quaternion turnRotation = Quaternion.Euler(0f, rotation * Time.fixedDeltaTime, 0f);
        rb.MoveRotation(rb.rotation * turnRotation);        
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
