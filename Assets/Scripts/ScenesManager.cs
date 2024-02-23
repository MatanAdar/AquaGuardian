using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class ScenesManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GoToGameScene()
    {
        SceneManager.LoadScene("Game");
    }

    public void GoToSettingScene()
    {
        SceneManager.LoadScene("Settings");
    }

    public void GoToMenuScene()
    {
        SceneManager.LoadScene("Menu");
    }
}
