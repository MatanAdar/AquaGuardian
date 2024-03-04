using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class openPanel : MonoBehaviour
{
    public GameObject Panel;
    public void OpenPanel()
    {
        if (Panel != null)
        {
            Panel.SetActive(true);
            
        }
    }

    public void Close_Panel()
    {
        if (Panel != null)
        {
            Panel.SetActive(false);

        }
    }
}
