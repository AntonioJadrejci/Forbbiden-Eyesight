using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Footsteps : MonoBehaviour
{
   
 
    public AudioSource walk, sprint;

    void Update()
    {
        bool pressingKeys = Input.GetAxisRaw("Horizontal") == 1 || Input.GetAxisRaw("Vertical") == 1;
        bool running = Input.GetKey(KeyCode.LeftShift);

        walk.enabled = pressingKeys && !running;
        sprint.enabled = pressingKeys && running;
    }
} 


