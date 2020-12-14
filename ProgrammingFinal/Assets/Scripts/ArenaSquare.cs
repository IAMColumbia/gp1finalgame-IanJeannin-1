using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaSquare : MonoBehaviour
{
    [Tooltip("How long the player can stay on a square before it breaks.")]
    [SerializeField]
    private float maxDurability=5;
    [Tooltip("How long the player must stay off the square for it to start regenerating")]
    [SerializeField]
    private float regeneratePeriod=0.1f;
    [Tooltip("How much the object regenerates per second.")]
    [SerializeField]
    private float regenerateRate=0.25f;
    [Tooltip("After durability goes to 0, how long the player has before it breaks completely.")]
    [SerializeField]
    private float breakPeriod=1;

    float durability;
    enum SquareState { onSquare, offSquare, regenerating, breaking };
    SquareState squareState=SquareState.offSquare;
    private SpriteRenderer spriteRenderer;
    Color32 startingColor;
    Color32 middleColor = new Color32(255, 255, 0, 255);
    Color32 endColor = new Color32(255, 0,0,255);
    float middleLerp = 0; //Color.Lerp from starting color to middle color
    float endLerp = 0; //Color.Lerp from middle color to end color
    float breakLerp = 0;
    bool hasBroken = false;

    // Start is called before the first frame update
    void Start()
    {
        durability = maxDurability;
        spriteRenderer = this.gameObject.GetComponent<SpriteRenderer>();
        startingColor = spriteRenderer.color;
    }

    // Update is called once per frame
    void Update()
    {
        if (squareState == SquareState.breaking)
        {
            if (hasBroken == false)
            {
                StopAllCoroutines();
                StartCoroutine("BreakSquare");
                hasBroken = true;
            }
        }
        else if (squareState==SquareState.onSquare)
        {
            LoseDurability();
        }
        else if(squareState==SquareState.regenerating)
        {
            Regenerate();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.GetComponent<Player>()!=null&&squareState!=SquareState.breaking)
        {
            squareState=SquareState.onSquare;
            StopAllCoroutines();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Player>() != null&&squareState!=SquareState.breaking)
        {
            squareState = SquareState.offSquare;
            StopAllCoroutines();
            StartCoroutine("BeginRegen");
        }
    }

    private IEnumerator BeginRegen()
    {
        yield return new WaitForSeconds(regeneratePeriod);
        squareState = SquareState.regenerating;
    }

    private void Regenerate()
    {
        if (durability > 0 && durability<maxDurability)
        {
            durability += Time.deltaTime * regenerateRate;

            if (durability >= maxDurability / 2)
            {
                spriteRenderer.color = Color.Lerp(startingColor, middleColor, middleLerp);
                if (middleLerp < 1)
                {
                    //middleLerp -= (Time.deltaTime * regenerateRate) / (maxDurability / 2)*regenerateRate;
                    middleLerp = ((maxDurability - durability) / maxDurability) * 2;
                }
            }
            else
            {
                spriteRenderer.color = Color.Lerp(middleColor, endColor, endLerp);
                if (endLerp < 1)
                {
                    //endLerp -= (Time.deltaTime * regenerateRate) / (maxDurability / 2)*regenerateRate;
                    endLerp = ((maxDurability / 2 - durability) / (maxDurability / 2));
                }
            }
        }
    }
    private void LoseDurability()
    {
        durability -= Time.deltaTime;
        if (durability>=maxDurability/2)
        {
            spriteRenderer.color = Color.Lerp(startingColor, middleColor, middleLerp);
            if (middleLerp < 1)
            {
                Debug.Log("Durability: " + durability);
                Debug.Log("Max Durability: " + maxDurability);
                Debug.Log("Middle Lerp: " + middleLerp);
                //middleLerp += Time.deltaTime / (maxDurability/2);
                middleLerp = ((maxDurability - durability) / maxDurability) * 2;
            }
        }
        else if(durability<=0)
        {
            squareState = SquareState.breaking;
        }
        else
        {
            Debug.Log("Durability: " + durability);
            Debug.Log("Max Durability: " + maxDurability);
            Debug.Log("Middle Lerp: " + middleLerp);
            Debug.Log("End Lerp: " + endLerp);
            spriteRenderer.color = Color.Lerp(middleColor, endColor, endLerp);
            if (endLerp < 1)
            {
                //endLerp += Time.deltaTime / (maxDurability/2);
                endLerp = ((maxDurability/2 - durability) / (maxDurability/2));
            }
        }
    }

    private IEnumerator BreakSquare()
    {
        spriteRenderer.color = Color.gray;
        yield return new WaitForSeconds(breakPeriod);
        spriteRenderer.color = Color.black;
        this.gameObject.GetComponent<BoxCollider2D>().enabled = false;
    }
}
