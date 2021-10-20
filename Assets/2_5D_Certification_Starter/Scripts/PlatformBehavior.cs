using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformBehavior : MonoBehaviour
{
    [SerializeField]
    private Transform _targetA, _targetB;
    [SerializeField]
    private float _platformSpeed = 5f;
    [SerializeField]
    private Transform _target;

    private void Start()
    {
        _target = _targetA;
    }

    void FixedUpdate()
    {
        transform.position = Vector3.MoveTowards(transform.position, _target.position, _platformSpeed * Time.deltaTime);
        if(Vector3.Distance(transform.position,_targetA.position)<0.05f)
        {
            _target = _targetB;
        }
        else if(Vector3.Distance(transform.position,_targetB.position)<0.05f)
        {
            _target = _targetA;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            other.transform.parent = this.transform;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "Player")
        {
            other.transform.parent = null;
        }
    }
}
