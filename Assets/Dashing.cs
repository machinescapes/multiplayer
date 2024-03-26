using UnityEngine;
using Photon.Pun;

public class Dashing : MonoBehaviourPunCallbacks
{
    [Header("References")]
    private Rigidbody rb;
    private Movement movementScript;

    [Header("Dashing")]
    public float dashForce;
    public float maxDashYSpeed;
    public float dashDuration;

    [Header("Settings")]
    public bool useCameraForward = true;
    public bool allowAllDirections = true;
    public bool disableGravity = false;
    public bool resetVel = true;

    [Header("Cooldown")]
    public float dashCd;
    private float dashCdTimer;

    [Header("Input")]
    public KeyCode dashKey = KeyCode.E;

    private bool dashing = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        movementScript = GetComponent<Movement>();
    }

    private void Update()
    {
        if (photonView.IsMine && Input.GetKeyDown(dashKey)) // Check if this is the local player
            Dash();

        if (dashCdTimer > 0)
            dashCdTimer -= Time.deltaTime;
    }

    [PunRPC]
    private void Dash()
    {
        if (dashCdTimer > 0 || dashing) return;
        else dashCdTimer = dashCd;

        dashing = true;

        Vector3 direction = GetDirection();

        // Calculate the upward component based on camera rotation
        float upwardComponent = Mathf.Clamp(Vector3.Dot(Camera.main.transform.forward, Vector3.up), 0f, 1f);

        Vector3 forceToApply = direction * dashForce + Vector3.up * upwardComponent * maxDashYSpeed;

        if (disableGravity)
            rb.useGravity = false;

        rb.velocity = Vector3.zero; // Reset velocity

        rb.AddForce(forceToApply, ForceMode.Impulse);

        Invoke(nameof(ResetDash), dashDuration);
    }

    private void ResetDash()
    {
        dashing = false;

        if (disableGravity)
            rb.useGravity = true;
    }

    private Vector3 GetDirection()
    {
        // Get the forward direction of the player's transform
        Vector3 forwardDirection = transform.forward;

        // Zero out the vertical component of the forward direction
        forwardDirection.y = 0f;

        // Normalize the direction to ensure consistent speed
        return forwardDirection.normalized;
    }
}
