using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    [SerializeField] private float rotateVelocity = 80.9f;

    [SerializeField] private float flySpeed = 100f;

    [SerializeField] private int _level;
    
    private Rigidbody _rigidBody;

    private AudioSource _audioSource;

    private bool _readyToCollision;

    void Start()
    {
        _rigidBody = GetComponent<Rigidbody>();
        _audioSource = GetComponent<AudioSource>();
        _readyToCollision = true;
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
        if (!_readyToCollision) return;

        _readyToCollision = false;
        switch (collision.gameObject.tag)
        {
            case "Battery":
                Debug.Log("Вам добавили энергии");
                Destroy(collision.gameObject);
                break;
            case "Finish":
                if (_level >= 2) _level = 0;
                else _level += 1;
                SceneManager.LoadScene(_level);
                Debug.Log(_level);
                break;
            case "StartPlatform":
                _readyToCollision = true;
                break;
            default:
                Debug.Log("Ракета взорвалась");
                _level = 0;
                SceneManager.LoadScene(_level);
                break;
        }
    }
}
