using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ladder : MonoBehaviour
{
    [SerializeField]
    private GameObject _pointA, _pointB;
    [SerializeField]
    private AudioSource _climbingSound;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            Player player = other.transform.GetComponent<Player>();
            if(player !=null)
            {
                player.ClimbLadder(_pointA, _pointB);
                _climbingSound.Play();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            Player player = other.transform.GetComponent<Player>();
            if (player !=null)
            {
                player.ExitLadder();
                _climbingSound.Pause();
            }
        }
    }
}
