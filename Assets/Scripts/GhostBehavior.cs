using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GhostBehavior : MonoBehaviour
{
    public Ghost ghost { get; private set; }

    public Transform target;

    public float duration;

    private void Awake()
    {
        this.ghost = GetComponent<Ghost>();
        this.enabled = false;
    }

    public void Enable()
    {
        Enable(this.duration);
    }

    public virtual void Enable(float duration)
    {
        this.enabled = true;
        CancelInvoke();
        Invoke(nameof(Disable), this.duration);
    }

    public virtual void Disable()
    {
        this.enabled = false;
        CancelInvoke();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {

        if (LayerMask.LayerToName(other.gameObject.layer) == "Speed" && !this.ghost.frightened.enabled)
        {
            this.ghost.movement.speedMultiplier = 0.5f;
        }
        else
        {
            this.ghost.movement.speedMultiplier = 1.0f;
        }

        Node node = other.GetComponent<Node>();

        if (node != null && this.enabled && !this.ghost.frightened.enabled)
        {
            Vector2 direction = Vector2.zero;
            float minDistance = float.MaxValue;

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

            this.ghost.movement.SetDirection(direction);
        }
    }


}
