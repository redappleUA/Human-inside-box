using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneProgressBarController : MonoBehaviour
{
    [SerializeField] Slider _progressBar;

    private void Start()
    {
        StartCoroutine(StartLoading());
    }

    private IEnumerator StartLoading()
    {
        if(SceneLoadingService.SceneToLoad == null)
        {
            Debug.LogError("Scene to load is null");
            yield break;
        }

        var operation = SceneLoadingService.LoadScene(SceneLoadingService.SceneToLoad, UnityEngine.SceneManagement.LoadSceneMode.Single);

        do
        {
            _progressBar.value = operation.PercentComplete;
            yield return null;
        }
        while (!operation.IsDone);
    }
} 
