using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arena : MonoBehaviour
{
    [SerializeField]
    private GameObject arenaPrefab;

    SpriteRenderer spriteRenderer;
    Vector2 bottomLeft;
    Vector2 topRight;
    private GameObject[,] arenaSquares=new GameObject[4,4];

    
    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        bottomLeft = transform.position - spriteRenderer.bounds.size/2;
        topRight = transform.position + spriteRenderer.bounds.size/2;
        CreateArena();
        ChangeDifficulty();
    }

    private void CreateArena()
    {
        float squareSize = arenaPrefab.GetComponent<SpriteRenderer>().bounds.size.x;
        float startingX = bottomLeft.x+squareSize/2;
        float startingY = topRight.y-squareSize/2;
        float xLoc;
        float yLoc;
        for (int y = 0; y < arenaSquares.GetLength(1); y++)
        {
            yLoc = startingY - squareSize * y;
            for(int x=0;x<arenaSquares.GetLength(0);x++)
            {
                xLoc = startingX + squareSize * x;
                arenaSquares[x,y]=(GameObject)Instantiate(arenaPrefab, new Vector2(xLoc, yLoc), Quaternion.identity);
            }
        }
    }

    public void ResetArena()
    {
        foreach(GameObject x in arenaSquares)
        {
            x.GetComponent<ArenaSquare>().ResetDurability();
        }
    }

    public void ChangeDifficulty()
    {
        foreach (GameObject square in arenaSquares)
        {
            if (square.GetComponent<ArenaSquare>() != null)
            {
                square.GetComponent<ArenaSquare>().ResetDurability();
            }
        }
        ArenaSquareSettings.ChangeDifficulty();
    }

}
