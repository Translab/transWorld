using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitch_auto : MonoBehaviour {

    public string scenename;
    public static bool switching = false;
    private int i = 0;
    public int wait_time = 60;
    // Use this for initialization
    void Start () {
       
    }
	
    void Update()
    {
        if (Time.timeSinceLevelLoad > wait_time && i == 0)
        {
            switching = true;
            StartCoroutine(Switching_auto());
            i = 1;
        }
    }

    IEnumerator Switching_auto()
    {
        Debug.Log("auto switching in 10s");
        yield return new WaitForSecondsRealtime(10);
        SceneManager.LoadSceneAsync(scenename, LoadSceneMode.Single);
    }
    void OnDestroy()
    {
        switching = false;
        i = 0;
    }

}
