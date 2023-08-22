using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

public class SceneManager : MonoBehaviour
{
    [SerializeField] AssetReference _sceneToLoad, _loadingScene;
    
    public void LoadScene()
    {
        SceneLoadingService.SceneToLoad = _sceneToLoad;
        SceneLoadingService.LoadScene(_loadingScene, UnityEngine.SceneManagement.LoadSceneMode.Single);
    }
}
