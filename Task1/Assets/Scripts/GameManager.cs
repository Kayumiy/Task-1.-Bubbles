using System;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{    
    // Input Data 
    public int objectCount;         // the initial number of bubbles in the field
    public float rectWidth;         // the size of the playing field
    public float rectHeight;        // the size of the playing field
    public float objectMinRadius;   // the range of values ​​for the bubble's initial radius
    public float objectMaxRadius;   // the range of values ​​for the bubble's initial radius
    
    public List<GameObject> bubbleGroup;     // collection of bubbles
    public GameObject rectangle;                    // Rectangle field
    public GameObject bubble;                       // bubble prefab
    float bubbleRadius;                 // random radius value for each new created bubble
    int number;

    // Start is called before the first frame update
    void Start()
    {
        number = 0;
        ResizeRect();
        bubbleGroup = new List<GameObject>();
        CreateInitialBubbles();
        InvokeRepeating("CreateBubblePerThreeSec", 3f, 3f);
    }
    

    /// <summary>
    /// Get random radiues value for bubble
    /// </summary>
    public void GetRandomRadiusValue()
    {
        bubbleRadius = UnityEngine.Random.Range(objectMinRadius, objectMaxRadius);
    }

    /// <summary>
    /// Create initial bubbles in the field after playing Scene and add it to the collection
    /// </summary>
    public void CreateInitialBubbles()
    {
        for (int i = 0; i < objectCount; i++)
        {
            CheckSpaceIsEmpty();
            GetRandomRadiusValue();
            //CreateSingleBubble();
        }
    }

    /// <summary>
    /// Create a bubble with instantioation prefab  
    /// </summary>
    public void CreateSingleBubble( float x, float y)
    {
        number++;
        GameObject aBubble = Instantiate(bubble, new Vector3(x , y , 1), Quaternion.identity) as GameObject;
        //GetRandomRadiusValue();
        aBubble.transform.localScale = new Vector3(bubbleRadius, bubbleRadius);
        aBubble.GetComponent<Bubble>().radius = bubbleRadius;
        aBubble.GetComponent<Bubble>().nameOfBubble = number.ToString();
        bubbleGroup.Add(aBubble);
    }

    /// <summary>
    /// Find empty space to create bubble in the field
    /// </summary>
    public void CheckSpaceIsEmpty()
    {        
        bool isEmpty = false;
        // While loop runs until finding the empty space in the field
        while (!isEmpty)
        {
            float x = UnityEngine.Random.Range(-rectWidth / 2 + objectMaxRadius, rectWidth / 2 - objectMaxRadius);
            float y = UnityEngine.Random.Range(-rectHeight / 2 + objectMaxRadius, rectHeight / 2 - objectMaxRadius);

            if (bubbleGroup.Count > 0)
            {
                int n = 0;
                // Check random position can collide with other bubble's position 
                foreach (GameObject bubbleObj in bubbleGroup)
                {                    
                    Bubble aBubble = bubbleObj.GetComponent<Bubble>();
                    //distance =  sqrt((x2 − x1)^2 + (y2 − y1)^2) − (r2 + r1)    this formula is the distance between bubbles
                    double distance = (float)Math.Sqrt((x - bubbleObj.transform.position.x) * (x - bubbleObj.transform.position.x) +
                        (y - bubbleObj.transform.position.y) * (y - bubbleObj.transform.position.y));
                    distance = distance - (bubbleRadius + bubbleObj.transform.localScale.x / 2);
                    if (distance > 0)
                    {
                        n++;                       
                    }
                    else if(distance <= 0)
                    {
                        break;
                    }
                }
                if (n == bubbleGroup.Count)
                {
                    isEmpty = true;
                    CreateSingleBubble(x, y); // x and y position ni ber
                }
            }
            else
            {
                isEmpty = true;
                CreateSingleBubble(x, y); // x and y position ni ber
            }
        }        
    }

    /// <summary>
    /// Create new bubble on collision point
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="radius"></param>
    /// <param name="firstObj"></param>
    /// <param name="secondObj"></param>
    public void CreateBubbleOnCollisionPoint(float x, float y, float radius, GameObject firstObj, GameObject secondObj)
    {
        DestroyBubble(firstObj.GetComponent<Bubble>().nameOfBubble, firstObj);
        DestroyBubble(secondObj.GetComponent<Bubble>().nameOfBubble, secondObj);
        number++;
        GameObject aBubble = Instantiate(bubble, new Vector3(x, y, 1), Quaternion.identity) as GameObject;
        aBubble.transform.localScale = new Vector3(radius * 2, radius * 2);
        aBubble.GetComponent<Bubble>().radius = radius;
        aBubble.GetComponent<Bubble>().nameOfBubble = number.ToString();
        bubbleGroup.Add(aBubble);
    }

    /// <summary>
    /// Resize field based on the use input
    /// </summary>
    public void ResizeRect()
    {
        rectangle.transform.localScale = new Vector3(rectWidth, rectHeight);
    }

    /// <summary>
    /// Create new bubble object every three second and add it to the collection 
    /// </summary>
    public void CreateBubblePerThreeSec()
    {
        CheckSpaceIsEmpty();
        GetRandomRadiusValue();
    }

    /// <summary>
    /// Destroy a bubble from Scene and List collection
    /// </summary>
    /// <param name="name"></param>
    /// <param name="bubbleObj"></param>
    public void DestroyBubble(string name, GameObject bubbleObj)
    {
        foreach (GameObject gmObj in bubbleGroup)
        {
            if (gmObj.GetComponent<Bubble>() != null)
            {
                Bubble bubble = gmObj.GetComponent<Bubble>();
                if (name.Equals(bubble.nameOfBubble))
                {
                    bubbleGroup.Remove(gmObj);
                    break;
                }
            }
        }
        Destroy(bubbleObj);
    }

}
