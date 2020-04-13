using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour
{
    [SerializeField] float rcsThrust = 100f;
    [SerializeField] float mainThrust = 50f;
    [SerializeField] float levelLoadDelay = 2f;
    
    [SerializeField] AudioClip mainEngine;
    [SerializeField] AudioClip death;
    [SerializeField] AudioClip success;

    [SerializeField] ParticleSystem mainEngineParticles;
    [SerializeField] ParticleSystem deathParticles;
    [SerializeField] ParticleSystem successParticles;

    Rigidbody rigidBody;
    AudioSource audioSource;

    enum State  {Alive,Dying,Transcending };
    State state = State.Alive ;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>(); // generics and getcomponent(Rigidbody) from unity
        audioSource = GetComponent<AudioSource>();//get audio clip from unity 
    }

    // Update is called once per frame
    void Update()
    {
        if(state == State.Alive)
        {
            RespondToThrustInput();
            RespondToRotateInput();

        }
    }


    void OnCollisionEnter(Collision collision)
    {
        if (state != State.Alive)
        {
            return; // ignore collisions after death 
        }

        switch (collision.gameObject.tag)
        {

            case "Friendly":
                //pass
                break;
            case "Finish":
                StartSuccessSequence();
                break;
            default:
                StartDeathSequence();
                break;
        }
    }

    private void StartSuccessSequence()
    {
        state = State.Transcending;
        audioSource.Stop();
        audioSource.PlayOneShot(success);
        successParticles.Play();
        Invoke("LoadNextLevel", levelLoadDelay);  
    }


    private void StartDeathSequence()
    {
        state = State.Dying;
        audioSource.Stop(); // stop thrust sound
        audioSource.PlayOneShot(death);
        deathParticles.Play();
        Invoke("LoadFirstLevel", levelLoadDelay);
    }

    
    private void LoadNextLevel()
    {
       
        SceneManager.LoadScene(1); // todo allow for than 2 levels 

    }
    private void LoadFirstLevel()
    {
      
        SceneManager.LoadScene(0); // todo allow for than 2 levels 

    }

    private void RespondToThrustInput()
    {
        if (Input.GetKey(KeyCode.Space)) // detecting if space key is hit (every frame) KeyCode is an enum . 
        {
            ApplyThrust();
        }
        else
        {
            audioSource.Stop();
            mainEngineParticles.Stop();
        }
    }

    private void ApplyThrust()
    {
        //CAN THRUST WHILE ROTATING
        rigidBody.AddRelativeForce(Vector3.up * mainThrust);//position of the ship (x,y,z) is the parameter(Vector3)
        if (audioSource.isPlaying == false) // so that it doesn't layer
        {
            audioSource.PlayOneShot(mainEngine);
        }
        mainEngineParticles.Play();
    }

    private void RespondToRotateInput()
    {
        rigidBody.freezeRotation = true; // take manual control of rotation 

        
        float rotationThisFrame = rcsThrust * Time.deltaTime;

        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.forward * rotationThisFrame);
        }
        else if (Input.GetKey(KeyCode.D)) //else if used as rocket shouldn't rotate on bs
        {
            transform.Rotate(-Vector3.forward * rotationThisFrame);
        }

        rigidBody.freezeRotation = false; //resume physics control of rotation 
    } 
}
