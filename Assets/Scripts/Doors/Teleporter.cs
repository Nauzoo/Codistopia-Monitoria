using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : MonoBehaviour
{
    [SerializeField] ChangeScene scneChanger;
    [SerializeField] GameObject joystick;

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            joystick.SetActive(false);
            FixedJoystick joyScript = joystick.GetComponent<FixedJoystick>();
            joyScript.handle.anchoredPosition = Vector2.zero;
            joyScript.input = Vector2.zero;
            
            scneChanger.ChangeToScene(LastScene.lastPassedScene);

        }
    }
}
