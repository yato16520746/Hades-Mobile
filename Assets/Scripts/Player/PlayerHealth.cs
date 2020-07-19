using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    static string Dead_Trigger = "Dead";
    static string Hit_Trigger = "Hit";
    static string HitType_Int = "Hit Type";

    // singleton
    static PlayerHealth _instance;
    public static PlayerHealth Instance { get { return _instance; } }

    [SerializeField] Slider _HPSlider;
    [SerializeField] int _maxHP = 100;
    int _currentHP;

    [SerializeField] Player_Delegate _delegate;
    [SerializeField] Animator _animator;
    bool _allowHitAnimation;

    [SerializeField] float _timeDelayHit = 0.7f;
    bool _canGetDamage;

    public bool IsDead { get { return (_currentHP <= 0); } }

    [Header("UI")]
    [SerializeField] Animator _UIHitAnimator;
  
    private void Start()
    {
        _instance = this;

        _currentHP = _maxHP;

        _HPSlider.maxValue = _maxHP;
        _HPSlider.value = _currentHP;
        _canGetDamage = true;
    }

    IEnumerator CanGetDamageAfter(float time)
    {
        yield return new WaitForSeconds(time);

        _canGetDamage = true;
    }

    public void FullHP()
    {
        _currentHP = _maxHP;
        _HPSlider.value = _currentHP;
    }

    public bool CanGetDamage { get { return _canGetDamage; } }

    public void AddDamage(int amount, Vector3 damagePosition)
    {
        if (!_canGetDamage)
        {
            return;
        }

        if (_currentHP <= 0)
        {
            return;
        }

        if (amount >= 0)
        {
            amount = -amount;
        }

        _currentHP += amount;

        _canGetDamage = false;
        StartCoroutine(CanGetDamageAfter(_timeDelayHit));

        if (_currentHP > _maxHP)
        {
            _currentHP = _maxHP;
        }
        else if (_currentHP < 0)
        {
            _currentHP = 0;
        }

        _HPSlider.value = _currentHP;

        _UIHitAnimator.SetTrigger("Hit");

        // death animation
        if (_currentHP <= 0)
        {
            _animator.SetTrigger(Dead_Trigger);
        }
        else
        // hit animation
        {
            _animator.SetTrigger(Hit_Trigger);
            _animator.SetInteger(HitType_Int, Random.Range(0, 3));

            // direction
            Vector3 direction = transform.position - damagePosition;
            direction.y = 0f;
            _delegate.HitForceDirection = direction.normalized;
        }
    }
}
