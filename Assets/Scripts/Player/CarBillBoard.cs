using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarBillBoard : MonoBehaviour
{
    public Material billboardMat;
    void Start(){
        billboardMat = GetComponent<MeshRenderer>().material;
    }
    
    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Vector3 collisionPoint = collision.ClosestPoint(collision.gameObject.transform.position);

            // Update the shader with the collision position and size
            billboardMat.SetVector("_HitPosition", new Vector3(collisionPoint.x, collisionPoint.y, this.transform.position.z));
            Debug.Log(collisionPoint.x + ", " + collisionPoint.y + ", " + this.transform.position.z);
            billboardMat.SetFloat("_WholeFloat", 0.0f);
        }
    }
}
