using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorLift : MonoBehaviour
{
    [SerializeField]
    private Transform _targetA, _targetB;
    [SerializeField]
    private int _speed = 5;
    [SerializeField]
    private bool _activated = false;
    [SerializeField]
    private Animator _elevatorGate;
    [SerializeField]
    private AudioSource _elevatorSound;

    public void CallElevator()
    {
        transform.position = _targetA.position;
        _activated = true;
        _elevatorSound.Play();
    }
    void FixedUpdate()
    {
        if (_activated == true)
        {
            transform.position = Vector3.MoveTowards(transform.position, _targetB.position, _speed * Time.deltaTime);
            StartCoroutine(OpenElevatorGate());
        }
        else if (_activated == false)
        {
            transform.position = Vector3.MoveTowards(transform.position, _targetA.position, _speed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            other.transform.parent = this.transform;
            _activated = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            other.transform.parent = null;
            _elevatorSound.Pause();
        }
    }

    IEnumerator OpenElevatorGate()
    {
        yield return new WaitForSeconds(9f);
        _elevatorGate.SetTrigger("Open");
    }

}
