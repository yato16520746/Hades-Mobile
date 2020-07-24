using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BerserkerState
{
    Idle,
    Walk,
    Dodge,
    Attack1,
    Attack2,
    Attack3,
    Hit,
    Death
}

// lớp Delegate tuyệt đối ko dùng ngoài Berserker Group
public class Berserker_Delegate : MonoBehaviour
{
    [Space]
    [SerializeField] Transform _parentTransform;
    [SerializeField] Joystick _joystick;
    [SerializeField] Animator _animator;
    [SerializeField] Rigidbody _rb;
      
    //
    BerserkerState _state;
    BerserkerState _oldState;

    //
    [Header("Movement")]
    [SerializeField] float _walkSpeed = 5.5f;
    float _walkThreshold = 0f;
    [SerializeField] float _dodgeSpeed = 12f;
    [SerializeField] float _hitForce = 25f;
    [SerializeField] float _speedLerp = 15f;
    [SerializeField] float _rotateLerp = 20f;

    // dodge state
    [Header("Dodge state")]
    [SerializeField] float _dodgeSpeedDown = 2f;
    bool _isDodgeSpeedDown = false;
    Vector3 _dodgeDirection;

    // buttons
    bool _buttonAttackTrigger = false;
    bool _buttonDodgeTrigger = false;


    //
    void Start()
    {

    }

    //
    void Update()
    {
        // computer input
        float computerHorizontal = 0f;
        float computerVertical = 0f;

        if ((Input.GetKey(KeyCode.D) && Input.GetKey(KeyCode.A)) ||
            (!Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.A)))
        {
            computerHorizontal = 0;
        }
        else
        {
            if (Input.GetKey(KeyCode.D))
            {
                computerHorizontal = 1;
            }

            if (Input.GetKey(KeyCode.A))
            {
                computerHorizontal = -1;
            }
        }

        if ((Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.S)) ||
           (!Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S)))
        {
            computerVertical = 0;
        }
        else
        {
            if (Input.GetKey(KeyCode.W))
            {
                computerVertical = 1;
            }

            if (Input.GetKey(KeyCode.S))
            {
                computerVertical = -1;
            }
        }

        Vector3 computerDirection = new Vector3(computerHorizontal, 0f, computerVertical);

        //
        Vector3 joystickDirection = new Vector3(_joystick.Horizontal, 0f, _joystick.Vertical);
        joystickDirection += computerDirection;
        joystickDirection = joystickDirection.normalized;

        // walking bool
        if (joystickDirection.x != 0f || joystickDirection.z != 0f)
        {
            _animator.SetBool("Walking", true);
            _walkThreshold = 1f;
        }
        else
        {
            _walkThreshold = Mathf.Lerp(_walkThreshold, 0f, 10f * Time.deltaTime);
            if (_walkThreshold < 0.3f)
            {
                _animator.SetBool("Walking", false);
            }
        }

        // use buttons
        if (_buttonAttackTrigger || Input.GetKeyDown(KeyCode.J))
        {
            _animator.SetBool("Attack", true);
        }
        if (_buttonDodgeTrigger || Input.GetKeyDown(KeyCode.K))
        {
            _animator.SetTrigger("Dodge");
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            _animator.SetTrigger("Lobby");
        }


        // state machine behaviour
        switch (_state)
        {
            case BerserkerState.Idle:

                // idle update
                _rb.velocity = Vector3.Lerp(_rb.velocity, Vector3.zero, _speedLerp * Time.deltaTime);

                break;

            case BerserkerState.Walk:

                // walk update
                {
                    Vector3 velocity = joystickDirection * _walkSpeed;
                    _rb.velocity = Vector3.Lerp(_rb.velocity, velocity, _speedLerp * Time.deltaTime);

                    Vector3 look = new Vector3(_rb.velocity.x, 0f, _rb.velocity.z);
                    if (look.magnitude > 0.001f)
                    {
                        Quaternion rotation = Quaternion.LookRotation(look);
                        _parentTransform.rotation = Quaternion.Lerp(_parentTransform.rotation, rotation, _rotateLerp * Time.deltaTime);
                    }
                }

                break;

            case BerserkerState.Attack1:

                // attack1 enter
                if (_state != _oldState)
                {
                    _animator.SetBool("Attack", false);
                }

                // attack1 update
                _rb.velocity = Vector3.Lerp(_rb.velocity, Vector3.zero, _speedLerp * Time.deltaTime);

                break;

            case BerserkerState.Attack2:

                // attack3 enter
                if (_state != _oldState)
                {
                    _animator.SetBool("Attack", false);
                }

                // attack2 update
                _rb.velocity = Vector3.Lerp(_rb.velocity, Vector3.zero, _speedLerp * Time.deltaTime);

                break;

            case BerserkerState.Attack3:

                // attack3 enter
                if (_state != _oldState)
                {
                    _animator.SetBool("Attack", false);
                }

                // attack3 update
                _rb.velocity = Vector3.Lerp(_rb.velocity, Vector3.zero, _speedLerp * Time.deltaTime);

                break;

            case BerserkerState.Dodge:

                // dodge enter
                if (_state != _oldState)
                {
                    if (joystickDirection.x == 0f && joystickDirection.z == 0f)
                    {
                        _dodgeDirection = _parentTransform.forward;
                    }
                    else
                    {
                        _dodgeDirection = joystickDirection;
                    }

                    _animator.SetBool("Attack", false);
                    _isDodgeSpeedDown = false;
                }

                // dodge update
                {
                    if (!_isDodgeSpeedDown)
                    {
                        Vector3 velocity = _dodgeDirection * _dodgeSpeed;
                        _rb.velocity = Vector3.Lerp(_rb.velocity, velocity, _speedLerp * Time.deltaTime);
                    }
                    else
                    {
                        _rb.velocity = Vector3.Lerp(_rb.velocity, Vector3.zero, _dodgeSpeedDown * Time.deltaTime);
                    }

                    Quaternion rotation = Quaternion.LookRotation(_dodgeDirection);
                    _parentTransform.rotation = Quaternion.Lerp(_parentTransform.rotation, rotation, _rotateLerp * Time.deltaTime);
                }

                break;
        }

        _oldState = _state;

        // reset buttons
        _buttonAttackTrigger = false;
        _buttonDodgeTrigger = false;


    }

    //
    private void LateUpdate()
    {
        // state machine behaviour exit, _oldState là state chạy exit

        
        if (_oldState != _state)
        {
            
        }
    }

    [HideInInspector]
    //
    public void setState(BerserkerState state)
    {
        _state = state;
    }

    // dodge state
    void Event_Dodge_SpeedDown()
    {
        _isDodgeSpeedDown = true;
    }


    // buttons
    public void Button_Attack()
    {
        _buttonAttackTrigger = true;
    }

    public void Button_Dodge()
    {
        _buttonDodgeTrigger = true;
    }
}
