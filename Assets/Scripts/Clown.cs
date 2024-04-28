using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clown : MonoBehaviour
{
    [Header("PickUp Information")]
    [SerializeField] private float pickupRange;
    [SerializeField] private GameObject pickupIndicator;
    [SerializeField] private Car car;
    private CapsuleCollider collider;
    private Rigidbody rigidbody;
    private bool isGettingPickedUp = false;
    private bool pickedUp = false;

    void Start()
    {
        collider = GetComponent<CapsuleCollider>();
        rigidbody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (!pickedUp)
        {
            CheckForPickup();
            if (isGettingPickedUp) {EnterCar();}
        }
        
    }

    void CheckForPickup()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (Vector3.Distance(transform.position, car.transform.position) < pickupRange)
            {
                isGettingPickedUp = true;
            }
        }
    }

    void EnterCar()
    {
        GameObject[] clowns = car.clowns;

        for (int i = 0; i < clowns.Length; i++)
        {
            if (clowns[i] == null)
            {
                collider.enabled = false;
                rigidbody.isKinematic = true;
                pickupIndicator.SetActive(false);
                car.PickupClown(this.gameObject, i);
                pickedUp = true;
                return;
            }
        }
    }
}
