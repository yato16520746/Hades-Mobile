using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyButton : MonoBehaviour
{
    bool _buttonDown;
    public bool ButtonDown { get { return _buttonDown; } }

    private void LateUpdate()
    {
        _buttonDown = false;
    }

    public void BUTTON_Down()
    {
        _buttonDown = true;
    }
}
