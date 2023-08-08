using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostFrightened : GhostBehavior
{
    // References to the SpriteRenderers for the ghost's body, eyes, blue sprite, and white sprite
    public SpriteRenderer body;
    public SpriteRenderer eyes;
    public SpriteRenderer blue;
    public SpriteRenderer white;

    // Flag to track if the ghost has been eaten by Pacman
    public bool eaten { get; private set; }

    // Override the Enable method to enable the frightened behavior
    public override void Enable()
    {
        // Call the base class's Enable method to activate the behavior
        base.Enable();

        // Disable the body and eyes sprites, and enable the blue sprite
        this.body.enabled = false;
        this.eyes.enabled = false;
        this.blue.enabled = true;
        this.white.enabled = false;

        // Schedule the Flash method to be called after half of the frightened duration
        Invoke(nameof(Flash), this.duration / 2.0f);
    }

    // Override the Disable method to disable the frightened behavior
    public override void Disable()
    {
        // Call the base class's Disable method to deactivate the behavior
        base.Disable();

        // Enable the body and eyes sprites, and disable the blue and white sprites
        this.body.enabled = true;
        this.eyes.enabled = true;
        this.blue.enabled = false;
        this.white.enabled = false;
    }

    // Flash the white sprite on and off
    private void Flash()
    {
        // If the ghost hasn't been eaten yet, toggle between blue and white sprites
        if (!this.eaten)
        {
            this.blue.enabled = false;
            this.white.enabled = true;
            this.white.GetComponent<AnimatedSprite>().Restart();
        }
    }

    // Called when the frightened behavior is enabled
    private void OnEnable()
    {
        // Reduce the ghost's movement speed when frightened
        this.ghost.movement.speedMultiplier = 0.5f;

        // Reset the eaten flag to false, and start the coroutine to disable the behavior after the duration
        this.eaten = false;
        StartCoroutine(DisableState());
    }

    // Coroutine to disable the frightened behavior after a specified duration
    private IEnumerator DisableState()
    {
        float duration = this.duration;
        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            yield return null;
        }

        // The frightened behavior has reached its duration, so disable the behavior
        this.Disable();
    }

    // Called when the frightened behavior is disabled
    private void OnDisable()
    {
        // Restore the ghost's movement speed to normal and reset the eaten flag to false
        this.ghost.movement.speedMultiplier = 1.0f;
        this.eaten = false;
    }

    // Called when the ghost is eaten by Pacman
    private void Eaten()
    {
        // Set the eaten flag to true and move the ghost to its home position
        this.eaten = true;
        Vector3 position = this.ghost.home.insideTransform.position;
        position.z = this.ghost.transform.position.z;
        this.ghost.transform.position = position;

        // Enable the ghost's home behavior
        this.ghost.home.Enable();

        // Disable the body sprite, enable the eyes sprite, and disable the blue and white sprites
        this.body.enabled = false;
        this.eyes.enabled = true;
        this.blue.enabled = false;
        this.white.enabled = false;
    }

    // Called when the ghost collides with Pacman
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // If the frightened behavior is enabled and the ghost collides with Pacman, call the Eaten method
        if (collision.gameObject.layer == LayerMask.NameToLayer("Pacman"))
        {
            if (this.enabled)
            {
                Eaten();
            }
        }
    }

    // Called when the ghost enters a trigger collider (used for movement)
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Get the Node component from the trigger object (if available)
        Node node = other.GetComponent<Node>();

        // If the Node component is available and the frightened behavior is enabled, update the ghost's movement direction
        if (node != null && this.enabled)
        {
            // Randomly choose a new available direction for the ghost's movement at the current node
            int index = Random.Range(0, node.availableDirections.Count);

            // If there are multiple available directions and the chosen direction is opposite to the current direction,
            // choose the next available direction in the list
            if (node.availableDirections.Count > 1 && node.availableDirections[index] == -this.ghost.movement.direction)
            {
                index++;

                if (index >= node.availableDirections.Count)
                {
                    index = 0;
                }
            }

            // Set the ghost's movement direction to the chosen direction
            this.ghost.movement.SetDirection(node.availableDirections[index]);
        }
    }
}
