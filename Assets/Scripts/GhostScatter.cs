using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostScatter : GhostBehavior
{
    public int loop = 0;

    private void OnDisable()
    {
        this.loop++;
        if (this.loop >= 4)
        {
            this.duration = 0.0f;
            this.ghost.movement.SetDirection(-this.ghost.movement.direction);
        }
        this.ghost.chase.Enable();
    }

    private void OnDrawGizmos()
    {
        if (this.enabled && this.target != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(transform.position, target.position);
        }
    }
}
