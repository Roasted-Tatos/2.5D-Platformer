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
    [SerializeField]
    private float _ladderSpeed = 2f;

    [SerializeField]
    private bool _canRoll = false;
    private Vector3 _direction, _velocity;
    private CharacterController controller;
    private Animator _anim;
    [SerializeField]
    private bool _jumping = false;
    private bool _idleJump = false;
    private bool _onLedge = false;
    private bool _onLadder = false;
    [SerializeField]
    private bool _isMoving = false;
    [SerializeField]
    private bool _climblingLadder = false;
    [SerializeField]
    private AudioSource _runningSound;
    [SerializeField]
    private AudioSource _climbingLadderSound;
    [SerializeField]
    private AudioSource _PlatformRunningSound;


    private UIManager uimanager;
    private GameObject _ledgeClimbComplete;
    private GameObject _finishedLocation;
    private GameObject _startLocation;
    private GameObject _ladderTop;
    private GameObject _ladderBottom;
    [SerializeField]
    private bool OnPlatform = false;


    // Start is called before the first frame update
    void Start()
    {
        uimanager = GameObject.Find("Canvas").GetComponent<UIManager>();
        controller = GetComponent<CharacterController>();
        if(controller == null)
        {
            return;
        }
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
            if(Input.GetKeyDown(KeyCode.Space))
            {
                _onLedge = false;
                _anim.SetBool("LedgeGrab",false);
                controller.enabled = true;
            }
        }
        if(_onLadder == true)
        {
            if (_gravity > 0)
            {
                _gravity = 0;
                _anim.SetBool("Ladder", _onLadder);
                _climblingLadder = true;
                transform.position = _ladderBottom.transform.position;
                //transform.rotation = Quaternion.LookRotation(Vector3.forward);
            }
            _velocity.y = Input.GetAxis("Vertical") * _ladderSpeed;
            if (Mathf.Abs(_velocity.y)>0.1f)
            {
                if (_velocity.y >0)
                {
                    _anim.speed = 1f;
                    _anim.SetFloat("Movement", 1f);
                }
                else
                {
                    _anim.speed = 1f;
                    _anim.SetFloat("Movement", -1f);
                }
            }
            else
            {
                if(_gravity ==0)
                {
                    _anim.speed = 0f;
                }
            }
            if(Input.GetKeyDown(KeyCode.W))
            {
                _climbingLadderSound.Play();
            }
            if(Input.GetKeyUp(KeyCode.W))
            {
                _climbingLadderSound.Pause();
            }
            if(Input.GetKeyDown(KeyCode.S))
            {
                _climbingLadderSound.Play();
            }
            if(Input.GetKeyUp(KeyCode.S))
            {
                _climbingLadderSound.Pause();
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
        //Forcing the Sound Effects for Running
        if (Input.GetKeyDown(KeyCode.D) && OnPlatform == false)
        {
            _runningSound.Play();
        }
        if (Input.GetKeyUp(KeyCode.D))
        {
            _runningSound.Pause();
        }
        if (Input.GetKeyDown(KeyCode.A) && OnPlatform == false)
        {
            _runningSound.Play();
        }
        if (Input.GetKeyUp(KeyCode.A))
        {
            _runningSound.Pause();
        }

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
    public void StopRolling()
    {
        _canRoll = false;
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

    public int totalPoints()
    {
        return _points;
    }

    public void ClimbLadder(GameObject _targetA,GameObject _targetB)
    {
        _ladderTop = _targetB;
        _ladderBottom = _targetA;
        _anim.SetFloat("Speed", 0);
        _onLadder = true;
        Debug.Log("Ladder Reached");
    }

    public void OffLadder()
    {
        transform.position = _ladderTop.transform.position;
        transform.rotation = Quaternion.LookRotation(Vector3.forward);
        _gravity = 20f;
        _climblingLadder = false;
        controller.enabled = true;
    }

    public void ExitLadder()
    {
        _onLadder = false;
        _anim.SetBool("Ladder", _onLadder);
        _anim.speed = 1;
        _velocity.y = 0;
        if(_climblingLadder == true)
        {
            controller.enabled = false;
        }
    }
    public void RunningPlatform()
    {
        OnPlatform = true;

        if(Input.GetKeyDown(KeyCode.A) && OnPlatform == true)
        {
            _PlatformRunningSound.Play();
        }
        if(Input.GetKeyUp(KeyCode.A))
        {
            _PlatformRunningSound.Pause();
        }
        if(Input.GetKeyDown(KeyCode.D) && OnPlatform == true)
        {
            _PlatformRunningSound.Play();
        }
        if(Input.GetKeyUp(KeyCode.D))
        {
            _PlatformRunningSound.Pause();
        }
    }
    public void StopRunningPlatform()
    {
        OnPlatform = false;
        _PlatformRunningSound.Pause();
    }
}
