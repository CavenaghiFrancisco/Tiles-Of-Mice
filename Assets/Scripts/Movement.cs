using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    [Header("Movement Configuration")]
    [SerializeField] float speed = 5f;

    [Header("Dash Configuration")]
    [SerializeField] float dashDistance = 5f;
    [SerializeField] float dashDuration = 0.2f;
    [SerializeField] float dashCooldown = 1f;

    private InputAction moveAction;
    private InputAction dashAction;
    private Controls controls;
    private Camera mainCamera;
    private bool isDashing = false;

    private void Awake()
    {
        controls = new Controls();

        moveAction = controls.Movement.Move;
        dashAction = controls.Movement.Dash;

        mainCamera = Camera.main;

        controls.Movement.Dash.performed += context =>
        {
            if (!isDashing)
            {
                StartCoroutine(Dash());
            }
        };
    }

    private void Update()
    {
        Move();
    }

    private void Move()
    {
        Vector2 input = moveAction.ReadValue<Vector2>();
        Vector3 camForward = Camera.main.transform.forward;
        Vector3 camRight = Camera.main.transform.right;
        camForward.y = 0f;
        camRight.y = 0f;
        transform.position += (camForward * input.y + camRight * input.x).normalized * Time.deltaTime * speed;
        transform.LookAt(transform.position + (camForward * input.y + camRight * input.x).normalized);
    }

    private System.Collections.IEnumerator Dash()
    {
        isDashing = true;

        Vector3 dashVector = transform.forward;
        dashVector.Normalize();

        float dashEndTime = Time.time + dashDuration;
        Vector3 startPosition = transform.position;
        Vector3 targetPosition = startPosition + dashVector * dashDistance;

        while (Time.time < dashEndTime)
        {
            float t = (dashEndTime - Time.time) / dashDuration;
            transform.position = Vector3.Lerp(targetPosition, startPosition, t);
            yield return null;
        }

        yield return new WaitForSeconds(dashCooldown);
        isDashing = false;
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