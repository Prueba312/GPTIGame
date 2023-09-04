using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("References")]
    public Rigidbody Rb;
    public Transform head;
    public Camera camera;

    [Header("Consfigurations")]
    public float WalkSpeed;
    public float runSpeed;
    public float jumpSpeed;

    [Header("Runtime")]
    Vector3 newVelocity;
    bool isGrounded = false;
    bool isJumping = false;

    void Start() 
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;        
    }
    void Update() 
    {
        transform.Rotate(Vector3.up, Input.GetAxis("Mouse X") * 2f);

        newVelocity = Vector3.up * Rb.velocity.y;
        float speed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : WalkSpeed;

        newVelocity.x = Input.GetAxis("Horizontal") * speed;
        newVelocity.z = Input.GetAxis("Vertical") * speed;

        if(isGrounded)
        {
            if(Input.GetKeyDown(KeyCode.Space) && !isJumping)
            {
                newVelocity.y = jumpSpeed;
                isJumping = true;
            }
        }
        Rb.velocity = transform.TransformDirection(newVelocity);
    }
    void FixedUpdate() 
    {
        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, 1f)) {
            isGrounded = true;
        }
        else isGrounded = false;
    }

    void LateUpdate()
    {
        Vector3 e = head.eulerAngles;
        e.x -= Input.GetAxis("Mouse Y") * 2f;
        e.x = RestrictAngle(e.x, -85f, 85f);
        head.eulerAngles = e;
    }

    void OnCollisionStay(Collision col)
    {
        isGrounded = true;
        isJumping = false;
    }

    void OnCollisionExit(Collision col)
    {
        isGrounded = false;
    }

    public static float RestrictAngle(float Angle, float AngleMin, float AngleMax)
    {
        if (Angle > 180)
        {
            Angle -= 360;
        }
        else if (Angle < -180)
        {
            Angle += 360; 
        }
            
        
        if(Angle > AngleMax)
            Angle = AngleMax;
        if(Angle < AngleMin)
            Angle = AngleMin;
        
        return Angle;

    }
}
