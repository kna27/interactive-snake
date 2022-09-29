using System.Collections;
using System.Collections.Generic;
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
    public Slider leftSlider;
    public Slider rightSlider;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        leftSlider.value = leftInput;
        rightSlider.value = rightInput;
    }
}
