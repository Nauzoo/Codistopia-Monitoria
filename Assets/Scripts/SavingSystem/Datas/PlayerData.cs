using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public Vector2 position;

    public PlayerData(Player_movement playerProp)
    {
        position = new Vector2(playerProp.transform.position[0], playerProp.transform.position[1]);
    }
}
