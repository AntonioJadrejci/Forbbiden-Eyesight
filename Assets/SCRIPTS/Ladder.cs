using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ladder : MonoBehaviour
{
    private bool inside = false;
    private Lucas player;
    public AudioSource sound;

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            player = col.GetComponent<Lucas>();
            if (player != null)
            {
                Debug.Log("Player touching ladder");
                inside = true;
            }
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            if (player != null)
            {
                Debug.Log("Player leaving ladder");
                player.StopClimbing();
                inside = false;
            }
        }
    }

    void Update()
    {
        if (inside && player != null)
        {
            if (Input.GetKey("w"))
            {
                player.ClimbLadder(Vector3.up);
                if (!sound.isPlaying)
                {
                    sound.enabled = true;
                    sound.loop = true;
                    sound.Play();
                }
            }
            else if (Input.GetKey("s"))
            {
                player.ClimbLadder(Vector3.down);
                if (!sound.isPlaying)
                {
                    sound.enabled = true;
                    sound.loop = true;
                    sound.Play();
                }
            }
            else
            {
                player.StopClimbing();
                if (sound.isPlaying)
                {
                    sound.enabled = false;
                    sound.loop = false;
                    sound.Stop();
                }
            }
        }
    }
}
