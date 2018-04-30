using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveScript : MonoBehaviour
{
    private Vector2 mousePositionOld;

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            mousePositionOld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
        else if (Input.GetMouseButton(0))
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (mousePositionOld.y < -0 && mousePosition.y < -0)
            {
                Quaternion rotation = Quaternion.FromToRotation(mousePositionOld, mousePosition);
                gameObject.transform.rotation = gameObject.transform.rotation * rotation;
            }

            mousePositionOld = mousePosition;
        }
    }
}