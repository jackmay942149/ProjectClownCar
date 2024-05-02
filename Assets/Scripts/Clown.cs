using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clown : MonoBehaviour
{
    [Header("PickUp Information")]
    [SerializeField] private float pickupRange;
    [SerializeField] private GameObject pickupIndicator;
    [SerializeField] public int cashHeld;
    [SerializeField] private float cashDropRate; // per second
    private Car car;
    private CapsuleCollider cc;
    private Rigidbody rb;
    private bool isGettingPickedUp = false;
    private bool pickedUp = false;

    void Start()
    {
        cc = GetComponent<CapsuleCollider>();
        rb = GetComponent<Rigidbody>();
        car = GameManager.GetInstance().car.GetComponent<Car>();
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
        InvokeRepeating("DropCash", 0.0f, 1.0f);

        for (int i = 0; i < clowns.Length; i++)
        {
            if (clowns[i] == null)
            {
                cc.enabled = false;
                rb.isKinematic = true;
                pickupIndicator.SetActive(false);
                car.PickupClown(this.gameObject, i);
                pickedUp = true;
                return;
            }
        }
    }

    void DropCash()
    {
        cashHeld -= 1;
        if (cashHeld == 0) {CancelInvoke("DropCash");}
    }
}


