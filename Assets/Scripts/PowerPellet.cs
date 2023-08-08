using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerPellet : Pellet
{
    // Duration of the power pellet effect
    public float duration = 8.0f;

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

    // Overrides the Eat method from the base class (Pellet)
    protected override void Eat()
    {
        // Get the GameManager instance and call the PowerPelletEaten method, passing this power pellet as a parameter
        FindObjectOfType<GameManager>().PowerPelletEaten(this);
    }
}
