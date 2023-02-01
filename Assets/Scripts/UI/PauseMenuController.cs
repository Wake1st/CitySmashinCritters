using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class PauseMenuController : MonoBehaviour
{
  public GameObject pauseScreen;
  public GameObject optionsScreen;
  public GameObject quitLevelScreen;
  public GameObject quitGameScreen;

  public string menuScene;

  public Button resumeGameButton;
  public Button optionsButton;
  public Button quitGameButton;
  public Button returnButton;
  public Button audioButton;
  public Button videoButton;
  public Button gameplayButton;
  public Button confirmQuitLevelButton;
  public Button cancelQuitLevelButton;
  public Button confirmQuitGameButton;
  public Button cancelQuitGameButton;

  private bool paused;
  private bool otherMenu;

  void Start()
  {
    pauseScreen.SetActive(false);
    optionsScreen.SetActive(false);
    quitGameScreen.SetActive(false);
  }

  void Update()
  {
    IsPaused();
  }

  private void IsPaused()
  {
    if (Input.GetButtonDown("Cancel"))
    {
      paused = !paused;
    }

    if (paused)
    {
      Time.timeScale = 0;

      pauseScreen.SetActive(!otherMenu);

      if (EventSystem.current.currentSelectedGameObject == null)
      {
        resumeGameButton.Select();
      }
    }
    else
    {
      otherMenu = false;

      pauseScreen.SetActive(false);
      optionsScreen.SetActive(false);
      quitLevelScreen.SetActive(false);
      quitGameScreen.SetActive(false);

      EventSystem.current.SetSelectedGameObject(null);

      Time.timeScale = 1;
    }
  }

  public void Options()
  {
    otherMenu = true;
    optionsScreen.SetActive(true);
    returnButton.Select();
  }

  public void Back()
  {
    otherMenu = false;
    optionsScreen.SetActive(false);
    quitLevelScreen.SetActive(false);
    quitGameScreen.SetActive(false);

    pauseScreen.SetActive(true);
    resumeGameButton.Select();
  }

  public void QuitLevelConfirmation()
  {
    otherMenu = true;
    quitLevelScreen.SetActive(true);
    cancelQuitLevelButton.Select();
  }

  public void QuitGameConfirmation()
  {
    otherMenu = true;
    quitGameScreen.SetActive(true);
    cancelQuitGameButton.Select();
  }

  public void UnPause()
  {
    paused = false;
  }

  public void QuitLevel()
  {
    SceneManager.LoadScene(menuScene);
  }

  public void QuitGame()
  {
#if UNITY_EDITOR
    if (Application.isEditor)
    {
      UnityEditor.EditorApplication.isPlaying = false;
    }
#endif
    else
    {
      Application.Quit();
    }
  }
}
