using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    [SerializeField] private float rotateVelocity = 80.9f;

    [SerializeField] private float flySpeed = 100f;

    private Rigidbody _rigidBody;

    private AudioSource _audioSource;

    void Start()
    {
        _rigidBody = GetComponent<Rigidbody>();
        _audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        ProcessInput();
    }

    private void ProcessInput()
    {
        Launch();

        Rotation();
    }

    private void Launch()
    {
        float launchSpeed = flySpeed * Time.deltaTime;

        if (Input.GetKey(KeyCode.Space))
        {
            _rigidBody.AddRelativeForce(Vector3.up * launchSpeed);

            if (!_audioSource.isPlaying) _audioSource.Play();
        }

        else _audioSource.Pause();
    }

    private void Rotation()
    {
        float rotationSpeed = rotateVelocity * Time.deltaTime;

        _rigidBody.freezeRotation = true;

        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.forward * rotationSpeed);
        }

        else if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(-Vector3.forward * rotationSpeed);
        }

        _rigidBody.freezeRotation = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Battery":
                Debug.Log("Вам добавили энергии");
                Destroy(collision.gameObject);
                break;
            case "Finish":
                Debug.Log("Вы победили!");
                break;
            default:
                Debug.Log("Ракета взорвалась");
                break;
        }
    }
}
