using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This script requires a SpriteRenderer component to be attached to the same GameObject
[RequireComponent(typeof(SpriteRenderer))]
public class GhostEyes : MonoBehaviour
{
    // Sprites for each eye direction (up, down, left, right)
    public Sprite up;
    public Sprite down;
    public Sprite left;
    public Sprite right;

    // Reference to the SpriteRenderer component attached to this GameObject
    public SpriteRenderer spriteRenderer { get; private set; }

    // Reference to the Movement component attached to the parent GameObject
    public Movement movement { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        // Initialization logic can be added here if needed
        // (Currently, the Start method is empty)
    }

    // Update is called once per frame
    private void Update()
    {
        // Check the current movement direction of the parent GameObject (ghost)
        // and set the appropriate eye sprite based on the direction
        if (this.movement.direction == Vector2.up)
        {
            this.spriteRenderer.sprite = this.up;
        }
        else if (this.movement.direction == Vector2.down)
        {
            this.spriteRenderer.sprite = this.down;
        }
        else if (this.movement.direction == Vector2.left)
        {
            this.spriteRenderer.sprite = this.left;
        }
        else if (this.movement.direction == Vector2.right)
        {
            this.spriteRenderer.sprite = this.right;
        }
    }

    // Awake is called when the GameObject is created in the scene
    private void Awake()
    {
        // Get a reference to the SpriteRenderer component attached to this GameObject
        this.spriteRenderer = GetComponent<SpriteRenderer>();

        // Get a reference to the Movement component attached to the parent GameObject (ghost)
        this.movement = GetComponentInParent<Movement>();
    }
}
