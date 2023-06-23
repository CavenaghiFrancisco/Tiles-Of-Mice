using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    [Header("Movement Configuration")]
    [SerializeField] float speed = 5f;

    [Header("Dash Configuration")]
    [SerializeField] private float dashSpeed = 20f;
    [SerializeField] private float dashTimer = 0f;
    [SerializeField] private float dashDuration = 0.2f;

    private InputAction moveAction;
    private InputAction dashAction;
    private Controls controls;
    private Camera mainCamera;
    private bool isDashing = false;
    private bool isDashInProgress = false;
    private Vector3 dashDirection;
    private Rigidbody rb;

    private Animator animator;

    private void Awake()
    {
        controls = new Controls();
        rb = GetComponent<Rigidbody>();
        moveAction = controls.Movement.Move;
        dashAction = controls.Movement.Dash;

        animator = GetComponent<Animator>();

        mainCamera = Camera.main;

        controls.Movement.Dash.performed += context =>
        {
            if (!isDashing)
            {
                StartDash();
            }
        };
    }

    private void FixedUpdate()
    {
        animator.SetBool("isRunning", Move()); //Uso Move() para settear el valor del animator, ya se que no es performante pero para salir del paso
        //animator.SetBool("isDashing", isDashInProgress); Lo mismo de arriba

        if (isDashInProgress)
        {
            DashMovement();
        }
    }

    private bool Move()
    {
        Vector2 input = moveAction.ReadValue<Vector2>();
        Vector3 camForward = Camera.main.transform.forward;
        Vector3 camRight = Camera.main.transform.right;
        camForward.y = 0f;
        camRight.y = 0f;
        rb.MovePosition(transform.position + (camForward * input.y + camRight * input.x).normalized * Time.fixedDeltaTime * speed);
        transform.LookAt(transform.position + (camForward * input.y + camRight * input.x).normalized);
        return !input.Equals(Vector2.zero);
    }

    private void StartDash()
    {
        isDashing = true;
        isDashInProgress = true;
        dashDirection = transform.forward;
        dashDirection.Normalize();
        dashTimer = 0f;
        animator.SetBool("isDashing", true);
    }

    private void DashMovement()
    {
        dashTimer += Time.fixedDeltaTime;
        if (dashTimer <= dashDuration)
        {
            float currentDashSpeed = Mathf.Lerp(dashSpeed, 0f, dashTimer / dashDuration);
            rb.velocity = dashDirection * currentDashSpeed;
        }
        else
        {
            isDashInProgress = false;
            isDashing = false;
            rb.velocity = Vector3.zero;
            animator.SetBool("isDashing", false);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (isDashing)
        {
            rb.velocity = Vector3.zero;
            isDashing = false;
            animator.SetBool("isDashing", false);
        }
    }

    private void OnEnable()
    {
        controls.Movement.Enable();
    }

    private void OnDisable()
    {
        controls.Movement.Disable();
    }
}
