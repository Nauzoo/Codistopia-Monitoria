using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class VickyData
{
    public float[] positions;
    public VickyData(VickFollower vickInstance)
    {
        positions = new float[2];

        positions[0] = vickInstance.transform.position.x;
        positions[1] = vickInstance.transform.position.y;

    }   
}
