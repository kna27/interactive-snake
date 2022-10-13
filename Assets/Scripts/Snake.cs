using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
public class Snake : MonoBehaviour
{
    public bool moveUp;
    public bool moveLeft;
    public bool moveRight;
    public bool moveDown;
    public int leftInput;
    public int rightInput;
    public int facingDirection;
    //public Slider leftSlider;
    //public Slider rightSlider;
    public Vector2 _direction = Vector2.right;
    float timePassed;
    public float moveDelay = 0.5f;
    public Vector2[] directions;
    public int rotationIndex;
    public List<Transform> _segments;
    public Transform segmentPrefab;
    public int score;
    public Sprite[] snakeBodies;
    // Start is called before the first frame update
    void Start()
    {
        Vector2[] directions = { Vector2.right, Vector2.down, Vector2.left, Vector2.up};
        //_segments = new List<Transform>();
        //_segments.Add(transform);
    }

    // Update is called once per frame
    void Update()
    {
        
        //leftSlider.value = leftInput;
        //rightSlider.value = rightInput;

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            rotationIndex--;
        } else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            rotationIndex++;
        }
        if (rotationIndex < 0)
        {
            rotationIndex = 3;
        } else if (rotationIndex > 3)
        {
            rotationIndex = 0;
        }

    }

    private void FixedUpdate()
    {
    if (timePassed == 0)
    {
        transform.position = new Vector2(Mathf.Round(transform.position.x) + directions[rotationIndex].x, Mathf.Round(transform.position.y + directions[rotationIndex].y));
            for (int i = _segments.Count -1; i > 0; i--)
            {
                _segments[i].position = _segments[i - 1].position;
            }
    }
        timePassed += Time.deltaTime;
        if (timePassed >= moveDelay)
        {
            timePassed = 0;
        }
    }

    private void Grow()
    {
        Transform segment = Instantiate(segmentPrefab);
        segment.GetComponent<SpriteRenderer>().sprite = snakeBodies[Random.Range(0, snakeBodies.Length - 1)];
        segment.position = _segments[_segments.Count - 1].position;
        _segments.Add(segment);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Food")
        {
            Grow();
            score++;
        }
    }
}
