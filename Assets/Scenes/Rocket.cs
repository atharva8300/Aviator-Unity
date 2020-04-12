using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour
{
    [SerializeField] float rcsThrust = 100f;
    [SerializeField] float mainThrust = 30f; 
   
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
            Thrust();
            Rotate();

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
                state = State.Transcending;
                Invoke("LoadNextLevel",1f); //parameterise time 
                break;
            default:
                state = State.Dying;
                Invoke("LoadFirstLevel", 1f);
                break;
        }
    }

    private void LoadNextLevel()
    {
        SceneManager.LoadScene(1); // todo allow for than 2 levels 

    }
    private void LoadFirstLevel()
    {
        SceneManager.LoadScene(0); // todo allow for than 2 levels 

    }

    private void Thrust()
    {
        if (Input.GetKey(KeyCode.Space)) // detecting if space key is hit (every frame) KeyCode is an enum . 
        {
            //CAN THRUST WHILE ROTATING
            rigidBody.AddRelativeForce(Vector3.up * mainThrust);//position of the ship (x,y,z) is the parameter(Vector3)
            if (audioSource.isPlaying == false) // so that it doesn't layer
            {
                audioSource.Play();
            }

        }
        else
        {
            audioSource.Stop();
        }
    }


    private void Rotate()
    {
        rigidBody.freezeRotation = true; // take manual control of rotation 

        
        float rotationThisFrame = rcsThrust * Time.deltaTime;

        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.forward * rotationThisFrame);
        }
        else if (Input.GetKey(KeyCode.D))//else if used as rocket shouldn't rotate on bs
        {
            transform.Rotate(-Vector3.forward * rotationThisFrame);
        }

        rigidBody.freezeRotation = false; //resume physics control of rotation 
    }

   
}
