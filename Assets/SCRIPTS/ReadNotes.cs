using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReadNotes : MonoBehaviour
{
    public GameObject player;
    public GameObject noteUI;
    public GameObject interactText;
    public bool inReach;

    void Start()
    {
        noteUI.SetActive(false);
        interactText.SetActive(false);
        inReach = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player)
        {
            inReach = true;
            interactText.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject == player)
        {
            inReach = false;
            interactText.SetActive(false);
        }
    }

    void Update()
    {
        if(inReach && Input.GetKeyDown(KeyCode.E))
        {
            noteUI.SetActive(true);
            interactText.SetActive(false);
            player.GetComponent<Lucas>().StopClimbing();
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }

    public void ExitButton()
    {
        noteUI.SetActive(false);
        player.GetComponent<Lucas>().ClimbLadder(Vector3.up);
    }
}