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
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        leftSlider.value = leftInput;
        rightSlider.value = rightInput;
        if(Input.GetKeyDown(KeyCode.W))
        {
            _direction = Vector2.up;
        } else if (Input.GetKeyDown(KeyCode.S))
        {
            _direction = Vector2.down;
        } else if (Input.GetKeyDown(KeyCode.A))
        {
            _direction = Vector2.left;
        } else if (Input.GetKeyDown(KeyCode.D))
        {
            _direction = Vector2.right;
        }
    }

    private void FixedUpdate()
    {
        transform.position = new Vector2(Mathf.Round(transform.position.x) + _direction.x, Mathf.Round(transform.position.y + _direction.y));
       // transform.position = new Vector2(Mathf.Round(transform.position.x) + )
    }
}
