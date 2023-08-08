using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    // List of available directions from this node
    public List<Vector2> availableDirections { get; private set; }

    // Layer mask for obstacle detection
    public LayerMask obstacleLayer;

    // Determines if movement upwards is allowed from this node
    public bool allowUp = true;

    // Start is called before the first frame update
    void Start()
    {
        // Initialize the list of available directions
        this.availableDirections = new List<Vector2>();

        // Check and add available directions based on the 'allowUp' setting
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
        // This method doesn't perform any actions in the update
    }

    // Check and add an available direction if there is no obstacle in that direction
    private void CheckAvailableDirection(Vector2 direction)
    {
        // Perform a box cast from the node's position in the given direction to check for obstacles
        RaycastHit2D hit = Physics2D.BoxCast(this.transform.position, Vector2.one * 0.5f, 0.0f, direction, 1.0f, this.obstacleLayer);

        // If the box cast hits no collider, the direction is considered available
        if (hit.collider == null)
        {
            this.availableDirections.Add(direction);
        }
    }
}
