using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerMovement : MonoBehaviour
{
    //Ref to collider attached to object child
    public Collider playerCollider;

    [Header("Movement")]
    public float moveSpeed;
    public float drag;
    public float defaultDrag = 5f;

    [Header("Jumping")]
    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    public bool readyToJump;
    public bool jumpIsCooldowned;
    public float gravityScale = 5f;

    [Header("Surfaces")]
    public List<Surface> surfaces;
    public bool jumpableSurface;

    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;

    public Transform orientation;

    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;

    Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        jumpIsCooldowned = false;
    }

    private void Update()
    {
        MyInput();
    }

    private void FixedUpdate()
    {
        MovePlayer();

        rb.AddForce(Physics.gravity * (gravityScale - 1) * rb.mass);
    }

    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        if(Input.GetKey(jumpKey) && readyToJump && jumpableSurface)
        {
            readyToJump = false;

            Jump();

            jumpIsCooldowned = true;
            Invoke(nameof(ResetJump), jumpCooldown);
        }
    }

    private void MovePlayer()
    {
        //calc move direction
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
    }

    private void Jump()
    {
        //reset y velocity
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    private void ResetJump()
    {
        readyToJump = true;
        jumpIsCooldowned = false;
    }

    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        //Limit if higher than allowed speed
        if(flatVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<Surface>() != null)
        {
            surfaces.Add(collision.gameObject.GetComponent<Surface>());
            SurfaceUpdate();
            rb.drag = drag;
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.GetComponent<Surface>() != null)
        {
            surfaces.Remove(collision.gameObject.GetComponent<Surface>());
            SurfaceUpdate();
            rb.drag = drag;
        }
    }

    private void SurfaceUpdate()
    {
        if(surfaces.Count <= 0)
        {
            drag = defaultDrag;
            jumpableSurface = false;
            return;
        }

        jumpableSurface = false;

        foreach (Surface s in surfaces)
        {
            if (s.drag > drag)
            {
                drag = s.drag;
            }
            if(s.jumpable)
            {
                jumpableSurface = true;
            }
        }

        if(!jumpIsCooldowned && jumpableSurface)
        {
            ResetJump();
        }
    }
}
