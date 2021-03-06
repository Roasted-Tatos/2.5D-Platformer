using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollingBehavior : MonoBehaviour
{
    [SerializeField]
    private GameObject _pointA, _pointB;
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            //trigger roll animation
            Player player = other.GetComponent<Player>();
            if (player !=null)
            {
                player.StartRolling(_pointA,_pointB);
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        Player player = other.GetComponent<Player>();
        if(player !=null)
        {
            player.StopRolling();
        }
    }
}
