using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class Oscillator : MonoBehaviour
{
    [SerializeField] Vector3 movementVector = new Vector3(10f , 10f ,10f);
    [SerializeField] float period = 2f;

    //todo remove from inspector later
    [Range(0,1)][SerializeField] float movementFactor;

    Vector3 startingPos;

    // Start is called before the first frame update
    void Start()
    {
        startingPos = transform.position; 
    }

    // Update is called once per frame
    void Update()
    {
        //set movement factor
        if(period <= Mathf.Epsilon) { return; }

        float cycles = Time.time / period; // grows continually from 0 
        const float tau = Mathf.PI * 2f; // about 6.28 
        float rawSinWave = Mathf.Sin(cycles * tau); // from -1 to 1

        movementFactor = rawSinWave / 2f + 0.5f; // from 0 to 1 
        Vector3 offset = movementVector * movementFactor;
        transform.position = startingPos + offset;
        
    }
}
