using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SwitchScenes_C : MonoBehaviour {
    public string scenename;
    public static bool switching = false;
    // Use this for initialization
    private int i = 0;

    void OnTriggerEnter(Collider collision)
    {
        if (i == 0)
        {
            switching = true;
            StartCoroutine(Loading_scene());
            i++;
        }
    }

    IEnumerator Loading_scene()
    {

        yield return new WaitForSecondsRealtime(1.75f);
        if (scenename == "MainWorld") {
            SceneManager.LoadSceneAsync(scenename, LoadSceneMode.Single);
            SceneManager.LoadSceneAsync("Mengyu", LoadSceneMode.Additive);
            SceneManager.LoadSceneAsync("Cindy", LoadSceneMode.Additive);
            SceneManager.LoadSceneAsync("Jing", LoadSceneMode.Additive);
            SceneManager.LoadSceneAsync("Gustavo", LoadSceneMode.Additive);
            SceneManager.LoadSceneAsync("Yin", LoadSceneMode.Additive);
            SceneManager.LoadSceneAsync("Ehsan", LoadSceneMode.Additive);
            SceneManager.LoadSceneAsync("Tim", LoadSceneMode.Additive);
            SceneManager.LoadSceneAsync("Weidi", LoadSceneMode.Additive);
            SceneManager.LoadSceneAsync("Zhenyu", LoadSceneMode.Additive);
            SceneManager.LoadSceneAsync("Anshul", LoadSceneMode.Additive);
            SceneManager.LoadSceneAsync("Alexis", LoadSceneMode.Additive);
            SceneManager.LoadSceneAsync("Enrica", LoadSceneMode.Additive);
        }
        else
        {
            SceneManager.LoadSceneAsync(scenename, LoadSceneMode.Single);
        }
    }
    void OnDestroy()
    {
        switching = false;
    }
}
