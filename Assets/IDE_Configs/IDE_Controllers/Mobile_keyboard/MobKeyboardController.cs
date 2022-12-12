using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobKeyboardController : MonoBehaviour
{
    private TouchScreenKeyboard mobKeyboard;

    public void Start() {
        mobKeyboard = TouchScreenKeyboard.Open("", TouchScreenKeyboardType.Default);
        mobKeyboard.active = true;
        Debug.Log(TouchScreenKeyboard.isSupported);
        Debug.Log(mobKeyboard.status);
    }

}
