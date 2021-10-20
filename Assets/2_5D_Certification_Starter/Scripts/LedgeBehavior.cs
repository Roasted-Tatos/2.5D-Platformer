using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LedgeBehavior : MonoBehaviour
{
    [SerializeField]
    private GameObject _LedgePoss, _standingPos;
    [SerializeField]
    private AudioClip _thudSound;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Ledge_Grab_Checker"))
        {
            Player player = other.transform.parent.GetComponent<Player>();

            if(player != null)
            {
                player.LedgeGrab(_LedgePoss,_standingPos);
                AudioSource.PlayClipAtPoint(_thudSound, Camera.main.transform.position);
            }
        }
    }
  
}
