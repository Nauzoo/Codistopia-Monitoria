using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveLastScene : MonoBehaviour
{
    public void SaveScene()
    {
        LastScene.lastPassedScene = SceneManager.GetActiveScene().buildIndex;
        LastScene.lastPlayerPos = Player_movement.GetInstance().transform.position;
        LastScene.lastVickPos = VickFollower.GetInstance().transform.position;

    }
}
