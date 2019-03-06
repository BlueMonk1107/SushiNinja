using System;
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Loading : MonoBehaviour {
    public int SceneIndex { get; set; }
    // Use this for initialization
    void Start () {
        StartCoroutine(LoadAsync(SceneIndex));
        StartCoroutine(LoadAsyncUI());
    }
    IEnumerator LoadAsync(int index)
    {
        yield return SceneManager.LoadSceneAsync(index);
    }

    IEnumerator LoadAsyncUI()
    {
        yield return SceneManager.LoadSceneAsync("GameUI", LoadSceneMode.Additive);
    }

}
