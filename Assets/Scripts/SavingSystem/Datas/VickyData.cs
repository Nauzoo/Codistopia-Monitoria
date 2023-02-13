using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class VickyData
{
    public Vector2 position;
    public VickyData(VickFollower vickInstance)
    {
        position = new Vector2(vickInstance.transform.position[0], vickInstance.transform.position[1]);

    }   
}
