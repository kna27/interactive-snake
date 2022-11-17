using System.Collections.Generic;
using UnityEngine;

public class Snake : MonoBehaviour
{
    private List<Transform> segments = new List<Transform>();
    public Transform segmentPrefab;
    public Vector2 direction = Vector2.right;
    private Vector2 input;
    public int initialSize = 4;
    private Vector2[] directions = { Vector2.right, Vector2.down, Vector2.left, Vector2.up };
    public Sprite cornerLeft;
    public Sprite cornerRight;
    public Sprite tailSprite;
    public Sprite[] snakeBodies;
    float timePassed;
    public float moveDelay = 0.5f;
    public int rotationIndex;
    private GameManager gameManager;
    Vector2 rotPos;
    private List<int> rotateDirs = new List<int>();
    private List<Vector2> cornerPositions = new List<Vector2>();
    private void Start()
    {
        ResetState();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            rotationIndex--;
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            rotationIndex++;
        }
        if (rotationIndex < 0)
        {
            rotationIndex = 3;
        }
        else if (rotationIndex > 3)
        {
            rotationIndex = 0;
        }
        input = directions[rotationIndex];
    }

    private void FixedUpdate()
    {
        float oldX = direction.x;
        float oldY = direction.y;
        if (timePassed == 0)
        {
            if (input != Vector2.zero)
            {
                direction = input;
            }
            for (int i = segments.Count - 1; i > 0; i--)
            {
                segments[i].SetPositionAndRotation(segments[i - 1].position, segments[i - 1].rotation);
                if (cornerPositions.Contains(segments[i].position))
                {
                   if (i == segments.Count - 1)
                   {
                        segments[i].transform.Rotate(0, 0, rotateDirs[cornerPositions.IndexOf(segments[i].position)] == 2 ? -90 : 90);
                        cornerPositions.RemoveAt(0);
                        rotateDirs.RemoveAt(0);
                   }
                    else
                    {
                        segments[i].GetComponent<SpriteRenderer>().sprite = rotateDirs[cornerPositions.IndexOf(segments[i].position)] == 2 ? cornerRight : cornerLeft;
                    }
                }
               else
                {
                    segments[i].GetComponent<SpriteRenderer>().sprite = snakeBodies[UnityEngine.Random.Range(0, (snakeBodies.Length - 1))];
                    segments[i].SetPositionAndRotation(segments[i].position, segments[i - 1].rotation);
                }
                segments[segments.Count - 1].GetComponent<SpriteRenderer>().sprite = tailSprite;
            }

            // Move the snake in the direction it is facing
            // Round the values to ensure it aligns to the grid
            float x = Mathf.Round(transform.position.x) + direction.x;
            float y = Mathf.Round(transform.position.y) + direction.y;

            if((oldX - direction.x > 0) && (oldY >= 0))
            {
                if(direction.y < 0)
                {
                    transform.Rotate(0, 0, -90);
                    segments[1].GetComponent<SpriteRenderer>().sprite = cornerRight;
                rotateDirs.Add(2);
                    cornerPositions.Add(transform.position);
                }
                else
                {
                    transform.Rotate(0, 0, 90);
                rotateDirs.Add(1);
                    segments[1].GetComponent<SpriteRenderer>().sprite = cornerLeft;
                    cornerPositions.Add(transform.position);
                }
            }
            else if((oldX - direction.x < 0) && (oldY <= 0))
            {
                if(direction.y > 0)
                {
                    transform.Rotate(0, 0, -90);
                    segments[1].GetComponent<SpriteRenderer>().sprite = cornerRight;
                rotateDirs.Add(2);
                    cornerPositions.Add(transform.position);
                }
                else
                {
                    transform.Rotate(0, 0, 90);
                    segments[1].GetComponent<SpriteRenderer>().sprite = cornerLeft;
                rotateDirs.Add(1);
                cornerPositions.Add(transform.position);
                }
            }
            else if((oldX - direction.x > 0) && (oldY <= 0))
            {
                transform.Rotate(0, 0, -90);
            rotateDirs.Add(2);
            segments[1].GetComponent<SpriteRenderer>().sprite = cornerRight;
                cornerPositions.Add(transform.position);
            }
            else if((oldX - direction.x < 0) && (oldY >= 0))
            {
                transform.Rotate(0, 0, -90);
            rotateDirs.Add(2);
            segments[1].GetComponent<SpriteRenderer>().sprite = cornerRight;
                cornerPositions.Add(transform.position);
            }

            transform.position = new Vector2(x, y);
        }
        timePassed += Time.deltaTime;
        if (timePassed >= moveDelay)
        {
            timePassed = 0;
        }
    }

    public void Grow()
    {
        Transform segment = Instantiate(segmentPrefab);
        segment.GetComponent<SpriteRenderer>().sprite = snakeBodies[UnityEngine.Random.Range(0, (snakeBodies.Length - 1))];
        segment.position = segments[segments.Count - 1].position;
        segments.Add(segment);
    }

    public void ResetState()
    {
        cornerPositions.Clear();
        direction = Vector2.right;
        transform.position = Vector3.zero;

        // Start at 1 to skip destroying the head
        for (int i = 1; i < segments.Count; i++)
        {
            Destroy(segments[i].gameObject);
        }

        // Clear the list but add back this as the head
        segments.Clear();
        segments.Add(transform);

        // -1 since the head is already in the list
        for (int i = 0; i < initialSize - 1; i++)
        {
            Grow();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Food"))
        {
            Grow();
            gameManager.score++;
        }
        else if (other.gameObject.CompareTag("Obstacle") || other.gameObject.CompareTag("Player"))
        {
            gameManager.Die();
        }
    }

}