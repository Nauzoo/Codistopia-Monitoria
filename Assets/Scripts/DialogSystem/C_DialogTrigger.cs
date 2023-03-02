using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class C_DialogTrigger : MonoBehaviour
{
    public void TriggerDialog(TextAsset dialog)
    {
        List<string> myStory = new List<string>(dialog.text.Split("\n")).ToList();
        DialogMannagerV2.GetInstance().TriggerDialog(myStory);

    }
}
