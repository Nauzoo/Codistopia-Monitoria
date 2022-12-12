using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    [SerializeField] private Animator fadeAnim;
    [SerializeField] private GameObject fader;
    private void Start()
    {        
        StartCoroutine(StartScene());
        
    }
    public void ChangeToScene(int SceneId)
    {        
        StartCoroutine(MoveToScene(SceneId));
    }

    private IEnumerator StartScene()
    {       
        fader.SetActive(true);
        fadeAnim.SetTrigger("FadeOut");
        yield return new WaitForSeconds(1);
        fader.SetActive(false);

    }
    private IEnumerator MoveToScene(int SceneId)
    {        
        fader.SetActive(true);
        fadeAnim.SetTrigger("Jumper");
        fadeAnim.SetTrigger("FadeIn");
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(SceneId);

    }
}
