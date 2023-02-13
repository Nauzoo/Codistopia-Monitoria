using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.IO;
using Kino;

public class StartingAnimation : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI codeTxt;
    [SerializeField] private Scrollbar scrollbar;
    [SerializeField] private TextAsset code;
    [SerializeField] private Animator fader;
    [SerializeField] private GameObject Title;
    [SerializeField] private Animator Logo;
    public AnalogGlitch GlitchEffect;

    [SerializeField] private GameObject[] buttons;

    private List<string> codeLines;
    
    void Start()
    {
        foreach (GameObject button in buttons)
        {
            button.SetActive(false);
        }

        GlitchEffect.scanLineJitter = 0.1f;
        GlitchEffect.colorDrift = 0.05f;
        Title.SetActive(false);
        codeTxt.text = " ";
        StartCoroutine(StartCode());

        codeLines = new List<string>(code.text.Split('\n'));
        
    }

    private void Update()
    {
        scrollbar.value = 0;
    }
    private IEnumerator StartCode()
    {
        GlitchEffect.colorDrift = 0;
        yield return new WaitForSeconds(4);
        codeTxt.text = ">> ";
        yield return new WaitForSeconds(2);

        StartCoroutine(TypeCode("cd: Codistopia.exe", 0.1f));
        yield return new WaitForSeconds(3);

        StartCoroutine(DisplayLines(codeLines, 0.1f));

        yield return new WaitForSeconds(3);
        codeTxt.text = ">>";
        StartCoroutine(TypeCode("como ser um programador?", 0.05f));
        yield return new WaitForSeconds(2);
        StartCoroutine(DisplayLines(codeLines, 0.1f));
        fader.SetTrigger("Fade");
        yield return new WaitForSeconds(4);
        GlitchEffect.colorDrift = 0;
        Title.SetActive(true);
        yield return new WaitForSeconds(3);
        Title.SetActive(false);
        yield return new WaitForSeconds(1);

        GlitchEffect.scanLineJitter = 0.1f;
        foreach (GameObject button in buttons)
        {
            button.SetActive(true);
            yield return new WaitForSeconds(0.5f);
        } 

    }

    private IEnumerator TypeCode(string code, float typingDelay)
    {
        foreach (char l in code)
        {
            codeTxt.text += l;
            yield return new WaitForSeconds(typingDelay);
        }
    }

    private IEnumerator DisplayLines(List<string> code, float delay)
    {
        codeTxt.color = Color.green;
        foreach (string line in code)
        {
            if (line.Length > 1)
            {
                codeTxt.text += line + '\n';
                yield return new WaitForSeconds(delay);
            }
            else
            {
                codeTxt.text += '\n';
            }
            GlitchEffect.colorDrift += 0.001f;
        }
    }
}
