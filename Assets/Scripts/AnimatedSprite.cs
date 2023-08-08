using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatedSprite : MonoBehaviour
{
    // Reference to the SpriteRenderer component of the GameObject
    public SpriteRenderer spriteRenderer { get; private set; }

    // Array that holds Sprites for the animation
    public Sprite[] sprites = new Sprite[0];

    // Duration of each frame of the animation
    public float animationTime = 0.25f;

    // Index of the current animation frame
    public int animationFrame { get; private set; }

    // Indicates if the animation should loop
    public bool loop = true;

    // Called at the start of the script execution
    void Start()
    {
        // Configures the Advance method to be called repeatedly after a specified time interval
        InvokeRepeating(nameof(this.Advance),
        this.animationTime,
        this.animationTime);
    }

    // Called when the GameObject is created in the scene
    private void Awake()
    {
        // Get the reference to the SpriteRenderer component of the GameObject
        this.spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Advances to the next frame of the animation
    private void Advance()
    {
        // Check if the SpriteRenderer is disabled
        if (!this.spriteRenderer.enabled)
        {
            return;
        }

        // Increment the index of the current frame
        this.animationFrame++;

        // Check if the animation reached its end and should restart in loop
        if (this.animationFrame >= this.sprites.Length && this.loop)
        {
            this.animationFrame = 0;
        }

        // Check if the frame index is within the bounds of the Sprites array
        if (
            this.animationFrame >= 0 &&
            this.animationFrame < this.sprites.Length
        )
        {
            // Set the current Sprite of the SpriteRenderer based on the frame index
            this.spriteRenderer.sprite = this.sprites[this.animationFrame];
        }
    }

    // Restarts the animation
    public void Restart()
    {
        // Reset the frame index to -1, so it will advance to the first frame on the next update
        this.animationFrame = -1;
        this.Advance();
    }
}
