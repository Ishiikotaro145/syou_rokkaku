using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuideLines : MonoBehaviour
{
    public int initialPoolSize;
    public static GuideLines instance;

    private List<GameObject> pool;
    public GameObject linePrefab;

    void Awake()
    {
        instance = this;
    }

    // Use this for initialization
    private void Start()
    {
        pool = new List<GameObject>();
        for (int i = 0; i < initialPoolSize; i++) pool.Add(Instantiate(linePrefab, transform));
    }


    public GameObject GetAvailableObject()
    {
        foreach (GameObject go in pool)
            if (!go.activeInHierarchy)
                return go;
        GameObject newGameObject = Instantiate(linePrefab, transform);
        pool.Add(newGameObject);
        return newGameObject;
    }

    public void RemoveAll()
    {
        if (pool == null) return;
        foreach (GameObject go in pool)
            go.SetActive(false);
    }
}