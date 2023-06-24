using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]

public class MoveItem : MonoBehaviour
{
    [SerializeField] private Vector3 movePosition;

    [SerializeField] private float moveSpeed;

    [SerializeField] [Range(0, 1)] private float moveProgress;

    private Vector3 _startPosition;

    void Start()
    {
        _startPosition = transform.position;
    }

    
    void Update()
    {
        moveProgress = Mathf.PingPong(Time.time, 1);

        Vector3 offset = movePosition * moveProgress * moveSpeed;

        transform.position = _startPosition + offset;
    }
}
