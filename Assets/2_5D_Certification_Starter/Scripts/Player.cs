using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float _speed = 5f;
    [SerializeField]
    private float _jumpHeight = 20f;
    [SerializeField]
    private float _gravity = 1f;
    [SerializeField]
    private bool _rollin = false;
    [SerializeField]
    private int _points;

    private bool _canRoll = false;
    private Vector3 _direction, _velocity;
    private CharacterController controller;
    private Animator _anim;
    private bool _jumping = false;
    private bool _idleJump = false;
    private bool _onLedge = false;

    private UIManager uimanager;
    private GameObject _ledgeClimbComplete;
    private GameObject _finishedLocation;
    private GameObject _startLocation;


    // Start is called before the first frame update
    void Start()
    {
        uimanager = GameObject.Find("Canvas").GetComponent<UIManager>();
        controller = GetComponent<CharacterController>();
        _anim = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement(); 

        if(_onLedge == true)
        {
            if(Input.GetKeyDown(KeyCode.E))
            {
                _anim.SetTrigger("Climb");
                _onLedge = false;
            }
        }
    }

    public void CalculateMovement()
    {
        if (controller.isGrounded == true)
        {
            float horizonalInput = Input.GetAxisRaw("Horizontal");
            _anim.SetFloat("Speed", Mathf.Abs(horizonalInput));
            _direction = new Vector3(0, 0, horizonalInput);
            _velocity = _direction * _speed;

            //making the -1 turn +1 when facing Right
            if (horizonalInput != 0)
            {
                Vector3 facing = transform.localEulerAngles;
                facing.y = _direction.z > 0 ? 0 : 180;
                transform.localEulerAngles = facing;
            }

            if (_jumping == true)
            {
                _jumping = false;
                _anim.SetBool("Jumping", _jumping);
            }

            //Jumping and Idle Jumping
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (Mathf.Abs(_velocity.z) > 0.1)
                {
                    _velocity.y += _jumpHeight;
                    _jumping = true;
                    _anim.SetBool("Jumping", _jumping);
                }
                else
                {
                    StartCoroutine(StartIdleJump());
                }
            }
            //if Rolling
            if (Input.GetKeyDown(KeyCode.LeftShift) && !_jumping && _canRoll == true) 
            {
                controller.enabled = false;
                _rollin = true;
                _anim.SetTrigger("Rolling");
                _canRoll = false;
            }
        }

        _velocity.y -= _gravity * Time.deltaTime;
        controller.Move(_velocity * Time.deltaTime);
    }
    IEnumerator StartIdleJump()
    {
        _idleJump = true;
        _anim.SetBool("Jumping", _idleJump);
        yield return new WaitForSeconds(0.2f);
        _velocity.y += _jumpHeight;
        yield return new WaitForSeconds(1.5f);
        _idleJump = false;
        _anim.SetBool("Jumping", _idleJump);
    }

    public void StartRolling(GameObject _pointA,GameObject _pointB)
    {
        _canRoll = true;
        _startLocation = _pointA;
        _finishedLocation = _pointB;
    }
    public void LedgeGrab(GameObject _LedgeGrabPos,GameObject _finalPos)
    {
        controller.enabled = false;
        _anim.SetBool("LedgeGrab", true);
        _anim.SetBool("Jumping", false);
        _anim.SetFloat("Speed", 0f);
        transform.position = _LedgeGrabPos.transform.position;
        _onLedge = true;
        _ledgeClimbComplete = _finalPos;
    }

    public void ClimbUpComplete()
    {
        transform.position = _ledgeClimbComplete.transform.position;
        _anim.SetBool("LedgeGrab", false);
        controller.enabled = true;
    }

    public void RollingComplete()
    {
        if(transform.rotation.y ==0)
        {
            transform.position = _finishedLocation.transform.position;
            _rollin = false;
            controller.enabled = true;
        }
        else
        {
            transform.position = _startLocation.transform.position;
            _rollin = false;
            controller.enabled = true;
        }
    }

    public void Addpoints()
    {
        _points++;

        uimanager.UpdatePointsDisplay(_points);
    }
}
