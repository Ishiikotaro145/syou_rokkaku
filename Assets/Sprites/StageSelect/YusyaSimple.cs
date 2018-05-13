using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YusyaSimple : MonoBehaviour
{
    private Rigidbody2D _rigidbody2D;

    // Use this for initialization
    void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _rigidbody2D.velocity = 4 * new Vector2(Random.value - .5f, Random.value - .5f).normalized;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.rotation = Quaternion.FromToRotation(Vector3.right, _rigidbody2D.velocity);
    }
}