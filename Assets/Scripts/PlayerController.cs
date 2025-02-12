using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private Vector2 _moveInput;
    private bool _isGrounded;
    
    [Header("References")]
    [SerializeField] private Rigidbody rb;
    
    [Header("Forces")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 10f;

    [Header("Ground Check")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundRadius = 0.5f;
    [SerializeField] private LayerMask groundLayer;

    private void Update()
    {
        _isGrounded = Physics.CheckSphere(groundCheck.position, groundRadius, groundLayer);
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = new Vector3(_moveInput.x * moveSpeed, rb.linearVelocity.y, rb.linearVelocity.z);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        _moveInput = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.started && _isGrounded)
        {
            // Jump
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, jumpForce, rb.linearVelocity.z);
            Debug.Log("Jumping!");
        }else if (context.canceled && rb.linearVelocity.y > 0)
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, rb.linearVelocity.y * 0.5f, rb.linearVelocity.z);
        }
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(groundCheck.position, groundRadius);
    }
}
