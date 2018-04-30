using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuttingManager : MonoBehaviour
{
    Vector2 rangeVector2 = new Vector2(0.1f, 0.1f);

    private GameObject old;

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Collider2D collider2D = Physics2D.OverlapArea(mousePosition - rangeVector2, mousePosition + rangeVector2);
            if (collider2D != null && !collider2D.gameObject.Equals(old))
            {
                collider2D.gameObject.GetComponent<CuttableObject>().Cut();
                old = collider2D.gameObject;
            }

            if (collider2D == null) old = null;
        }
    }
}