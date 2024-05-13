using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropOffLocation : MonoBehaviour
{
    [Header("DropOff Information")]
    [SerializeField] private string dropOffName;
    [SerializeField] private float dropOffRange;
    [SerializeField] private GameObject dropOffIndicator;
    private Car car;
    private bool isDroppingOff = false;

    // Start is called before the first frame update
    void Start()
    {
        car = GameManager.GetInstance().car.GetComponent<Car>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckForDropOff();
        if (isDroppingOff)
        {
            car.DropOff(dropOffName);
            isDroppingOff = false;
        }
    }

    private void CheckForDropOff()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (Vector3.Distance(dropOffIndicator.transform.position, car.transform.position) < dropOffRange)
            {
                isDroppingOff = true;
            }
        }
    }
}
