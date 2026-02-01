using System;
using UnityEngine;

public class TestCameraFollow : MonoBehaviour
{
    public Transform target = null;
    
    private Vector3 _offset;

    private void Start()
    {
        _offset = transform.position - target.position;
    }

    void Update()
    {
        Vector3 newPos = target.position + _offset;
        transform.position = Vector3.MoveTowards(transform.position, newPos, 0.1f);
    }
}
