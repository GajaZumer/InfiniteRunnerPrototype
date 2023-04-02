using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//at dash move semistatic objects in scene (other blood cells)
public class MoveSemiStatic : MonoBehaviour
{
    private Rigidbody rb;

    void Awake()
    {
        //zasledila post na StackOverflow-u, kjer se gameObject.GetComponent<Rigidbody>() ni izvrsil pravocasno v metodi Start in je zato FixedUpdate vrnil null
        rb = gameObject.GetComponent<Rigidbody>();
    }

    void Start()
    {
        PlayerController.current.onDash += onDash;
    }

    void OnDisable()
    {
        PlayerController.current.onDash -= onDash;
    }

    void onDash()
    {
        rb.velocity = new Vector3(0.0f, 0.0f, transform.localScale.x * Heart.DashVelocity);
    }
}
