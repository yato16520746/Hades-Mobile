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

public class Berserker_Delegate : MonoBehaviour
{
    [Space]
    [SerializeField] Joystick _joystick;
    [SerializeField] Animator _animator;
    [SerializeField] Rigidbody _rb;


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(_joystick.Horizontal + ", " + _joystick.Vertical);
    }
}
