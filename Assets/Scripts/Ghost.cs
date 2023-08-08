using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : MonoBehaviour
{
    // Points awarded when this ghost is eaten by Pacman
    public int points = 200;

    // Reference to the Movement component of the ghost
    public Movement movement { get; private set; }

    // Reference to the GhostHome component of the ghost
    public GhostHome home { get; private set; }

    // Reference to the GhostChase component of the ghost
    public GhostChase chase { get; private set; }

    // Reference to the GhostScatter component of the ghost
    public GhostScatter scatter { get; private set; }

    // Reference to the GhostFrightened component of the ghost
    public GhostFrightened frightened { get; private set; }

    // Initial behavior of the ghost (GhostHome, GhostChase, or GhostScatter)
    public GhostBehavior initialBehavior;

    // Start is called before the first frame update
    void Start()
    {
        // Reset the state of the ghost
        this.ResetState();
    }

    // Called when the GameObject is created in the scene
    private void Awake()
    {
        // Get references to various components attached to the ghost GameObject
        this.movement = GetComponent<Movement>();
        this.home = GetComponent<GhostHome>();
        this.scatter = GetComponent<GhostScatter>();
        this.chase = GetComponent<GhostChase>();
        this.frightened = GetComponent<GhostFrightened>();
    }

    // Reset the state of the ghost to its initial state
    public void ResetState()
    {
        // Enable the GameObject
        this.gameObject.SetActive(true);

        // Reset the movement component of the ghost
        this.movement.ResetState();

        // Disable the frightened behavior and enable the scatter behavior
        this.frightened.Disable();
        this.chase.Disable();
        this.scatter.Enable();

        // If this ghost has a home behavior and it's not the initial behavior, disable it
        if (this.home != this.initialBehavior)
        {
            this.home.Disable();
        }

        // If an initial behavior is set, enable it
        if (this.initialBehavior != null)
        {
            this.initialBehavior.Enable();
        }
    }

    // Called when the ghost collides with another GameObject
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the collision is with the Pacman GameObject
        if (collision.gameObject.layer == LayerMask.NameToLayer("Pacman"))
        {
            // If the ghost is in the frightened state, it gets eaten by Pacman
            if (this.frightened.enabled)
            {
                // Notify the GameManager that the ghost was eaten by Pacman
                FindObjectOfType<GameManager>().GhostEaten(this);
            }
            else
            {
                // Otherwise, Pacman gets eaten by the ghost
                FindObjectOfType<GameManager>().PacmanEaten();
            }
        }
    }
}
