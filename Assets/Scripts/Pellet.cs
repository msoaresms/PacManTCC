using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pellet : MonoBehaviour
{
    // Points earned when the pellet is eaten
    public int points = 10;

    // Start is called before the first frame update
    void Start()
    {
        // This method doesn't perform any actions in the start
    }

    // Update is called once per frame
    void Update()
    {
        // This method doesn't perform any actions in the update
    }

    // Called when another collider enters the trigger zone of this pellet
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the collider belongs to the PacMan object
        if (other.gameObject.layer == LayerMask.NameToLayer("Pacman"))
        {
            // Call the Eat method to handle the pellet being eaten
            this.Eat();
        }
    }

    // Method to handle the pellet being eaten
    protected virtual void Eat()
    {
        // Get the GameManager instance and call the PelletEaten method, passing this pellet as a parameter
        FindObjectOfType<GameManager>().PelletEaten(this);
    }
}
