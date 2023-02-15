using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostScatter : GhostBehavior
{

    public Transform target;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
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

    private void OnDisable()
    {
        this.ghost.chase.Enable();
    }
}
