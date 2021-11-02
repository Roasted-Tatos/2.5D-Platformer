using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunningSound : MonoBehaviour
{
    Player player;

    private void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
    }
    private void OnTriggerStay(Collider other)
    {
        if(other.tag == "Player")
        {
            player.RunningPlatform();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "Player")
        {
            player.StopRunningPlatform();
        }
    }
}
