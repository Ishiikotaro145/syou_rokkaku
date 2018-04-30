using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuttableObject : MonoBehaviour
{
    public enum CuttableType
    {
        once,
        twice,
        none
    }

    public CuttableType type;
    private GameObject part1;
    private GameObject part2;

    private bool isCut;
    private float speedV;
    private int count = 1;

    // Use this for initialization
    void Start()
    {
        part1 = transform.GetChild(0).gameObject;
        part2 = transform.GetChild(1).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (isCut)
        {
            Vector2 part1Position = part1.transform.position;
            Vector2 part2Position = part2.transform.position;
            part1.transform.position = new Vector2(part1Position.x - 2 * Time.deltaTime,
                part1Position.y + speedV * Time.deltaTime);
            part2.transform.position = new Vector2(part2Position.x + 2 * Time.deltaTime,
                part2Position.y + speedV * Time.deltaTime);
            speedV -= 10 * Time.deltaTime;
        }
    }

    public void Cut()
    {
        if (type == CuttableType.twice && count > 0)
        {
            count--;
            GetComponent<SpriteRenderer>().enabled = false;
        }
        else{isCut = true;}
        
    }
}