using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

[DisallowMultipleComponent]
public class Oscillator : MonoBehaviour
{
    [SerializeField] Vector3 movementVector = new Vector3(10f, 10f, 10f);
    [SerializeField] float period = 2f;

    //todo remove from inspector 
    [Range(0,1)] [SerializeField] float movementFactor; //0 for not moved, 1 forfully moved

    Vector3 startingPos; //stored for absolute movement

    // Start is called before the first frame update
    void Start()
    {
        startingPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (period <= Mathf.Epsilon) { return; }

        float cycles = Time.time / period; //how many periods fit in current time = # of cycles
        const float tau = Mathf.PI * 2; //constant 2pi = 6.28 = 1 cycle
        float rawSinWave = Mathf.Sin(cycles * tau); //from -1 to 1 dependinf on current time/ place in cycle

        movementFactor = (rawSinWave / 2f) + 0.5f; //from 0 to 1

        Vector3 offset = movementVector * movementFactor; //where we want to move * how much 
        transform.position = startingPos + offset;
    }
}
