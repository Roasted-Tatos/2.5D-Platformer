using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableBehavior : MonoBehaviour
{
    [SerializeField]
    private AudioClip _collectSound;
    [SerializeField]
    private float volume = 1f;
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            Player player = GameObject.Find("Player").GetComponent<Player>();
            if(player !=null)
            {
                player.Addpoints();
                AudioSource.PlayClipAtPoint(_collectSound, Camera.main.transform.position, volume);
            }
            Destroy(this.gameObject);
        }
    }
}
