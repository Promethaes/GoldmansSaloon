using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneNavigation : MonoBehaviour
{
    [SerializeField] float sceneLoadWaitTime = 1.0f;
    IEnumerator WaitToTransition(string name)
    {
        yield return new WaitForSeconds(sceneLoadWaitTime);
        SceneManager.LoadSceneAsync(name);
    }
    
    public void ToGameScene()
    {
        StartCoroutine(WaitToTransition("GameScene"));
    }

    public void ToMenuScene()
    {
        StartCoroutine(WaitToTransition("SampleScene"));
    }

    public void ExitApplication()
    {
        Application.Quit();
    }
}
