using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GhostBehavior : MonoBehaviour
{
    // Reference to the Ghost script of the associated ghost GameObject
    public Ghost ghost { get; private set; }

    // Transform representing the target destination for the ghost's movement
    public Transform target;

    // Duration for which the behavior should be active before disabling
    public float duration;

    // Called when the GameObject is created in the scene
    private void Awake()
    {
        // Get a reference to the Ghost script attached to the GameObject
        this.ghost = GetComponent<Ghost>();

        // Initially, disable this behavior script
        this.enabled = false;
    }

    // Enable the ghost behavior
    public virtual void Enable()
    {
        // Enable this behavior script
        this.enabled = true;

        // Cancel any previously scheduled Invoke calls
        CancelInvoke();

        // Set the ghost's movement direction opposite to its current direction
        this.ghost.movement.SetDirection(-this.ghost.movement.direction);

        // If the ghost is not in the home behavior, start a coroutine to disable the behavior after a specified duration
        if (!this.ghost.home.enabled)
        {
            StartCoroutine(DisableState());
        }
    }

    // Coroutine to disable the behavior after a specified duration
    private IEnumerator DisableState()
    {
        float duration = this.duration;
        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            // If the ghost is not frightened, increment the elapsed time
            if (!this.ghost.frightened.enabled)
            {
                elapsed += Time.deltaTime;
            }
            yield return null;
        }

        // Set the ghost's movement direction opposite to its current direction and disable the behavior
        this.ghost.movement.SetDirection(-this.ghost.movement.direction);
        this.Disable();
    }

    // Disable the ghost behavior
    public virtual void Disable()
    {
        // Disable this behavior script
        this.enabled = false;

        // Cancel any previously scheduled Invoke calls
        CancelInvoke();
    }

    // Called when the ghost collides with a trigger collider (used for movement)
    private void OnTriggerEnter2D(Collider2D other)
    {
        // If the ghost is not frightened, adjust its movement speed based on the layer of the trigger object
        if (!this.ghost.frightened.enabled)
        {
            if (LayerMask.LayerToName(other.gameObject.layer) == "Speed")
            {
                this.ghost.movement.speedMultiplier = 0.5f;
            }
            else
            {
                this.ghost.movement.speedMultiplier = 1.0f;
            }
        }

        // Get the Node component from the trigger object (if available) and adjust the ghost's movement direction
        Node node = other.GetComponent<Node>();

        if (node != null && this.enabled && !this.ghost.frightened.enabled)
        {
            Vector2 direction = Vector2.zero;
            float minDistance = float.MaxValue;

            // Iterate through the available directions at the node and choose the direction that leads to the target
            foreach (Vector2 availableDirection in node.availableDirections)
            {
                if (availableDirection != -this.ghost.movement.direction)
                {
                    Vector3 newPosition =
                        this.transform.position +
                        new Vector3(availableDirection.x,
                            availableDirection.y,
                            0.0f);
                    float distance =
                        (this.target.position - newPosition).sqrMagnitude;

                    if (distance < minDistance)
                    {
                        direction = availableDirection;
                        minDistance = distance;
                    }
                }
            }

            // Set the ghost's movement direction based on the chosen direction
            this.ghost.movement.SetDirection(direction);
        }
    }
}
