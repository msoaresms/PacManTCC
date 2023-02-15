using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostHome : GhostBehavior
{
    public Transform insideTransform;

    public Transform outsideTransform;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnDisable()
    {
        if (this.gameObject.activeSelf)
        {
            StartCoroutine(ExitTransition());
        }
    }

    private IEnumerator ExitTransition()
    {
        this.ghost.movement.SetDirection(Vector2.up, true);
        this.ghost.movement.rigidbody.isKinematic = true;
        this.ghost.movement.enabled = false;

        Vector3 position = transform.position;

        float duration = 0.5f;
        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            Vector3 newPosition =
                Vector3
                    .Lerp(position,
                    this.insideTransform.position,
                    elapsed / duration);

            newPosition.z = position.z;
            this.ghost.transform.position = newPosition;
            elapsed += Time.deltaTime;
            yield return null;
        }

        elapsed = 0.0f;

        while (elapsed < duration)
        {
            Vector3 newPosition =
                Vector3
                    .Lerp(this.insideTransform.position,
                    this.outsideTransform.position,
                    elapsed / duration);

            newPosition.z = position.z;
            this.ghost.transform.position = newPosition;
            elapsed += Time.deltaTime;
            yield return null;
        }

        this.ghost.movement.SetDirection(Vector2.left, true);
        this.ghost.movement.rigidbody.isKinematic = false;
        this.ghost.movement.enabled = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (
            this.enabled &&
            collision.gameObject.layer == LayerMask.NameToLayer("Obstacle")
        )
        {
            this.ghost.movement.SetDirection(-this.ghost.movement.direction);
        }
    }

    private void OnEnable()
    {
        StopAllCoroutines();
    }
}
