using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    public List<Vector2> availableDirections { get; private set; }

    public LayerMask obstacleLayer;

    public bool allowUp = true;

    // Start is called before the first frame update
    void Start()
    {
        this.availableDirections = new List<Vector2>();

        if (this.allowUp)
        {
            CheckAvailableDirection(Vector2.up);
        }

        CheckAvailableDirection(Vector2.down);
        CheckAvailableDirection(Vector2.left);
        CheckAvailableDirection(Vector2.right);
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void CheckAvailableDirection(Vector2 direction)
    {
        RaycastHit2D hit =
            Physics2D
                .BoxCast(this.transform.position,
                Vector2.one * 0.5f,
                0.0f,
                direction,
                1.0f,
                this.obstacleLayer);

        if (hit.collider == null)
        {
            this.availableDirections.Add(direction);
        }
    }
}
