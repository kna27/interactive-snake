using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    public float timePassed;
    public float moveDelay = 0.5f;
    public int moveThreshold = 80;
    public int rotationIndex;
    private GameManager gameManager;
    Vector2 rotPos;
    private UDPSocket socket;
    private List<int> rotateDirs = new List<int>();
    private List<Vector2> cornerPositions = new List<Vector2>();
    private bool manualMode;
    
    private void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        if (GameObject.Find("Socket"))
        {
            socket = GameObject.Find("Socket").GetComponent<UDPSocket>();
        }
        else
        {
            manualMode = true;
        }
        
        ResetState();
    }

    private void CheckMovement()
    {
        int socketMove = 0;
        if (!manualMode)
        {
            if (socket.y1 >= moveThreshold && socket.y2 >= moveThreshold)
            {
                socketMove = socket.y1 > socket.y2 ? 1 : 2;
            }
            else if (socket.y1 >= moveThreshold)
            {
                socketMove = 1;
            }
            else if (socket.y2 >= moveThreshold)
            {
                socketMove = 2;
            }
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow) || socketMove == 1)
        {
            rotationIndex--;
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow) || socketMove == 2)
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
    }

    private void FixedUpdate()
    {
        float oldX = direction.x;
        float oldY = direction.y;
        if (timePassed == 0)
        {
            CheckMovement();
            if (input != Vector2.zero)
            {
                direction = input;
            }
            for (int i = segments.Count - 1; i > 0; i--)
            {
                if (i == segments.Count - 2)
                {
                    segments[i].GetComponent<SpriteRenderer>().sprite = snakeBodies[UnityEngine.Random.Range(0, (snakeBodies.Length - 1))];
                }
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

            if ((oldX - direction.x > 0) && (oldY >= 0))
            {
                if (direction.y < 0)
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
            else if ((oldX - direction.x < 0) && (oldY <= 0))
            {
                if (direction.y > 0)
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
            else if ((oldX - direction.x > 0) && (oldY <= 0))
            {
                transform.Rotate(0, 0, -90);
                rotateDirs.Add(2);
                segments[1].GetComponent<SpriteRenderer>().sprite = cornerRight;
                cornerPositions.Add(transform.position);
            }
            else if ((oldX - direction.x < 0) && (oldY >= 0))
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
        segment.GetComponent<SpriteRenderer>().sprite = tailSprite;
        segment.SetPositionAndRotation(segments[segments.Count - 1].position, segments[segments.Count - 1].rotation);
        segment.position = segments[segments.Count - 1].position;
        segments.Add(segment);
    }

    public void ResetState()
    {
        cornerPositions.Clear();
        direction = Vector2.right;
        transform.position = Vector3.zero;

        // Clear the list but add back this as the head
        segments.Clear();
        segments.Add(transform);
        Time.timeScale = 1f;
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