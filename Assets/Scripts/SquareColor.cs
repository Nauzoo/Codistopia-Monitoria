using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquareColor : MonoBehaviour
{
    private SpriteRenderer mySprite;
    private void Start()
    {
        mySprite = GetComponent<SpriteRenderer>();
    }
    public void ChangeColor()
    {
        Start();
        mySprite.color = Color.red;

    }
}
