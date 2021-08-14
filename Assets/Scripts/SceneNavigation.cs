using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneNavigation : MonoBehaviour
{
    public void ToGameScene(){
        SceneManager.LoadSceneAsync("GameScene");
    }

    public void ToMenuScene(){
        SceneManager.LoadSceneAsync("SampleScene");
    }

    public void ExitApplication(){
        Application.Quit();
    }
}
