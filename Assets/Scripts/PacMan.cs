using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Movement))]
public class PacMan : MonoBehaviour
{
    // Reference to the animated sprite used for the death sequence
    public AnimatedSprite deathSequence;

    // Reference to the Movement script attached to this game object
    public Movement movement { get; private set; }

    // Reference to the SpriteRenderer component attached to this game object
    public SpriteRenderer spriteRenderer { get; private set; }

    // Reference to the Collider2D component attached to this game object
    public new Collider2D collider { get; private set; }

    // Update is called once per frame
    void Update()
    {
        // Check for input to change the movement direction of PacMan
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            this.movement.SetDirection(Vector2.up);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            this.movement.SetDirection(Vector2.down);
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            this.movement.SetDirection(Vector2.left);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            this.movement.SetDirection(Vector2.right);
        }

        // Calculate the angle of rotation for PacMan based on its movement direction
        float angle = Mathf.Atan2(this.movement.direction.y, this.movement.direction.x);

        // Rotate PacMan to face its movement direction
        this.transform.rotation = Quaternion.AngleAxis(angle * Mathf.Rad2Deg, Vector3.forward);
    }

    // Called when the object is first awakened in the scene
    private void Awake()
    {
        // Get references to the Movement, SpriteRenderer, and Collider2D components
        this.movement = GetComponent<Movement>();
        this.spriteRenderer = GetComponent<SpriteRenderer>();
        this.collider = GetComponent<Collider2D>();
    }

    // Reset the state of PacMan to its initial values
    public void ResetState()
    {
        this.enabled = true;
        this.spriteRenderer.enabled = true;
        this.collider.enabled = true;
        this.movement.ResetState();
        this.gameObject.SetActive(true);
    }

    // Trigger the death sequence for PacMan when it is eaten by a ghost
    public void DeathSequence()
    {
        this.enabled = false;
        this.spriteRenderer.enabled = false;
        this.collider.enabled = false;
        this.movement.enabled = false;
    }

    // Draw a yellow wire sphere around PacMan when selected in the Unity editor
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(this.transform.position, 8);
    }
}
