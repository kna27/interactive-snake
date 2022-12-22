using UnityEngine;
using UnityEngine.UI;

public class IndicatorFillers : MonoBehaviour
{
    private UDPSocket socket;
    private bool socketFound;
    public Slider left;
    public Slider right;

    void Start()
    {
        if (GameObject.Find("Socket"))
        {
            socket = GameObject.Find("Socket").GetComponent<UDPSocket>();
            socketFound = true;
        }
    }
    
    void Update()
    {
        if (socketFound)
        {
            left.value = socket.y2;
            right.value = socket.y1;
        }
    }
}
