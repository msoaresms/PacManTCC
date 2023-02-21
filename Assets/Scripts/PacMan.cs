using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Movement))]
public class PacMan : MonoBehaviour
{
    public AnimatedSprite deathSequence;

    public Movement movement { get; private set; }

    public SpriteRenderer spriteRenderer { get; private set; }

    public new Collider2D collider { get; private set; }

    public Transform pinkyTarget;
    public Transform inkyAuxTarget;
    public Transform blinky;
    public Transform inkyTarget;
    public Transform clyde;
    public Transform clydeTarget;
    public Transform clydeScatterTarget;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 pacman = this.transform.position;

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

        float angle =
            Mathf.Atan2(this.movement.direction.y, this.movement.direction.x);
        this.transform.rotation =
            Quaternion.AngleAxis(angle * Mathf.Rad2Deg, Vector3.forward);

        if (this.movement.direction == Vector2.up)
        {
            this.pinkyTarget.transform.position = new Vector3(pacman.x - 4.0f, pacman.y + 4.0f, 0.0f);
            this.inkyAuxTarget.transform.position = new Vector3(pacman.x - 2.0f, pacman.y + 2.0f, 0.0f);
        }
        else if (this.movement.direction == Vector2.down)
        {
            this.pinkyTarget.transform.position = new Vector3(pacman.x, pacman.y - 4.0f, 0.0f);
            this.inkyAuxTarget.transform.position = new Vector3(pacman.x, pacman.y - 2.0f, 0.0f);
        }
        else if (this.movement.direction == Vector2.left)
        {
            this.pinkyTarget.transform.position = new Vector3(pacman.x - 4.0f, pacman.y, 0.0f);
            this.inkyAuxTarget.transform.position = new Vector3(pacman.x - 2.0f, pacman.y, 0.0f);
        }
        else if (this.movement.direction == Vector2.right)
        {
            this.pinkyTarget.transform.position = new Vector3(pacman.x + 4.0f, pacman.y, 0.0f);
            this.inkyAuxTarget.transform.position = new Vector3(pacman.x + 2.0f, pacman.y, 0.0f);
        }

        this.inkyTarget.transform.position = Vector3.LerpUnclamped(this.blinky.transform.position, this.inkyAuxTarget.transform.position, 2.0f);

        if (Vector3.Distance(this.transform.position, this.clyde.transform.position) >= 8.0f)
        {
            this.clydeTarget.position = this.transform.position;
        }
        else
        {

            this.clydeTarget.position = this.clydeScatterTarget.transform.position;
        }

    }

    private void Awake()
    {
        this.movement = GetComponent<Movement>();
        this.spriteRenderer = GetComponent<SpriteRenderer>();
        this.collider = GetComponent<Collider2D>();
    }

    public void ResetState()
    {
        this.enabled = true;
        this.spriteRenderer.enabled = true;
        this.collider.enabled = true;

        // this.deathSequence.enabled = false;
        // this.deathSequence.spriteRenderer.enabled = false;
        this.movement.ResetState();
        this.gameObject.SetActive(true);
    }

    public void DeathSequence()
    {
        this.enabled = false;
        this.spriteRenderer.enabled = false;
        this.collider.enabled = false;
        this.movement.enabled = false;
        // this.deathSequence.enabled = true;
        // this.deathSequence.spriteRenderer.enabled = true;
        // this.deathSequence.Restart();
    }

    private void OnDrawGizmos()
    {

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(this.transform.position, 8);

    }
}
