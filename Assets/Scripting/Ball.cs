using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public enum CollisionTag
    {
        BounceWall,
        Player,
        ScoreWall
    }
    [SerializeField] private float ballSpeed = 8f;
    [SerializeField] private List<string> collisionTags;
    private Vector2 ballDirection;

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip wallCollide;
    [SerializeField] private AudioClip playerCollide;
    [SerializeField] private AudioClip goalCollide;
    void Start()
    {
        transform.position = Vector2.zero;
        ballDirection = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
    }

    void Update()
    {
        transform.Translate(ballDirection * ballSpeed * Time.deltaTime);
    }
    //Reset ball resets the ball to its base position at a random direction
    private void ResetBall()
    {
        transform.position = Vector2.zero;
        ballDirection = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(collisionTags[(int)CollisionTag.ScoreWall]))
        {
            audioSource.PlayOneShot(goalCollide);
            ResetBall();
            GameManager.IncrementScore(other.GetComponent<ScoreWall>().scoringPlayer);
        }
        else if (other.CompareTag(collisionTags[(int)CollisionTag.BounceWall]))
        {
            audioSource.PlayOneShot(wallCollide);
            ballDirection.y = -ballDirection.y;
        }
        else if (other.CompareTag(collisionTags[(int)CollisionTag.Player]))
        {
            audioSource.PlayOneShot(playerCollide);
            ballDirection.x = -ballDirection.x;
            ballDirection.y = transform.position.y - other.transform.position.y;
            ballDirection = ballDirection.normalized;
        }
    }
}
