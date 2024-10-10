using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarBrakeLights : MonoBehaviour
{
    private bool brakeLightsOn = true;
    public Material brakeMat;
    public GameObject[] brakeLights;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxisRaw("Horizontal") == 0 && Input.GetAxisRaw("Vertical") == 0 && !brakeLightsOn){
            TurnLightsOn();
        }
        else if ((Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0) && brakeLightsOn) {
            TurnLightsOff();
        }
    }

    void TurnLightsOn(){
        brakeLightsOn = true;
        brakeMat.EnableKeyword("_EMISSION");

        foreach (GameObject light in brakeLights){
            light.SetActive(true);
        }
    }

    void TurnLightsOff(){
        brakeLightsOn = false;
        brakeMat.DisableKeyword("_EMISSION");

        foreach (GameObject light in brakeLights){
            light.SetActive(false);
        }
    }
}
