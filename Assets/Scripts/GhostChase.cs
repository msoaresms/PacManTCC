using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostChase : GhostBehavior
{
    // Reference to the PacMan script
    public PacMan pacman;

    // GameObject representing Blinky
    private GameObject blinky;

    // GameObject representing the auxiliary target for Inky
    private GameObject inkyAuxTarget;

    // GameObject representing the scatter target for Clyde
    private GameObject clydeScatterTarget;

    // Called when this behavior is disabled (when switching to another behavior)
    private void OnDisable()
    {
        // Enable the Scatter behavior when this behavior is disabled
        this.ghost.scatter.Enable();
    }

    // Start is called before the first frame update
    private void Start()
    {
        // Find the GameObjects representing Blinky, the auxiliary target for Inky, and the scatter target for Clyde
        this.blinky = GameObject.Find("Blinky");
        this.inkyAuxTarget = GameObject.Find("InkyAuxChaseTarget");
        this.clydeScatterTarget = GameObject.Find("ClydeScatterTarget");
    }

    // Update is called once per frame
    private void Update()
    {
        // Get the name of the ghost associated with this behavior
        string ghostName = GetComponent<Ghost>().name;

        // Update the target position based on the type of ghost
        if (ghostName == "Blinky")
        {
            this.UpdateBlinkyTarget();
        }
        else if (ghostName == "Pinky")
        {
            this.UpdatePinkyTarget();
        }
        else if (ghostName == "Inky")
        {
            this.UpdateInkyTarget();
        }
        else if (ghostName == "Clyde")
        {
            this.UpdateClydeTarget();
        }
    }

    // Update the target position for Blinky (directly chase Pacman)
    private void UpdateBlinkyTarget()
    {
        this.target.position = this.pacman.transform.position;
    }

    // Update the target position for Pinky (chase a position ahead of Pacman)
    private void UpdatePinkyTarget()
    {
        Vector2 pacmanDirection = this.pacman.movement.direction;
        Vector2 pacmanPosition = this.pacman.transform.position;

        // Set the target position based on the direction of Pacman's movement
        if (pacmanDirection == Vector2.up)
        {
            this.target.position = new Vector3(pacmanPosition.x - 4.0f, pacmanPosition.y + 4.0f, 0.0f);
        }
        else if (pacmanDirection == Vector2.down)
        {
            this.target.position = new Vector3(pacmanPosition.x, pacmanPosition.y - 4.0f, 0.0f);
        }
        else if (pacmanDirection == Vector2.left)
        {
            this.target.position = new Vector3(pacmanPosition.x - 4.0f, pacmanPosition.y, 0.0f);
        }
        else if (pacmanDirection == Vector2.right)
        {
            this.target.position = new Vector3(pacmanPosition.x + 4.0f, pacmanPosition.y, 0.0f);
        }
    }

    // Update the target position for Inky (chase a position based on Blinky and an auxiliary target)
    private void UpdateInkyTarget()
    {
        Vector2 pacmanDirection = this.pacman.movement.direction;
        Vector2 pacmanPosition = this.pacman.transform.position;

        // Set the position of the auxiliary target based on Pacman's direction
        if (pacmanDirection == Vector2.up)
        {
            this.inkyAuxTarget.transform.position = new Vector3(pacmanPosition.x - 2.0f, pacmanPosition.y + 2.0f, 0.0f);
        }
        else if (pacmanDirection == Vector2.down)
        {
            this.inkyAuxTarget.transform.position = new Vector3(pacmanPosition.x, pacmanPosition.y - 2.0f, 0.0f);
        }
        else if (pacmanDirection == Vector2.left)
        {
            this.inkyAuxTarget.transform.position = new Vector3(pacmanPosition.x - 2.0f, pacmanPosition.y, 0.0f);
        }
        else if (pacmanDirection == Vector2.right)
        {
            this.inkyAuxTarget.transform.position = new Vector3(pacmanPosition.x + 2.0f, pacmanPosition.y, 0.0f);
        }

        // Set the target position as a linear interpolation between Blinky's position and the auxiliary target
        this.target.position = Vector3.LerpUnclamped(this.blinky.transform.position, this.inkyAuxTarget.transform.position, 2.0f);
    }

    // Update the target position for Clyde (chase Pacman if far, go to scatter target if close)
    public void UpdateClydeTarget()
    {
        // Check the distance between Clyde and Pacman
        if (Vector3.Distance(this.pacman.transform.position, this.transform.position) >= 8.0f)
        {
            // If far from Pacman, chase Pacman directly
            this.target.position = this.pacman.transform.position;
        }
        else
        {
            // If close to Pacman, go to the scatter target position
            this.target.position = this.clydeScatterTarget.transform.position;
        }
    }

    // Called when Gizmos should be drawn for debugging purposes
    private void OnDrawGizmos()
    {
        // Draw a red line from the ghost's position to its target position (for debugging)
        if (this.enabled && this.target != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, target.position);
        }
    }
}
