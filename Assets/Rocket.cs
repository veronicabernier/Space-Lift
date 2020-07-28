using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour
{
    Rigidbody rigidBody;
    AudioSource audioSource;

    [SerializeField] float rcsThrust = 100f;
    [SerializeField] float mainThrust = 100f;
    [SerializeField] float fallingSpeed = 100f;

    enum State { Alive, Dying, Transcending };
    State state = State.Alive;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        // todo stop sound somewhere
        if (state == State.Alive) { 
            Thrust();
            Rotate();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(state != State.Alive) { return; }

        switch(collision.gameObject.tag)
        {
            case "Friendly":
                //nothing happens
                break;
            case "Finish":
                state = State.Transcending;
                Invoke("LoadNextLevel", 1f); //todo parametise time
                break;
            default:
                state = State.Dying;
                Invoke("LoadFirstLevel", 1f);
                print("Dead");
                break;
        }
    }

    private void LoadNextLevel()
    {
        SceneManager.LoadScene(1);
    }

    private void LoadFirstLevel()
    {
        SceneManager.LoadScene(0);
    }


    private void Thrust()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            rigidBody.AddRelativeForce(Vector3.up * mainThrust * Time.deltaTime);
            if (!audioSource.isPlaying || Input.GetKeyDown(KeyCode.Space))
            {
                audioSource.Play();
            }
        }
        else
        {
            audioSource.Stop();
            rigidBody.AddForce(Vector3.down * fallingSpeed * Time.deltaTime);
        }
    }

    private void Rotate()
    {
        rigidBody.freezeRotation = true; //take manual control of rotation
        

        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.forward * rcsThrust * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.D))
        {   
            transform.Rotate(-Vector3.forward * rcsThrust * Time.deltaTime);
        }
        rigidBody.freezeRotation = false; //resume physic's control on rotation
    }

}
