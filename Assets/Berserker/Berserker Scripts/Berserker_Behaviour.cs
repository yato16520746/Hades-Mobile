using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Berserker_Behaviour : StateMachineBehaviour
{
    Berserker_Delegate _delegate;
    [SerializeField] BerserkerState _state;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!_delegate)
        {
            _delegate = animator.GetComponent<Berserker_Delegate>();
        }

        _delegate.setState(_state);
    }
}
