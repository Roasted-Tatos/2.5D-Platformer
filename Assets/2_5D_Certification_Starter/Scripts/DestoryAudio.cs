using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestoryAudio : MonoBehaviour
{
    private void Awake()
    {
        GameObject A = GameObject.FindGameObjectWithTag("music");
        Destroy(A);

        GameObject[] objs = GameObject.FindGameObjectsWithTag("stageMusic");
        if (objs.Length > 1)
        {
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(this.gameObject);
    }
}