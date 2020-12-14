using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Vector2 Direction;
    public float startingSpeed=3;
    public WallManager wallManager;
    public Arena arena;
    [SerializeField]
    private float maxSpeed=8;
    [SerializeField]
    private float speedMultiplier=0.1f;
    [SerializeField]
    private Menu menu;

    private float speed;
    private Vector3 moveTranslation;
    private Rigidbody2D rb2D;
    SpriteRenderer spriteRenderer;
    PlayerController playerController;
    GameObject collidingWall;

    static List<GameObject> objectsColliding=new List<GameObject>();
    
    bool isInArena = false;

    void Awake()
    {
        speed = startingSpeed;
        rb2D = GetComponent<Rigidbody2D>();
        playerController = GetComponent<PlayerController>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (playerController == null)
        {
            playerController = this.gameObject.AddComponent<PlayerController>();
        }
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Menu.gameState == Menu.GameState.play)
        {
            if (speed > maxSpeed)
            {
                speed = maxSpeed;
            }
            CheckInArena();
            if (isInArena)
            {
                spriteRenderer.color = Color.blue;

            }
            else
            {
                spriteRenderer.color = Color.red;
                Reset();
            }
            if (playerController.IsKeyDown)
            {
                this.Direction = playerController.direction;
            }
            else
            {
                this.Direction = Vector3.zero;
            }
        }
    }

    void FixedUpdate()
    {
        rb2D.MovePosition(rb2D.position + Direction * speed * Time.fixedDeltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //Inside of a wall, there is a smaller TriggerCollider, if the player hits this, it means they have been crushed between walls. 
        if (other.tag=="WallTag")
        {
            Reset();
        }
        else if(other.tag=="ScoreTrigger")
        {
            speed += speedMultiplier;
            ScoreManager.Score++;
            Debug.Log("Score: " + ScoreManager.Score);
            Destroy(other);
        }
        objectsColliding.Add(other.gameObject);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        objectsColliding.Remove(other.gameObject);
    }

    private void CheckInArena()
    {
        List<GameObject> objectsToRemove=new List<GameObject>();
        bool tempInArena = false;
        foreach(GameObject storedObject in objectsColliding)
        {
            if (storedObject == null)
            {
                objectsToRemove.Add(storedObject);
            }
            else
            {
                if (storedObject.GetComponent<ArenaSquare>() != null)
                {
                    if(storedObject.GetComponent<ArenaSquare>().GetSquareState()!=ArenaSquare.SquareState.broken)
                    {
                        tempInArena = true;
                    }
                }
            }
        }
        if(tempInArena==true)
        {
            isInArena = true;
        }
        else
        {
            isInArena = false;
        }

        foreach(GameObject removeObject in objectsToRemove)
        {
            objectsColliding.Remove(removeObject);
        }
    }
    
    public static void RemoveWall(GameObject wall)
    {
        objectsColliding.Remove(wall);
    }

    public void Reset()
    {
        transform.position = new Vector2(0, 0);
        speed = startingSpeed;
        ScoreManager.Score = 0;
        arena.GetComponent<Arena>().ResetArena();
        menu.GameOver();
        wallManager.ResetWalls();
        Time.timeScale = 0;
    }
}
