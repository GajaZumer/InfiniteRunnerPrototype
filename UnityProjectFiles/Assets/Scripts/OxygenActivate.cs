using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OxygenActivate : MonoBehaviour
{
    private GameObject oxygen;
    private float radius;
    private float timeToSpawnNewOxygen = 2.0f;

    void Start()
    {
        //every 2 seconds activate new Oxygen
        InvokeRepeating("LaunchOxygen", 0.0f, timeToSpawnNewOxygen);
        radius = Heart.Radius;
    }    

    void LaunchOxygen()
    {
        GameObject oxygen = OxygenPool.SharedInstance.GetPooledOxygen();
        if (oxygen != null)
        {
            //set new oxygen position to a random point on a circle with a radius of 6.25f
            oxygen.transform.localPosition = (radius - 0.75f) * Random.insideUnitCircle; 
            oxygen.transform.localPosition +=  new Vector3(0.0f, 0.0f, 70.0f);
            oxygen.SetActive(true);
        }
    }
}
