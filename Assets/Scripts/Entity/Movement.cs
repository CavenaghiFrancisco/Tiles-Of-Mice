using System.Collections;
using System.Collections.Generic;
using TOM;
using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    [Header("Movement Configuration")]
    [SerializeField] float speed = 5f;

    [Header("Dash Configuration")]
    [SerializeField] private float dashSpeed = 20f;
    [SerializeField] private float dashTimer = 0f;
    [SerializeField] private float dashMaxCooldown = 1f;
    [SerializeField] private float dashDuration = 0.2f;
    private float dashCooldown = 1f;
    public bool IsDashing { get => isDashing; }
    public bool canDash = true;

    private InputAction moveAction;
    private Controls controls;
    private Camera mainCamera;
    private bool isDashing = false;
    private bool isDashInProgress = false;
    private Vector3 dashDirection;
    private Rigidbody rb;

    private Animator animator;

    [Header("Particles Configuration")]
    [SerializeField] private ParticleSystem dustParticle;
    [Header("Particles Configuration")]
    [SerializeField] private TrailRenderer trail;
    [SerializeField] private TrailRenderer trail1;

    private void Awake()
    {
        controls = new Controls();

        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();

        moveAction = controls.Movement.Move;
        mainCamera = Camera.main;
        canDash = true;

        controls.Movement.Dash.performed += context =>
        {
            if (!isDashing && dashCooldown >= dashMaxCooldown && canDash)
            {
                StartDash();
                dashCooldown = 0;
            }
        };
    }

    private void FixedUpdate()
    {
        if (!GameManager.IsPaused)
        {
            animator.SetBool("isRunning", Move()); //Uso Move() para settear el valor del animator, ya se que no es performante pero para salir del paso
                                                   //animator.SetBool("isDashing", isDashInProgress); Lo mismo de arriba
            if (dashCooldown < 1)
            {
                dashCooldown += Time.deltaTime;
                if (dashCooldown > 1)
                {
                    dashCooldown = 1;
                }
            }

            if (isDashInProgress)
            {
                DashMovement();
            }
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
        bool isMoving = !input.Equals(Vector2.zero);
        if (isMoving)
        {
            if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Br1e_Run"))
            {
                animator.Play("Br1e_Run");
            }
            if (!dustParticle.isEmitting)
            {
                dustParticle.Play();
            }
            //ps.gameObject.SetActive(true);
        }
        else
        {
            if (!isDashInProgress)//Cuando metamos combate se va a romper jaja
            {
                animator.Play("Br1e_Idle");
            }
            if (dustParticle.isPlaying)
            {
                dustParticle.Stop(true, ParticleSystemStopBehavior.StopEmitting);
            }
            //ps.gameObject.SetActive(false);
        }

        return isMoving;
    }

    private void StartDash()
    {
        isDashing = true;
        isDashInProgress = true;
        dashDirection = transform.forward;
        dashDirection.Normalize();
        dashTimer = 0f;
        animator.SetBool("isDashing", true);
        animator.Play("Br1e_Dash");
        StartCoroutine(RunVFX(trail, 1));
        StartCoroutine(RunVFX(trail1, 1));
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

    private IEnumerator RunVFX(TrailRenderer vfx, float seconds)
    {
        vfx.Clear();
        vfx.enabled = true;
        yield return new WaitForSeconds(seconds - 0.2f);
        vfx.enabled = false;
        yield return null;
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

    public void StopAllMovement()
    {
        isDashInProgress = false;
        isDashing = false;
        rb.velocity = Vector3.zero;
        animator.SetBool("isDashing", false);
        animator.SetBool("isRunning", false);
        trail.emitting = false;
        trail1.emitting = false;
        animator.Play("Br1e_Idle");
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
