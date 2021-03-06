﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    public Vector2 Direction;
    private Rigidbody2D rb2D;
    public static float Speed=1;

    // Start is called before the first frame update
    void Start()
    {
        //Time.timeScale = 0;
        if(gameObject.GetComponent<Rigidbody2D>()==null)
        {
            rb2D=gameObject.AddComponent<Rigidbody2D>();
        }
        rb2D = gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        rb2D.MovePosition(rb2D.position + Direction * Speed * Time.fixedDeltaTime);
        //Once wall leaves screen, destroy it
        if(transform.position.x<Util.GetBottomLeft().x||transform.position.x>Util.GetTopRight().x||transform.position.y<Util.GetBottomLeft().y||transform.position.y>Util.GetTopRight().y)
        {
            WallManager.RemoveWall(this);
            Destroy(this.gameObject);
        }
    }
}
