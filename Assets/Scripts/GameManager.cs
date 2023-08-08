using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    // Multiplier applied to ghost points when eaten by Pacman
    public int ghostMultiplier { get; private set; } = 1;

    // Array of Ghost objects in the scene
    public Ghost[] ghosts;

    // Array of GameObjects representing lives in the UI
    public GameObject[] livesUI;

    // Reference to the PacMan script
    public PacMan pacman;

    // Transform that holds all the pellets in the scene
    public Transform pellets;

    // Current score
    public int score { get; private set; }

    // Number of remaining lives
    public int lives { get; private set; }

    // Time interval to count how long Pacman hasn't eaten any pellets
    public float intervalPellet = 0.0f;

    // Number of pellets eaten by Pacman
    public int pelletsEaten = 0;

    // Reference to the TextMeshProUGUI component displaying the score
    public TMP_Text scoreText;

    // Reference to the AudioSource component
    private AudioSource audioSource;

    // Array of audio clips for different sounds
    public AudioClip[] sounds;

    // Start is called before the first frame update
    void Start()
    {
        // Get the AudioSource component on the GameObject
        this.audioSource = GetComponent<AudioSource>();
        // Start a new game
        this.NewGame();
    }

    public void FixedUpdate()
    {
        // Update the time interval since Pacman last ate a pellet
        this.intervalPellet += Time.deltaTime;

        // Adjust ghost speed based on the number of pellets eaten
        if (this.pelletsEaten <= 20 && this.pelletsEaten > 10)
        {
            this.ghosts[0].movement.speed = 8.0f;
        }

        if (this.pelletsEaten <= 10)
        {
            this.ghosts[0].movement.speed = 9.0f;
        }
    }

    // Reset the state of ghosts and Pacman
    private void ResetState()
    {
        this.ResetGhostMultiplier();
        for (int i = 0; i < this.ghosts.Length; i++)
        {
            this.ghosts[i].ResetState();
        }

        this.pacman.ResetState();
    }

    // Reset the ghost multiplier to 1
    private void ResetGhostMultiplier()
    {
        this.ghostMultiplier = 1;
    }

    // Called when a Ghost is eaten by Pacman
    public void GhostEaten(Ghost ghost)
    {
        // Calculate the points for eating the ghost and apply the multiplier
        int points = ghost.points * ghostMultiplier;

        // Update the score and increase the ghost multiplier
        this.SetScore(this.score + points);
        this.ghostMultiplier++;
    }

    // Called when Pacman is eaten by a ghost
    public void PacmanEaten()
    {
        // Perform Pacman's death sequence and reduce one life
        this.pacman.DeathSequence();
        this.SetLives(this.lives - 1);
        this.updateLivesUI();

        // If there are remaining lives, reset the state after 3 seconds; otherwise, trigger Game Over
        if (this.lives > 0)
        {
            Invoke(nameof(ResetState), 3f);
        }
        else
        {
            this.audioSource.Stop();
            this.audioSource.PlayOneShot(this.sounds[1]);
            this.GameOver();
        }
    }

    // Update the UI to show remaining lives
    public void updateLivesUI() 
    {
        if (this.lives == 2)
        {
            this.livesUI[0].SetActive(false);
        } 
        else if (this.lives == 1)
        {
            this.livesUI[1].SetActive(false);
        } 
        else if (this.lives == 0) 
        {
            this.livesUI[2].SetActive(false);
        }
    }

    // Set the current score and update the score UI
    private void SetScore(int score)
    {
        this.score = score;
        this.scoreText.text = this.score.ToString();
    }

    // Set the number of lives
    private void SetLives(int lives)
    {
        this.lives = lives;
    }

    // Start a new game
    private void NewGame()
    {
        // Reset score, lives, and start a new round
        this.SetScore(0);
        this.SetLives(3);
        this.NewRound();
    }

    // Start a new round by activating all the pellets
    private void NewRound()
    {
        foreach (Transform pellet in this.pellets)
        {
            pellet.gameObject.SetActive(true);
        }
    }

    // Called when the game is over
    private void GameOver()
    {
        // Disable all the ghosts and Pacman
        for (int i = 0; i < this.ghosts.Length; i++)
        {
            this.ghosts[i].gameObject.SetActive(false);
        }

        this.pacman.gameObject.SetActive(false);
    }

    // Called when a normal pellet is eaten by Pacman
    public void PelletEaten(Pellet pellet)
    {
        // Play the chomp sound and reset the pellet interval
        this.playChompSound();
        this.intervalPellet = 0.0f;

        // Increase the number of pellets eaten and update the score
        this.pelletsEaten++;
        pellet.gameObject.SetActive(false);
        this.SetScore(this.score + pellet.points);

        // If there are no more pellets, end the round after 3 seconds
        if (!this.HasRemainingPellets())
        {
            this.pacman.gameObject.SetActive(false);
            Invoke(nameof(NewRound), 3.0f);
        }
    }

    // Play the chomp sound if it's not already playing
    private void playChompSound()
    {
        if (!this.audioSource.isPlaying)
        {
            this.audioSource.PlayOneShot(this.sounds[0]);
        }
    }

    // Called when a PowerPellet is eaten by Pacman
    public void PowerPelletEaten(PowerPellet pellet)
    {
        // Play the chomp sound and enable the frightened state for all ghosts
        this.playChompSound();
        for (int i = 0; i < this.ghosts.Length; i++)
        {
            this.ghosts[i].frightened.Enable();
        }

        // Continue with the regular pellet eaten logic
        this.PelletEaten(pellet);
        CancelInvoke();

        // Reset the ghost multiplier after the duration of the PowerPellet
        Invoke(nameof(ResetGhostMultiplier), pellet.duration);
    }

    // Check if there are remaining pellets in the scene
    private bool HasRemainingPellets()
    {
        foreach (Transform pellet in this.pellets)
        {
            if (pellet.gameObject.activeSelf)
            {
                return true;
            }
        }
        return false;
    }
}
