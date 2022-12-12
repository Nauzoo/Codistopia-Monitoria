using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public float[] position;

    public PlayerData(Player_movement playerProp)
    {
        position = new float[2];

        position[0] = playerProp.transform.position.x;
        position[1] = playerProp.transform.position.y;
    }
}
