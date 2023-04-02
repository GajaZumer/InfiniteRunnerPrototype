using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System; //potrebujemo zaradi Action event-a

public class PlayerController : MonoBehaviour
{
    public static PlayerController current;

    private Rigidbody rb;
    private Rigidbody rb_full;
    private Rigidbody rb_semi;
    private float moveSpeed;
    private float rotateSpeed;
    private float targetSpeedRotation;
    private float targetSpeedVertical;
    private float speedDifH;
    private float accelRateH;
    private float speedDifV;
    private float accelRateV;
    private float acceleration;
    private float decceleration;
    private float rotAcceleration;
    private float rotDecceleration;
    private float movementH;
    private float movementV;
    private float moveVertical;
    private float rotate;
    private int velPower;
    private bool isDashing = false;
    private Vector2 startPosition;
    private static int numOxygen;
    private static int numClots;
    private static float centerDist;
    //when dashing, a particle system trail starts playing
    private ParticleSystem trail;

    //game objects of other blood cells, blood vessel and oxygen
    public GameObject fullMovement;
    public GameObject semistatic;

    //particle system of explosions on collision with clots (even clot explodes or player explodes/dies)
    public GameObject psExplosionClot;
    public GameObject psExplosionPlayer;

    public static int NumOxygen
    {
        get {return numOxygen;  }
        set {numOxygen = value; }
    }

    public static int NumClots
    {
        get {return numClots; }
        set {numClots = value; }
    }

    public static float CenterDistance
    {
        get {return centerDist; }
        set {centerDist = value; }
    }

    public float MoveSpeed
    {
        get { return moveSpeed; }
        set { moveSpeed = value; }
    }

    private void Awake()
    {
        current = this;

        rb = gameObject.GetComponent<Rigidbody>();
        rb_full = fullMovement.GetComponent<Rigidbody>();
        rb_semi = semistatic.GetComponent<Rigidbody>();
        trail = gameObject.GetComponent<ParticleSystem>();
    }

    public void Start()
    {
        moveSpeed = 9.0f;
        rotateSpeed = 25.0f;

        acceleration = 0.1f;
        decceleration = -0.3f;

        rotAcceleration = -0.05f;
        rotDecceleration = 0.01f;

        velPower = 2;

        startPosition = new Vector2(transform.position.x, transform.position.y);
    }

    public event Action onDash;
    public void Dash()
    {
        if ((onDash != null) && (NumOxygen > 0))
        {
            onDash();
            isDashing = true;
            trail.Play();
            NumOxygen -= 1;
            StartCoroutine(WaitUntilEndOfDash());   
        }
    }

    IEnumerator WaitUntilEndOfDash()
    {
        yield return new WaitForSeconds(2.0f);
        isDashing = false;
    }

    public event Action onOxygenCollide;
    public void OxygenObtained()
    {
        if (onOxygenCollide != null)
        {
            onOxygenCollide();
        }
    }

    public event Action<int> onClotCollide;
    public void ClotCollide(int state)
    {
        if (onClotCollide != null)
        {
            onClotCollide(state);
        }
    }

    //get info about keys pressed
    public void Update()
    {
        targetSpeedRotation = rotateSpeed * Input.GetAxisRaw("Horizontal"); //Returns the value of the virtual axis; -1, 0, 1
        targetSpeedVertical = moveSpeed * Input.GetAxisRaw("Vertical");

        if (Input.GetKeyDown("space"))
        {
            Dash();
        }
    }

    //update physics (movement)
    void FixedUpdate()
    {
        //update vertical movement
        speedDifV = targetSpeedVertical - rb.velocity.y;
        accelRateV = (Mathf.Abs(targetSpeedVertical) > 0.01) ? acceleration : decceleration;
        movementV = Mathf.Pow(Mathf.Abs(speedDifV) * accelRateV, velPower) * Mathf.Sign(speedDifV);

        rb.AddForce(new Vector3(0.0f, movementV, 0.0f), ForceMode.Impulse);

        //calculate distance from the center of the blood vessel
        CenterDistance = Vector2.Distance(new Vector2(transform.position.x, transform.position.y), startPosition);
        //stop vertical movement, if distance from center is too big 
        if (CenterDistance > 5.0f)
        {
            rb.velocity = Vector3.zero;
        }

        //update rotational movement of the environment (blood vessels, other blood cells, oxygen)
        speedDifH = targetSpeedRotation - rb_full.angularVelocity.z;
        accelRateH = (Mathf.Abs(targetSpeedRotation) > 0.01) ? rotAcceleration : rotDecceleration;
        movementH = Mathf.Pow(Mathf.Abs(speedDifH) * accelRateH, 3) * Mathf.Sign(speedDifH);

        fullMovement.transform.Rotate(0.0f, 0.0f, movementH, Space.World);
        semistatic.transform.Rotate(0.0f, 0.0f, movementH, Space.World);
    }

    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            if (!isDashing)
            {
                ClotCollide(0);
                psExplosionPlayer.transform.position = gameObject.transform.position + new Vector3(0.0f, 0.0f, -4.0f);
                psExplosionPlayer.GetComponent<ParticleSystem>().Play();
                gameObject.SetActive(false);
            }
            else
            {
                NumClots += 1;
                ClotCollide(1);
                psExplosionClot.transform.position = collision.gameObject.transform.position;
                psExplosionClot.GetComponent<ParticleSystem>().Play();
                collision.gameObject.SetActive(false);
            }

        }
        if (collision.gameObject.tag == "Oxygen")
        {
            if (NumOxygen < 4)
            {
                NumOxygen += 1;
            }
            OxygenObtained();
        }
    }
}
