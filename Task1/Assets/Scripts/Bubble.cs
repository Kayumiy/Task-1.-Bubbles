using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Bubble : MonoBehaviour
{
    float increasingVal;            // increasing value for bubble. Bubble should be increased every second
    public float radius;
    public string nameOfBubble;
    public GameManager gameManager;   

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
        GetIncreasingValue();
        GetRandomColor();
        InvokeRepeating("IncreaseBubbleSize", 1f, 1f);
        CheckDistanceWithAllCircles();
        CheckBordersColission();
    }


    /// <summary>
    /// Give random color to buble
    /// </summary>
    public void GetRandomColor()
    {
        GetComponent<SpriteRenderer>().color = new Color(
        UnityEngine.Random.Range(0f, 1f),
        UnityEngine.Random.Range(0f, 1f),
        UnityEngine.Random.Range(0f, 1f));
    }

    /// <summary>
    /// Calculate increasing value based on the size of the circle
    /// </summary>
    public void GetIncreasingValue()
    {
        increasingVal = radius * 10 / 100;
    }    

    /// <summary>
    /// Increase size of the bubble every second
    /// </summary>
    public void IncreaseBubbleSize()
    {
        transform.localScale += new Vector3(increasingVal, increasingVal);
        CheckDistanceWithAllCircles();
        CheckBordersColission();

    }

    /// <summary>
    /// If the collision happens, Destroy both bubbles and create new one
    /// </summary>
    public void CheckDistanceWithAllCircles()
    {
        foreach (GameObject bubbleObj in gameManager.bubbleGroup.ToList())
        {            
            Bubble aBubble = bubbleObj.GetComponent<Bubble>();
            // Don't calculate the distance between bubble and itself
            if (nameOfBubble != aBubble.nameOfBubble)
            {                
                //distance =  sqrt((x2 − x1)^2 + (y2 − y1)^2) − (r2 + r1)    this formula is the distance between bubbles
                double distance = (float)Math.Sqrt((transform.position.x - bubbleObj.transform.position.x) * (transform.position.x - bubbleObj.transform.position.x) +
                    (transform.position.y - bubbleObj.transform.position.y) * (transform.position.y - bubbleObj.transform.position.y));
                distance = distance - (transform.localScale.x / 2 + bubbleObj.transform.localScale.x / 2);
                if (distance <= 0)
                {
                    float x = ((aBubble.transform.localScale.x / 2 * (transform.position.x - aBubble.transform.position.x)) /
                        (transform.localScale.x / 2 +  aBubble.transform.localScale.x / 2)) + aBubble.transform.position.x;
                    float y = ((aBubble.transform.localScale.y / 2 * (transform.position.y - aBubble.transform.position.y)) /
                        (transform.localScale.x / 2 + aBubble.transform.localScale.x / 2)) + aBubble.transform.position.y;
                    float rad = transform.localScale.x / 2  + aBubble.transform.localScale.x /2;                    
                    gameManager.CreateBubbleOnCollisionPoint(x, y, rad, this.gameObject, bubbleObj);
                }
            }
        }
    }
   
    /// <summary>
    /// If bubble collide with border, destroy bubble
    /// </summary>
    public void CheckBordersColission()
    {
        if (transform.position.x > 0 && transform.position.y < 0)
        {
            if (transform.position.x + transform.localScale.x / 2 >= gameManager.rectWidth/2)
            {
                gameManager.DestroyBubble(nameOfBubble, gameObject);               
            }
            else if (transform.position.y - transform.localScale.y / 2 <= -gameManager.rectHeight/2)
            {
                gameManager.DestroyBubble(nameOfBubble, gameObject);                
            }
        }
        else if (transform.position.x > 0 && transform.position.y > 0)
        {
            if (transform.position.y + transform.localScale.y / 2 >= gameManager.rectHeight / 2)
            {
                gameManager.DestroyBubble(nameOfBubble, gameObject);               
            }
            else if (transform.position.x + transform.localScale.x / 2 >= gameManager.rectWidth / 2)
            {
                gameManager.DestroyBubble(nameOfBubble, gameObject);                
            }
        }
        else if (transform.position.x < 0 && transform.position.y > 0)
        {
            if (transform.position.x - transform.localScale.x / 2 <= -gameManager.rectWidth / 2)
            {
                gameManager.DestroyBubble(nameOfBubble, gameObject);               
            }
            else if (transform.position.y + transform.localScale.y / 2 >= gameManager.rectHeight / 2)
            {
                gameManager.DestroyBubble(nameOfBubble, gameObject);                
            }
        }
        else
        {
            if (transform.position.x - transform.localScale.x / 2 <= -gameManager.rectWidth / 2)
            {
                gameManager.DestroyBubble(nameOfBubble, gameObject);                
            }
            else if (transform.position.y - transform.localScale.y / 2 <= -gameManager.rectHeight / 2)
            {
                gameManager.DestroyBubble(nameOfBubble, gameObject);                
            }
        }
    }

}
