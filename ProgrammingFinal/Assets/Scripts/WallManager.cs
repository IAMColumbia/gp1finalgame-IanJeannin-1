using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallManager : MonoBehaviour
{
    public List<GameObject> xPrefabs;
    public List<GameObject> yPrefabs;

    [SerializeField]
    private float wallSpawnTimer=1;
    [Tooltip("How much faster the wall speed gets every time the player scores.")]
    [SerializeField]
    private float speedMultiplier=0.1f;
    [Tooltip("How quickly the cooldown between wall spawns will decrease every time the player scores.")]
    [SerializeField]
    private float cooldownMultiplier = 0.1f;

    private static List<Wall> walls=new List<Wall>();
    private static int lastRandom=10; //Nonsense number so that the first time it's checked isn't null
    private float speedCounter=0;
    private float newSpeed;

    // Start is called before the first frame update
    void Start()
    {
        SpawnWall();
        StartCoroutine("WallSpawnTimer");
    }

    private void SpawnWall()
    {
        GameObject prefab;
        Vector2 pos;
        Vector2 direction;
        int randomXOrY = Random.Range(0, 5);
        //If the last direction is the same as this direction, reroll once. 
        if(randomXOrY==lastRandom)
        {
            randomXOrY = Random.Range(0, 5);
        }
        lastRandom = randomXOrY;

        //Left Prefab
        if (randomXOrY == 0)
        {
            pos = new Vector2(Util.GetBottomLeft().x, 0);
            direction = new Vector2(1, 0);
            prefab = xPrefabs[Random.Range(0, xPrefabs.Count)];
        }
        //Right Prefab
        else if (randomXOrY == 1)
        {
            pos = new Vector2(Util.GetTopRight().x, 0);
            direction = new Vector2(-1, 0);
            prefab = xPrefabs[Random.Range(0, xPrefabs.Count)];
        }
        //Top Prefab
        else if (randomXOrY == 2)
        {
            pos = new Vector2(0, Util.GetTopRight().y);
            direction = new Vector2(0, -1);
            prefab = yPrefabs[Random.Range(0, xPrefabs.Count)];
        }
        //BottomPrefab
        else
        {
            pos = new Vector2(0, Util.GetBottomLeft().y);
            direction = new Vector2(0, 1);
            prefab = yPrefabs[Random.Range(0, xPrefabs.Count)];
        }
        GameObject w = (GameObject)Instantiate(prefab, pos, Quaternion.identity, this.transform);
        w.GetComponent<Wall>().Direction = direction;
        walls.Add(w.GetComponent<Wall>());
    }

    // Update is called once per frame
    void Update()
    {
        if(speedCounter!=ScoreManager.Score)
        {
            speedCounter = ScoreManager.Score;
            Wall.Speed += speedMultiplier;
            Player.Speed += speedMultiplier;
            wallSpawnTimer -= cooldownMultiplier;
        }
    }

    private IEnumerator WallSpawnTimer()
    {
        yield return new WaitForSeconds(wallSpawnTimer);
        SpawnWall();
        StartCoroutine("WallSpawnTimer");
    }

    public static void RemoveWall(Wall wall)
    {
        walls.Remove(wall);
    }

    public void ResetWalls()
    {
        for(int x=walls.Count-1;x>=0;x--)
        {
            Destroy(walls[x].gameObject);
        }
        Wall.Speed = 1;
        walls.Clear();
        SpawnWall();
    }
}
