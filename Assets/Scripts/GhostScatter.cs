using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostScatter : GhostBehavior
{
    // Variable to keep track of how many times the ghost has looped back to the scatter behavior
    public int loop = 0;

    // Called when the ghost scatter behavior is disabled
    private void OnDisable()
    {
        // Increment the loop counter
        this.loop++;

        // If the ghost has looped back to the scatter behavior for the fourth time,
        // set the duration to 0 and reverse the ghost's movement direction
        if (this.loop >= 4)
        {
            this.duration = 0.0f;
            this.ghost.movement.SetDirection(-this.ghost.movement.direction);
        }

        // Enable the ghost's chase behavior
        this.ghost.chase.Enable();
    }

    // Called when drawing gizmos in the editor for debugging purposes
    private void OnDrawGizmos()
    {
        // If the scatter behavior is enabled and the target is not null, draw a white line from the ghost to the target position
        if (this.enabled && this.target != null)
        {
            Gizmos.color = Color.white;
            Gizmos.DrawLine(transform.position, target.position);
        }
    }
}
