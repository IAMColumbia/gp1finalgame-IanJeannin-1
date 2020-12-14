using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Util : MonoBehaviour
{
    static Vector3 bottomLeft;
    static Vector3 topRight;
    static Rect cameraRect;

    // Start is called before the first frame update
    void Start()
    {
        bottomLeft = Camera.main.ScreenToWorldPoint(Vector3.zero);
        topRight = Camera.main.ScreenToWorldPoint(new Vector3(
            Camera.main.pixelWidth, Camera.main.pixelHeight));

        cameraRect = new Rect(
            bottomLeft.x,
            bottomLeft.y,
            topRight.x - bottomLeft.x,
            topRight.y - bottomLeft.y);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static Vector2 GetBottomLeft()
    {
        return bottomLeft;
    }

    public static Vector2 GetTopRight()
    {
        return topRight;
    }
}
