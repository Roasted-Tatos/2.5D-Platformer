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
    private Vector3 _direction, _velocity;
    private CharacterController controller;
    private Animator _anim;
    private bool _jumping = false;
    private bool _idleJump = false;
    private bool _onLedge;
    private GameObject _ledgeClimbComplete;

    // Start is called before the first frame update
    void Start()
    {
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
            _anim.SetTrigger("Climb");
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
}
