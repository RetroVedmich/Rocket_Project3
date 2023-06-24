using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    [SerializeField] private float rotateVelocity = 80.9f;

    [SerializeField] private float flySpeed = 100f;

    [SerializeField] private AudioClip flySound;

    [SerializeField] private AudioClip explodeSound;

    [SerializeField] private AudioClip finishSound;

    [SerializeField] private ParticleSystem rocketFly;

    [SerializeField] private ParticleSystem rocketExplotion;

    [SerializeField] private ParticleSystem rocketFinish;

    private int _level;
    
    private Rigidbody _rigidBody;

    private AudioSource _audioSource;

    private enum State { Playing, Dead, NextLevel }

    private State _state = State.Playing;

    void Start()
    {
        _rigidBody = GetComponent<Rigidbody>();
        _audioSource = GetComponent<AudioSource>();
        _state = State.Playing;
        _level = SceneManager.GetActiveScene().buildIndex;
    }

    void Update()
    {
        if(_state == State.Playing)
        {
            Launch();

            Rotation();
        }
    }

    private void Launch()
    {
        float launchSpeed = flySpeed * Time.deltaTime;

        if (Input.GetKey(KeyCode.Space))
        {
            _rigidBody.AddRelativeForce(Vector3.up * launchSpeed);

            if (!_audioSource.isPlaying)
            {
                _audioSource.PlayOneShot(flySound);

                rocketFly.Play();
            }
        }

        else
        {
            _audioSource.Pause();

            rocketFly.Stop();
        }
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

    private void LoadFirstLevel()
    {
        _level = 0;
        SceneManager.LoadScene(_level);
    }

    private void LoadNextLevel()
    {
        _level++;
        if (_level > 4) _level = 0;
        SceneManager.LoadScene(_level);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (_state == State.NextLevel || _state == State.Dead) return;

        switch (collision.gameObject.tag)
        {
            case "Battery":
                Debug.Log("Вам добавили энергии");
                Destroy(collision.gameObject);
                break;
            case "Finish":
                _state = State.NextLevel;
                _audioSource.PlayOneShot(finishSound);
                rocketFinish.Play();
                Invoke("LoadNextLevel", 1.5f);
                break;
            case "StartPlatform":
                break;
            default:
                Debug.Log("Ракета взорвалась");
                _audioSource.PlayOneShot(explodeSound);
                _state = State.Dead;
                rocketExplotion.Play();
                Invoke("LoadFirstLevel", 1.5f);
                break;
        }
    }
}
