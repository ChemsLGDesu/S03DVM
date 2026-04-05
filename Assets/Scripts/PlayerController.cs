using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public InputSystem_Actions inputs;
    private CharacterController controller;

    public float walkSpeed = 5.00f;
    public float runSpeed = 9.00f;
    public float rotationSpeed = 200f;
    private float currentSpeed;

    public float moveSpeed = 5f;
    public float verticalVelocity = 0;
    public float jumpForce = 10;
    public float pushForce = 4;

    public float dashForce = 20.00f;
    public float dashDuration = 0.2f;
    private float dashTimer;
    private bool IsDashing;



    [SerializeField]private Vector2 moveInput;
    private bool isRunning;

    private void Awake()
    {
        inputs = new();
        controller = GetComponent<CharacterController>();
    }
    private void OnEnable()
    {
        inputs.Enable();

        inputs.Player.Move.performed += ctx =>  moveInput = ctx.ReadValue<Vector2>();
        inputs.Player.Move.canceled += ctx => moveInput = Vector2.zero;


        inputs.Player.Jump.performed += OnJump;

        inputs.Player.Sprint.performed += ctx => isRunning = true;
        inputs.Player.Sprint.canceled += ctx => isRunning = false;

        inputs.Player.Sprint.performed += OnDash;

    }
    void Start()
    {

    }
    void Update()
    {

         OnMove();
        //OnSimpleMove();
    }

    private void OnDisable() => inputs.Disable();
    public void OnMove()
    {
        
        transform.Rotate(Vector3.up * moveInput.x * rotationSpeed * Time.deltaTime);

        
        currentSpeed = isRunning ? runSpeed : walkSpeed;
        Vector3 moveDir = transform.forward * currentSpeed * moveInput.y;

        
        if (controller.isGrounded && verticalVelocity < 0)
            verticalVelocity = -2.00f;

        verticalVelocity += Physics.gravity.y * Time.deltaTime;
        moveDir.y = verticalVelocity;

        
        if (IsDashing)
        {
            
            float dashMultiplier = dashTimer / dashDuration;
            Vector3 dashMove = transform.forward * dashForce * dashMultiplier;

            moveDir.x = dashMove.x;
            moveDir.z = dashMove.z;

            dashTimer -= Time.deltaTime;
            if (dashTimer <= 0) IsDashing = false;
        }

        
        controller.Move(moveDir * Time.deltaTime);
    }

    private void OnJump(InputAction.CallbackContext context)
    {
        if (controller.isGrounded)
            verticalVelocity = jumpForce;
    }
    public void OnSimpleMove()
    {
        transform.Rotate(Vector3.up * moveInput.x * rotationSpeed * Time.deltaTime);
        Vector3 moveDir = transform.forward * moveSpeed * moveInput.y ;
        controller.SimpleMove(moveDir);
    }
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Vector3 pushDir = (hit.transform.position - transform.position).normalized;

        if (hit.rigidbody != null && hit.rigidbody.linearVelocity == Vector3.zero)
        {
            print(hit.gameObject.name);
            hit.rigidbody.AddForce(pushDir * pushForce, ForceMode.Impulse);
        }
    }
    private void OnDash(InputAction.CallbackContext context)
    {
        if (!IsDashing)
        {
            IsDashing = true;
            dashTimer = dashDuration;
        }
    }

    private void OnDrawGizmos()
    {
        if (controller == null) return;

        // Dirección del movimiento (Azul)
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(transform.position + Vector3.up, transform.forward * 2.00f);

        // Velocidad Vertical (Verde)
        Gizmos.color = Color.green;
        // Dibujamos una línea que sube o baja según la velocidad vertical actual
        Gizmos.DrawLine(transform.position + Vector3.up, transform.position + Vector3.up + (Vector3.up * verticalVelocity * 0.50f));
    }
}
