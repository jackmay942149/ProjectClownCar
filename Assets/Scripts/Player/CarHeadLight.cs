using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderKeywordFilter;
using UnityEngine;

public class CarHeadLight : MonoBehaviour
{
    public GameObject[] headLights;
    private bool headLightsOn = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L)){
            headLightsOn = !headLightsOn;
            foreach (GameObject light in headLights){
                light.SetActive(headLightsOn);
            }
        }
    }
}
