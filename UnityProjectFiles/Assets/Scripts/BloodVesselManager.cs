using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BloodVesselManager : MonoBehaviour
{
    private Rigidbody rb;
    private float accelRate;
    private float radius;
    private float distFromCenter;

    //list of clots (gameobject), that can appear in blood vessel
    [SerializeField]
    private List<GameObject> ClotsList;
    private GameObject Clot;
    private int randomNumber;

    public GameObject previousBloodVessel; 

    void Awake()
    {
        //zasledila post na StackOverflow-u, kjer se gameObject.GetComponent<Rigidbody>() ni izvrsil pravocasno v metodi Start in je zato FixedUpdate vrnil null
        rb = gameObject.GetComponent<Rigidbody>();
    }

    void Start()
    {
        accelRate = Heart.AccelRate;
        radius = Heart.Radius;

        Heart.current.onPump += onPump;
        PlayerController.current.onDash += onDash;
    }

    void OnDisable()
    {
        Heart.current.onPump -= onPump;
        PlayerController.current.onDash -= onDash;
    }

    private void onPump(float accelRate)
    {
        rb.AddForce(new Vector3(0.0f, 0.0f, accelRate), ForceMode.Impulse);
    }

    private void onDash()
    {
        rb.velocity = new Vector3(0.0f, 0.0f, transform.localScale.x * Heart.DashVelocity);
    }
   
    void Update()
    {
        //blood vessel is out of sight
        if (transform.position.z <= Camera.main.transform.position.z)
        {
            //clot manager
            // deactivate blood clot
            if (Clot != null) Clot.SetActive(false);

            //activate new clot (with probability 40%)
            if (Random.value < 0.4f)
            {
                //choose 3D model of clot
                randomNumber = Random.Range(0, ClotsList.Count);
                Clot = ClotsList[randomNumber];
                //set position of a 3D model at a random point on a circle with a radius of 6.25f
                Clot.SetActive(true);
                Clot.transform.localPosition = (radius - 0.75f) * Random.insideUnitCircle.normalized;
            }

            //reset position of blood vessel 
            transform.position = previousBloodVessel.transform.position + new Vector3(0.0f, 0.0f, 10.0f);
            
            //TIP
            //Avoid regularly setting transform.postion and transform.rotation on rigidbodies. PhysX must do a big solve when this happens. This reduces the stability of the physics simulation and has a heavy performance cost.
            //vir: https://digitalopus.ca/site/using-rigid-bodies-in-unity-everything-that-is-not-in-the-manual/
            //alternativa: rb.velocity = new Vector3(0.0f, 0.0f, 100.0f / Time.deltaTime);
        }

        //laminar blood flow
        distFromCenter = PlayerController.CenterDistance;
        rb.drag = 1.5f / 7.0f * distFromCenter + 1.0f;
    }
}
