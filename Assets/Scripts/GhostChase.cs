using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostChase : GhostBehavior
{
    public PacMan pacman;

    private GameObject blinky;

    private GameObject inkyAuxTarget;

    private GameObject clydeScatterTarget;

    private void OnDisable()
    {
        this.ghost.scatter.Enable();
    }

    private void Start()
    {
        this.blinky = GameObject.Find("Blinky");
        this.inkyAuxTarget = GameObject.Find("InkyAuxChaseTarget");
        this.clydeScatterTarget = GameObject.Find("ClydeScatterTarget");
    }

    private void Update()
    {
        string ghostName = GetComponent<Ghost>().name;

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

    private void UpdateBlinkyTarget()
    {
        this.target.position = this.pacman.transform.position;
    }

    private void UpdatePinkyTarget()
    {
        Vector2 pacmanDirection = this.pacman.movement.direction;
        Vector2 pacmanPosition = this.pacman.transform.position;

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

    private void UpdateInkyTarget()
    {
        Vector2 pacmanDirection = this.pacman.movement.direction;
        Vector2 pacmanPosition = this.pacman.transform.position;

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

        this.target.position = Vector3.LerpUnclamped(this.blinky.transform.position, this.inkyAuxTarget.transform.position, 2.0f);
    }

    public void UpdateClydeTarget()
    {
        if (Vector3.Distance(this.pacman.transform.position, this.transform.position) >= 8.0f)
        {
            this.target.position = this.pacman.transform.position;
        }
        else
        {
            this.target.position = this.clydeScatterTarget.transform.position;
        }
    }

    private void OnDrawGizmos()
    {
        if (this.enabled && this.target != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, target.position);
        }
    }
}
