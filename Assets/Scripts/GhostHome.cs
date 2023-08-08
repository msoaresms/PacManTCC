using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostHome : GhostBehavior
{
    // Transforms representing the positions inside and outside the ghost home
    public Transform insideTransform;
    public Transform outsideTransform;

    // Reference to the GameManager script
    public GameManager gameManager;

    // Start is called before the first frame update
    public void Start()
    {
        // Start the coroutine to make the ghost leave the house
        StartCoroutine(LeaveHouse());
    }

    // Coroutine for making the ghost leave the house
    private IEnumerator LeaveHouse()
    {
        // Get a reference to the Inky ghost
        Ghost inky = this.gameManager.ghosts[2];

        // Get the name of this ghost
        string ghostName = GetComponent<Ghost>().name;

        // Flag to track if the ghost has left the house
        bool leaved = false;

        while (!leaved)
        {
            // If the ghost is Pinky and the time has passed, disable the ghost home behavior
            if (this.enabled && ghostName == "Pinky")
            {
                yield return new WaitForSeconds(0.75f);
                this.enabled = false;
                leaved = true;
            }
            // If the ghost is Inky and the time has passed or a certain number of pellets have been eaten,
            // disable the ghost home behavior and reset the intervalPellet counter
            else if (this.enabled && ghostName == "Inky" && (this.gameManager.intervalPellet > 4.0f || this.gameManager.pelletsEaten > 30))
            {
                this.enabled = false;
                leaved = true;
                this.gameManager.intervalPellet = 0.0f;
            }
            // If the ghost is Clyde and the time has passed or a certain number of pellets have been eaten,
            // and Inky has left the house, disable the ghost home behavior and reset the intervalPellet counter
            else if (this.enabled && ghostName == "Clyde" && (this.gameManager.intervalPellet > 4.0f || this.gameManager.pelletsEaten > 90) && !inky.home.enabled)
            {
                this.enabled = false;
                leaved = true;
                this.gameManager.intervalPellet = 0.0f;
            }

            yield return null;
        }
    }

    // Called when the ghost home behavior is disabled
    private void OnDisable()
    {
        // If the GameObject is still active, start the coroutine for the exit transition
        if (this.gameObject.activeSelf)
        {
            StartCoroutine(ExitTransition());
        }
    }

    // Coroutine for the exit transition when the ghost leaves the home
    public IEnumerator ExitTransition()
    {
        // Set the ghost's movement direction to up and freeze its rigidbody
        this.ghost.movement.SetDirection(Vector2.up, true);
        this.ghost.movement.rigidbody.isKinematic = true;
        this.ghost.movement.enabled = false;

        // Get the current position of the ghost
        Vector3 position = transform.position;

        // Define the duration of the exit transition
        float duration = 0.5f;
        float elapsed = 0.0f;

        // Move the ghost from inside the home to the outside position
        while (elapsed < duration)
        {
            Vector3 newPosition = Vector3.Lerp(position, this.insideTransform.position, elapsed / duration);
            newPosition.z = position.z;
            this.ghost.transform.position = newPosition;
            elapsed += Time.deltaTime;
            yield return null;
        }

        elapsed = 0.0f;

        // Move the ghost from the inside position to the outside position
        while (elapsed < duration)
        {
            Vector3 newPosition = Vector3.Lerp(this.insideTransform.position, this.outsideTransform.position, elapsed / duration);
            newPosition.z = position.z;
            this.ghost.transform.position = newPosition;
            elapsed += Time.deltaTime;
            yield return null;
        }

        // Set the ghost's movement direction to left, unfreeze its rigidbody, and enable its movement
        this.ghost.movement.SetDirection(Vector2.left, true);
        this.ghost.movement.rigidbody.isKinematic = false;
        this.ghost.movement.enabled = true;
    }

    // Called when the ghost collides with an obstacle
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // If the ghost home behavior is enabled and the ghost collides with an obstacle, change its movement direction
        if (this.enabled && collision.gameObject.layer == LayerMask.NameToLayer("Obstacle"))
        {
            this.ghost.movement.SetDirection(-this.ghost.movement.direction);
        }
    }

    // Called when the ghost home behavior is enabled
    private void OnEnable()
    {
        // Stop all coroutines to prevent any previous coroutine from affecting the current one
        StopAllCoroutines();
    }
}
