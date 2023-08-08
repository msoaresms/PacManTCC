using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Passage : MonoBehaviour
{
    // Reference to the transform of the passage's connection point
    public Transform connection;

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

    // Called when another collider enters the trigger zone of this passage
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Get the position of the object that entered the trigger
        Vector3 position = other.transform.position;

        // Update the X and Y coordinates of the object's position to match the connection point's position
        position.x = this.connection.position.x;
        position.y = this.connection.position.y;

        // Set the updated position of the object
        other.transform.position = position;
    }
}
