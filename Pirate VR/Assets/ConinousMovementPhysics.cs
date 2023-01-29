using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ConinousMovementPhysics : MonoBehaviour
{
    public float speed = 1f;
    public float turnSpeed = 60f;
    public float jumpHeight = 1.5f;

    public bool moveWhenGrounded = false;
    public bool jumpWithHand = true;

    public float minJumpWithHandSpeed = 2f;
    public float maxJumpWithHandSpeed = 7f;

    public InputActionProperty moveInputSource;
    public InputActionProperty turnInputSource;
    public InputActionProperty jumpInputSource;

    public Rigidbody rb;
    public Rigidbody leftHandRb;
    public Rigidbody rightHandRb;


    public CapsuleCollider bodyCollider;

    public Transform directionSource;
    public Transform turnSource;

    public LayerMask groundLayer;

    private Vector2 inputMoveAxis;
    private float jumpVelocity = 7f;
    private float inputTurnAxis;

    private void Update() 
    {
        inputMoveAxis = moveInputSource.action.ReadValue<Vector2>();
        inputTurnAxis = turnInputSource.action.ReadValue<Vector2>().x;

        bool jumpInput = jumpInputSource.action.WasPerformedThisFrame();

        if(jumpWithHand)
        {
            if(jumpInput && IsGrounded())
            {
                jumpVelocity = Mathf.Sqrt(2 *-Physics.gravity.y * jumpHeight);
                rb.velocity = Vector3.up * jumpVelocity;
            }
        }
        else
        {
            bool inputJumpPressed = jumpInputSource.action.IsPressed();

            float handSpeed = ((leftHandRb.velocity - rb.velocity).magnitude + (rightHandRb.velocity - rb.velocity).magnitude) / 2f;

            if(inputJumpPressed && IsGrounded() && handSpeed > minJumpWithHandSpeed)
            {
                rb.velocity = Vector3.up * Mathf.Clamp(handSpeed, minJumpWithHandSpeed, maxJumpWithHandSpeed);
            }
        }
    }

    private void FixedUpdate() 
    {
        if(!moveWhenGrounded || (moveWhenGrounded && IsGrounded()))
        {
            Quaternion rot = Quaternion.Euler(0, directionSource.eulerAngles.y, 0);
            Vector3 dir = rot * new Vector3(inputMoveAxis.x, 0, inputMoveAxis.y);

            Vector3 targetMovePosition = rb.position + dir * Time.fixedDeltaTime * speed;

            Vector3 axis = Vector3.up;
            float angle = turnSpeed * Time.fixedDeltaTime * inputTurnAxis;

            Quaternion q = Quaternion.AngleAxis(angle, axis);

            rb.MoveRotation(rb.rotation * q);

            Vector3 newPos = q * (targetMovePosition - turnSource.position) + turnSource.position;

            rb.MovePosition(newPos);
        }
        else
        {
            Vector3 axis = Vector3.up;
            float angle = turnSpeed * Time.fixedDeltaTime * inputTurnAxis;

            Quaternion q = Quaternion.AngleAxis(angle, axis);

            rb.MoveRotation(rb.rotation * q);

            Vector3 newPos = q * (rb.position - turnSource.position) + turnSource.position;

            rb.MovePosition(newPos);
        }

    }

    public bool IsGrounded()
    {
        Vector3 start = bodyCollider.transform.TransformPoint(bodyCollider.center);
        float rayLength = bodyCollider.height/2 - bodyCollider.radius + 0.05f;

        return Physics.SphereCast(start, bodyCollider.radius, Vector3.down, out RaycastHit hitInfo, rayLength, groundLayer);
    }

}
