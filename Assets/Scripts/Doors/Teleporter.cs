using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            FixedJoystick.Instance.FlipActiveState(false);
            FixedJoystick joyScript = FixedJoystick.Instance.GetComponent<FixedJoystick>();
            joyScript.handle.anchoredPosition = Vector2.zero;
            joyScript.input = Vector2.zero;
            
            ChangeScene.Instance.ChangeToScene(LastScene.lastPassedScene);

        }
    }
}
