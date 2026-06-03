using UnityEngine;

public class WizardBookScript : MonoBehaviour
{
    public Transform target;

    [Header("Follow Settings")]
    public float smoothTime = 0.2f;
    public float followOffsetX = 0.6f;
    public float followOffsetY = 0.6f;

    [Header("Floating Bob")]
    public float bobHeight = 0.1f;
    public float bobSpeed = 3f;

    [Header("Idle Orbit")]
    public float orbitRadius = 0.8f;
    public float orbitSpeed = 2f;

    private Vector3 velocity = Vector3.zero;

    void Update()
    {
        if (target == null) return;

        bool isMoving =
            Mathf.Abs(Input.GetAxisRaw("Horizontal")) > 0 ||
            Mathf.Abs(Input.GetAxisRaw("Vertical")) > 0;

        Vector3 desiredPosition;

        if (isMoving)
        {
            // Stay top-left relative to facing direction
            float direction = Mathf.Sign(target.localScale.x);

            desiredPosition = target.position +
                new Vector3(-followOffsetX * direction, followOffsetY, 0f);
        }
        else
        {
            // Orbit while idle
            float angle = Time.time * orbitSpeed;

            float x = Mathf.Cos(angle) * orbitRadius;
            float y = Mathf.Sin(angle) * orbitRadius;

            desiredPosition = target.position + new Vector3(x, y, 0f);
        }

        // Add gentle bobbing
        float bob = Mathf.Sin(Time.time * bobSpeed) * bobHeight;
        desiredPosition.y += bob;

        // Smooth rubber-band movement
        transform.position = Vector3.SmoothDamp(
            transform.position,
            desiredPosition,
            ref velocity,
            smoothTime
        );
    }
}

