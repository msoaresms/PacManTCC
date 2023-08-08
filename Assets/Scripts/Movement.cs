using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Movement : MonoBehaviour
{
    // The speed of movement for the object
    public float speed = 8.0f;

    // Multiplier to adjust the speed of movement (e.g., when ghost is frightened)
    public float speedMultiplier = 1.0f;

    // The initial direction the object will move in
    public Vector2 initialDirection;

    // Layer mask for obstacle detection
    public LayerMask obstacleLayer;

    // Reference to the Rigidbody2D component
    public new Rigidbody2D rigidbody { get; private set; }

    // The current movement direction
    public Vector2 direction { get; private set; }

    // The next movement direction (used for delayed turns)
    public Vector2 nextDirection { get; private set; }

    // The starting position of the object
    public Vector3 startingPosition { get; private set; }

    // Called when the object is first awakened in the scene
    private void Awake()
    {
        // Get a reference to the Rigidbody2D component
        this.rigidbody = GetComponent<Rigidbody2D>();

        // Store the starting position of the object
        this.startingPosition = this.transform.position;
    }

    // Start is called before the first frame update
    void Start()
    {
        // Reset the state of the object when the game starts
        ResetState();
    }

    // Update is called once per frame
    void Update()
    {
        // If a next direction has been set, update the movement direction to the next direction
        if (this.nextDirection != Vector2.zero)
        {
            SetDirection(this.nextDirection);
        }
    }

    // Reset the state of the object to its initial values
    public void ResetState()
    {
        this.speedMultiplier = 1.0f;
        this.direction = this.initialDirection;
        this.nextDirection = Vector2.zero;
        this.transform.position = this.startingPosition;
        this.rigidbody.isKinematic = false;
        this.enabled = true;
    }

    // Called in fixed intervals for physics-based updates
    private void FixedUpdate()
    {
        // Calculate the translation vector for the object's movement
        Vector2 position = this.rigidbody.position;
        Vector2 translation = this.direction * this.speed * this.speedMultiplier * Time.fixedDeltaTime;

        // Move the object using the Rigidbody2D component
        this.rigidbody.MovePosition(position + translation);
    }

    // Set the movement direction of the object (and handle delayed turns)
    public void SetDirection(Vector2 direction, bool forced = false)
    {
        // If the direction change is forced or the new direction is not occupied by an obstacle, set the new direction
        if (forced || !Occupied(direction))
        {
            this.direction = direction;
            this.nextDirection = Vector2.zero;
        }
        // If the new direction is occupied by an obstacle, store it as the next direction to be applied later
        else
        {
            this.nextDirection = direction;
        }
    }

    // Check if the given direction is occupied by an obstacle using raycasting
    public bool Occupied(Vector2 direction)
    {
        // Perform a box cast from the current position in the given direction to check for obstacles
        RaycastHit2D hit = Physics2D.BoxCast(this.transform.position, Vector2.one * 0.75f, 0.0f, direction, 1.5f, this.obstacleLayer);

        // If the box cast hits a collider, the direction is considered occupied
        return hit.collider != null;
    }
}
