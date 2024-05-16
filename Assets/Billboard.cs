using UnityEngine;

public class BillboardBreaker : MonoBehaviour
{
    public GameObject leftHalf;  // Assign the left half of the billboard in the Inspector
    public GameObject rightHalf; // Assign the right half of the billboard in the Inspector
    public float breakForce; // Adjust this value as needed
    public float randomForce;
    public float pullawayForce;

    private bool isBroken = false;

    void OnTriggerEnter(Collider other)
    {
        if (!isBroken && other.CompareTag("Player")) // Assuming the car has the tag "Car"
        {
            BreakBillboard(other);
        }
    }

    void BreakBillboard(Collider car)
    {
        isBroken = true;

        // Enable physics on the billboard halves
        Rigidbody leftRb = leftHalf.GetComponent<Rigidbody>();
        Rigidbody rightRb = rightHalf.GetComponent<Rigidbody>();

        leftRb.isKinematic = false;
        rightRb.isKinematic = false;

        // Get the car's velocity and direction
        Vector3 carVelocity = car.attachedRigidbody.velocity;
        Vector3 breakDirection = carVelocity.normalized;

        // Apply forces to simulate breaking
        Vector3 leftForce = (breakDirection * breakForce) + (Vector3.left * Random.Range(0.5f, 1.5f) * pullawayForce) + (Random.insideUnitSphere * randomForce);
        Vector3 rightForce = (breakDirection * breakForce) + (Vector3.right * Random.Range(0.5f, 1.5f) * pullawayForce) + (Random.insideUnitSphere * randomForce);

        leftRb.AddForce(leftForce, ForceMode.Impulse);
        rightRb.AddForce(rightForce, ForceMode.Impulse);

    }
}
