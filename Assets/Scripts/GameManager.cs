using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int ghostMultiplier { get; private set; } = 1;

    public Ghost[] ghosts;

    public PacMan pacman;

    public Transform pellets;

    // public Text scoreText;
    // public Text livesText;
    // public Text gameOverText;
    public int score { get; private set; }

    public int lives { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        this.NewGame();
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void ResetState()
    {
        this.ResetGhostMultiplier();
        for (int i = 0; i < this.ghosts.Length; i++)
        {
            this.ghosts[i].ResetState();
        }

        this.pacman.ResetState();
    }

    private void ResetGhostMultiplier()
    {
        this.ghostMultiplier = 1;
    }

    public void GhostEaten(Ghost ghost)
    {
        int points = ghost.points * ghostMultiplier;

        this.SetScore(this.score + points);
        this.ghostMultiplier++;
    }

    public void PacmanEaten()
    {
        this.pacman.DeathSequence();
        this.SetLives(this.lives - 1);

        if (this.lives > 0)
        {
            Invoke(nameof(ResetState), 3f);
        }
        else
        {
            this.GameOver();
        }
    }

    private void SetScore(int score)
    {
        this.score = score;
        // this.scoreText.text = score.ToString().PadLeft(2, '0');
    }

    private void SetLives(int lives)
    {
        this.lives = lives;
        // this.livesText.text = "x" + this.lives.ToString();
    }

    private void NewGame()
    {
        this.SetScore(0);
        this.SetLives(3);
        this.NewRound();
    }

    private void NewRound()
    {
        // this.gameOverText.enabled = false;
        foreach (Transform pellet in this.pellets)
        {
            pellet.gameObject.SetActive(true);
        }
    }

    private void GameOver()
    {
        // this.gameOverText.enabled = true;
        for (int i = 0; i < this.ghosts.Length; i++)
        {
            this.ghosts[i].gameObject.SetActive(false);
        }

        this.pacman.gameObject.SetActive(false);
    }

    public void PelletEaten(Pellet pellet)
    {
        pellet.gameObject.SetActive(false);
        this.SetScore(this.score + pellet.points);

        if (!this.HasRemainingPellets())
        {
            this.pacman.gameObject.SetActive(false);
            Invoke(nameof(NewRound), 3.0f);
        }
    }

    public void PowerPelletEaten(PowerPellet pellet)
    {
        for (int i = 0; i < this.ghosts.Length; i++)
        {
            this.ghosts[i].frightened.Enable(pellet.duration);
        }

        this.PelletEaten(pellet);
        CancelInvoke();
        Invoke(nameof(ResetGhostMultiplier), pellet.duration);
    }

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