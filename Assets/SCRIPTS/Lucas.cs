using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lucas : MonoBehaviour
{
    private CharacterController characterController;
    private bool isClimbing = false;
    public float climbSpeed = 3f;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    public void ClimbLadder(Vector3 direction)
    {
        isClimbing = true;
        characterController.Move(direction * climbSpeed * Time.deltaTime);
    }

    public void StopClimbing()
    {
        isClimbing = false;
    }

    void Update()
    {
        if (!isClimbing)
        {
            // Normal player movement logic goes here
        }
    }
}
