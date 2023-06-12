using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateItem : MonoBehaviour
{
    [SerializeField] private float rotateVelocity = 80.9f;

    void Update()
    {
        float rotationSpeed = rotateVelocity * Time.deltaTime;

        transform.Rotate(Vector3.forward * rotationSpeed);
    }
}
