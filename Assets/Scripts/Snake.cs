using UnityEngine;
using UnityEngine.UI;

public class Snake : MonoBehaviour
{
    public bool moveUp;
    public bool moveLeft;
    public bool moveRight;
    public bool moveDown;
    public int leftInput;
    public int rightInput;
    public int facingDirection;
    public Slider leftSlider;
    public Slider rightSlider;
    public Vector2 _direction = Vector2.right;
    float timePassed;
    public float moveDelay = 0.5f;
    public Vector2[] directions;
    public int rotationIndex;
    // Start is called before the first frame update
    void Start()
    {
        Vector2[] directions = { Vector2.right, Vector2.down, Vector2.left, Vector2.up};
    }

    // Update is called once per frame
    void Update()
    {
        
        leftSlider.value = leftInput;
        rightSlider.value = rightInput;
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
    }
        timePassed += Time.deltaTime;
        if (timePassed >= moveDelay)
        {
            timePassed = 0;
        }
        // transform.position = new Vector2(Mathf.Round(transform.position.x) + )
    }
}
