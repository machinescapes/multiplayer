using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public float WalkSpeed = 4f;
    public float MaxVelocityChange = 10f;
    public float sprintSpeed = 14f;
    [Space]
    public float airControl = 0.5f;




    [Space]
    public float jumpheight = 30f;





    private Vector2 input;
    private Rigidbody rb;


    private bool sprinting;
    private bool jumping;

    private bool grounded = false;
    [Header("Animaton")]
    public Animation handAnimaton;
    public AnimationClip handWalkAnimation;
    public AnimationClip IdleAnimation;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        input = new Vector2(x:Input.GetAxisRaw("Horizontal"), y:Input.GetAxisRaw("Vertical"));
        input.Normalize();

        sprinting = Input.GetButton("Sprint");
        jumping = Input.GetButton("Jump");


    }


    private void OnTriggerStay(Collider other)
    {
        grounded = true;
    }
    
    void FixedUpdate()
    {
        if (grounded)
        {
            if (jumping)
            {
                rb.velocity = new Vector3(rb.velocity.x, y:jumpheight, z:rb.velocity.z);
            }
            else if (input.magnitude > 0.5f)
            {
                handAnimaton.clip = handWalkAnimation;
                handAnimaton.Play();

                rb.AddForce(CalculateMovement(sprinting ? sprintSpeed : WalkSpeed), ForceMode.VelocityChange);
            }
            else
            {
                handAnimaton.clip = IdleAnimation;
                handAnimaton.Play();
                // add :vector 3
                Vector3 velocity1 = rb.velocity;

                velocity1 = new Vector3(x: velocity1.x * 0.2f * Time.fixedDeltaTime, y: velocity1.y, z: velocity1.z * 0.2f * Time.fixedDeltaTime);
                rb.velocity = velocity1;
            }

            grounded = false;
        }
        else
        {
            if (input.magnitude > 0.5f)
            {

                rb.AddForce(CalculateMovement(sprinting ? sprintSpeed * airControl : WalkSpeed * airControl), ForceMode.VelocityChange);
            }
            else
            {
                
                // add :vector 3
                Vector3 velocity1 = rb.velocity;

                velocity1 = new Vector3(x: velocity1.x * 0.2f * Time.fixedDeltaTime, y: velocity1.y, z: velocity1.z * 0.2f * Time.fixedDeltaTime);
                rb.velocity = velocity1;
            }
        }


       
    }
  
    Vector3 CalculateMovement(float _speed)
    {
        Vector3 targetVelocity = new Vector3(input.x, y:0, z:input.y);
        targetVelocity = transform.TransformDirection(targetVelocity);
        targetVelocity *= _speed;

        Vector3 velocity = rb.velocity;

        if(input.magnitude > 0.5)
        {
            Vector3 velocityChange = targetVelocity - velocity;

            velocityChange.x = Mathf.Clamp(value: velocityChange.x, min: -MaxVelocityChange, MaxVelocityChange);
            velocityChange.z = Mathf.Clamp(value: velocityChange.z, min: -MaxVelocityChange, MaxVelocityChange);

            velocityChange.y = 0;

            return (velocityChange);
        }
        else
        {
            return new Vector3();
        }
        
        
    }


}
