using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class gameOver : MonoBehaviour
{
    private bool modeWindowOpen = false;
    [SerializeField] private GameObject modeWindow;

    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private Slider loadingSlider;
    #region ToggleSelectWindow
    public void ToggleModeWindow()
    {
        modeWindowOpen = !modeWindowOpen;
    }
    void Update()
    {
        modeWindow.SetActive(modeWindowOpen);
    }
    #endregion
    #region LoadLevel
    public void Play(int sceneIndex)
    {
        StartCoroutine(LoadAsync(sceneIndex));
    }

    IEnumerator LoadAsync(int sceneIndex)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);

        loadingScreen.SetActive(true);

        while (!operation.isDone)
        {
            loadingSlider.value = operation.progress;
            yield return null;
        }
    }
    #endregion
    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
