using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{
    [Header("PickUp Information")]
    public GameObject[] clownPositions;
    public GameObject[] clowns;

    
    public void PickupClown(GameObject clown, int positionInCar)
    {
        clown.transform.position =  new Vector3 (0.0f, 0.0f, 0.0f);
        clown.transform.rotation = clownPositions[positionInCar].transform.rotation;
        clown.transform.SetParent(clownPositions[positionInCar].transform, false);
        clowns[positionInCar] = clown;
    }
}
