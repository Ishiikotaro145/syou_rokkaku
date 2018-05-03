using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveScript : MonoBehaviour
{
    public static StoveScript instance;
    public GameObject gameStartTrigger;
    private Vector2 mousePositionOld;

    private bool gameStart;
    // Use this for initialization
    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameStart) return;
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

    public void Reset()
    {
        gameObject.transform.rotation = Quaternion.identity;
        gameStart = false;
        gameStartTrigger.SetActive(true);
    }

    public void GameStart()
    {
        gameStart = true;
        gameStartTrigger.SetActive(false);
    }
}