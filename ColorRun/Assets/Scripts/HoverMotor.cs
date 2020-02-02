using UnityEngine;
using System.Collections;

public class HoverMotor : MonoBehaviour
{

    public float speed = 90f;
    public float turnSpeed = 5f;
    public float smoothing = .5f;
    public float hoverForce = 65f;
    public float hoverHeight = 3.5f;
    public ParticleSystem burnerParticles;
    public Light headlight;
    private float powerInput;
    private float turnInput;
    private Rigidbody carRigidbody;

    private bool accelerating = false;
    private bool reversing = false;
    private bool lightsOn = false;

    void Awake()
    {
        carRigidbody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (Input.GetButton("Vertical"))
        {
            accelerating = true;
        }
        else
        {
            accelerating = false;
            if (Input.GetButton("Reverse"))
            {
                reversing = true;
            }
            else
            {
                reversing = false;
            }
        }

        if (Input.GetButtonDown("Jump"))
        {
            lightsOn = !lightsOn;
            headlight.enabled = lightsOn;
        }
        

        turnInput = Input.GetAxis("Horizontal");
    }

    void FixedUpdate()
    {
        Ray ray = new Ray(transform.position, -transform.up);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, hoverHeight))
        {
            float proportionalHeight = (hoverHeight - hit.distance) / hoverHeight;
            Vector3 appliedHoverForce = Vector3.up * proportionalHeight * hoverForce;
            carRigidbody.AddForce(appliedHoverForce, ForceMode.Acceleration);
        }
        float tempInput = 0f;

        float smoothedTurn = Mathf.Lerp(turnInput, tempInput, smoothing);

        if (accelerating)
        {
            burnerParticles.Play();
            carRigidbody.AddForce(transform.forward * speed, ForceMode.Acceleration);
            reversing = false;
        }
        else if (reversing)
        {
            
            carRigidbody.AddForce(transform.forward * -speed * .5f, ForceMode.Acceleration);
        }
        else
        {
            burnerParticles.Stop();
        }

        carRigidbody.transform.Rotate(new Vector3(0f, smoothedTurn * turnSpeed, 0f));
    }

}