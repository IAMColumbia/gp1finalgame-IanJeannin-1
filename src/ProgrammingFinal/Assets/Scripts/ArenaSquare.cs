using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaSquare : MonoBehaviour
{
    float durability;
    public enum SquareState { onSquare, offSquare, regenerating, breaking, broken };
    private SquareState squareState=SquareState.offSquare;
    private SpriteRenderer spriteRenderer;
    float middleLerp = 0; //Color.Lerp from starting color to middle color
    float endLerp = 0; //Color.Lerp from middle color to end color
    float breakLerp = 0;
    bool hasBroken = false;

    // Start is called before the first frame update
    void Start()
    {
        durability = ArenaSquareSettings.maxDurability;
        spriteRenderer = this.gameObject.GetComponent<SpriteRenderer>();
        ArenaSquareSettings.startingColor = spriteRenderer.color;
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

    /*private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.GetComponent<Player>()!=null&&squareState!=SquareState.breaking)
        {
            squareState=SquareState.onSquare;
            StopAllCoroutines();
        }
    }*/

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Player>() != null && squareState != SquareState.breaking && squareState != SquareState.broken)
        {
            squareState = SquareState.onSquare;
            StopAllCoroutines();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Player>() != null&&squareState!=SquareState.breaking&&squareState!=SquareState.broken)
        {
            squareState = SquareState.offSquare;
            StopAllCoroutines();
            StartCoroutine("BeginRegen");
        }
    }

    private IEnumerator BeginRegen()
    {
        yield return new WaitForSeconds(ArenaSquareSettings.regeneratePeriod);
        squareState = SquareState.regenerating;
    }

    private void Regenerate()
    {
        if (durability > 0 && durability<ArenaSquareSettings.maxDurability)
        {
            durability += Time.deltaTime * ArenaSquareSettings.regenerateRate;

            if (durability >= ArenaSquareSettings.maxDurability / 2)
            {
                spriteRenderer.color = Color.Lerp(ArenaSquareSettings.startingColor, ArenaSquareSettings.middleColor, middleLerp);
                if (middleLerp < 1)
                {
                    //middleLerp -= (Time.deltaTime * regenerateRate) / (maxDurability / 2)*regenerateRate;
                    middleLerp = ((ArenaSquareSettings.maxDurability - durability) / ArenaSquareSettings.maxDurability) * 2;
                }
            }
            else
            {
                spriteRenderer.color = Color.Lerp(ArenaSquareSettings.middleColor, ArenaSquareSettings.endColor, endLerp);
                if (endLerp < 1)
                {
                    //endLerp -= (Time.deltaTime * regenerateRate) / (maxDurability / 2)*regenerateRate;
                    endLerp = ((ArenaSquareSettings.maxDurability / 2 - durability) / (ArenaSquareSettings.maxDurability / 2));
                }
            }
        }
    }
    private void LoseDurability()
    {
        durability -= Time.deltaTime;
        if (durability>=ArenaSquareSettings.maxDurability/2)
        {
            spriteRenderer.color = Color.Lerp(ArenaSquareSettings.startingColor, ArenaSquareSettings.middleColor, middleLerp);
            if (middleLerp < 1)
            {
                Debug.Log("Durability: " + durability);
                Debug.Log("Max Durability: " + ArenaSquareSettings.maxDurability);
                Debug.Log("Middle Lerp: " + middleLerp);
                //middleLerp += Time.deltaTime / (maxDurability/2);
                middleLerp = ((ArenaSquareSettings.maxDurability - durability) / ArenaSquareSettings.maxDurability) * 2;
            }
        }
        else if(durability<=0)
        {
            squareState = SquareState.breaking;
        }
        else
        {
            Debug.Log("Durability: " + durability);
            Debug.Log("Max Durability: " + ArenaSquareSettings.maxDurability);
            Debug.Log("Middle Lerp: " + middleLerp);
            Debug.Log("End Lerp: " + endLerp);
            spriteRenderer.color = Color.Lerp(ArenaSquareSettings.middleColor, ArenaSquareSettings.endColor, endLerp);
            if (endLerp < 1)
            {
                //endLerp += Time.deltaTime / (maxDurability/2);
                endLerp = ((ArenaSquareSettings.maxDurability/2 - durability) / (ArenaSquareSettings.maxDurability/2));
            }
        }
    }

    private IEnumerator BreakSquare()
    {
        spriteRenderer.color = Color.gray;
        yield return new WaitForSeconds(ArenaSquareSettings.breakPeriod);
        spriteRenderer.color = Color.black;
        squareState = SquareState.broken;
    }


    public void ResetDurability()
    {
        durability = ArenaSquareSettings.maxDurability;
        squareState = SquareState.offSquare;
        spriteRenderer.color = Color.green;
        hasBroken = false;
    }

    public SquareState GetSquareState()
    {
        return squareState;
    }
}
