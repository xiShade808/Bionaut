using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
Ok David, this is the movement script/character controller for the main character.
The movement is meant to be as realistic and smooth as possible, that's why there is an acceleration and a drag/deceleration period before and after the character
reaches maximum speed.

There is also the jump part, which works by detecting whether or not the player is grounded (interacting with the ground layer) or if he is pressing the jump button
*/

public class PlayerMovement : MonoBehaviour
{
    //public Controller controller;
    [SerializeField] public float maxMoveSpeed;
    [SerializeField] public float movementAcceleration;
    [SerializeField] public float linearDrag;
    [SerializeField] public float jumpForce;
    private Rigidbody2D rb;
    private float horizontalDirection;
    private bool changingDirection => (rb.velocity.x > 0f && horizontalDirection < 0f) || (rb.velocity.x < 0f && horizontalDirection > 0f);

    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float groundRaycastLength;
    private bool onGround;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    void Update()
    {
        horizontalDirection = GetInput().x;
    }
    private Vector2 GetInput()
    {
        return new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
    }
    
    private void MoveCharacter()
    {
        rb.AddForce(new Vector2(horizontalDirection, 0f) * movementAcceleration);

        if (Mathf.Abs(rb.velocity.x) > maxMoveSpeed) 
            rb.velocity = new Vector2(Mathf.Sign(rb.velocity.x) * maxMoveSpeed, rb.velocity.y);
    }

    private void ApplyLinearDrag()
    {
        if (Mathf.Abs(horizontalDirection) < 0.4f || changingDirection)
        {
            rb.drag = linearDrag;
        } 
        else
        {
            rb.drag = 0f;
        }
    }

    void FixedUpdate()
    {
        CheckCollisions();
        MoveCharacter();
        ApplyLinearDrag();
        if (Input.GetButtonDown("Jump") && onGround) 
            Jump();
    }

    void Jump()
    {
            rb.velocity = new Vector2(rb.velocity.x, 0f);
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }

    private void CheckCollisions()
    {
        onGround = Physics2D.Raycast(transform.position * groundRaycastLength, Vector2.down, groundRaycastLength, groundLayer);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * groundRaycastLength);
    }
}
