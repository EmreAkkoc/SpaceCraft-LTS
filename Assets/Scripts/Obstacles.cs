using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacles : MonoBehaviour
{

    [SerializeField] Vector3 movingVector = new Vector3(10,10,10);
    [SerializeField] [Range(0f,1f)] float movingFactor;
    [SerializeField] float period = 4.0f;
    Vector3 startingPosition;
    Vector3 offset;
    float tau = Mathf.PI * 2;
    void Start()
    {
        startingPosition = transform.position;
        
    }

    
    void Update()
    {
        float cycles = Time.time / period;
        float raySinWave = Mathf.Sin(tau * cycles);
        offset = movingVector * raySinWave;
        transform.position = startingPosition + offset;
    }
}
