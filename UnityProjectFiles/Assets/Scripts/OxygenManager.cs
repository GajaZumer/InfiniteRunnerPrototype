using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OxygenManager : MonoBehaviour
{
    private Rigidbody rb_O2;
    private float accelRate;
    
    void Awake()
    {
        rb_O2 = gameObject.GetComponent<Rigidbody>();
    }

    void Start()
    {
        accelRate = Heart.AccelRate;
    }

    void OnEnable()
    {
        PlayerController.current.onDash += onDash;
        Heart.current.onPump += onPump;
    }

    void OnDisable()
    {
        PlayerController.current.onDash -= onDash;
        Heart.current.onPump -= onPump;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy")) //better than other.gameObject.tag == "Enemy", because it doesn't allocate memory (https://answers.unity.com/questions/200820/is-comparetag-better-than-gameobjecttag-performanc.html)
        {
            gameObject.SetActive(false);
        }

        if ((other.CompareTag("Player")) && (PlayerController.NumOxygen < 4))
        {
            gameObject.SetActive(false);
        }
    }

    private void onPump(float accelRate)
    {
        rb_O2.AddForce(new Vector3(0.0f, 0.0f, accelRate), ForceMode.Impulse);
    }

    private void onDash()
    {
        rb_O2.velocity = new Vector3(0.0f, 0.0f, transform.localScale.x * Heart.DashVelocity);
    }

    //oxygen is out of sight
    void Update()
    {
        if (transform.position.z <= Camera.main.transform.position.z)
        {
            gameObject.SetActive(false);
        }
    }
}
