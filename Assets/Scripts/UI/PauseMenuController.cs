using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PauseMenuController : MonoBehaviour
{
  public GameObject pauseScreen;
  public GameObject optionsScreen;
  public GameObject quitScreen;

  public Button resumeGameButton;
  public Button optionsButton;
  public Button quitGameButton;
  public Button returnButton;
  public Button audioButton;
  public Button videoButton;
  public Button gameplayButton;
  public Button confirmQuitButton;
  public Button cancelQuitButton;

  private bool paused;
  private bool otherMenu;

  void Start()
  {
    pauseScreen.SetActive(false);
    optionsScreen.SetActive(false);
    quitScreen.SetActive(false);
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
      quitScreen.SetActive(false);

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

  public void QuitConfirmation()
  {
    otherMenu = true;
    quitScreen.SetActive(true);
    cancelQuitButton.Select();
  }

  public void Back()
  {
    otherMenu = false;
    optionsScreen.SetActive(false);
    quitScreen.SetActive(false);

    pauseScreen.SetActive(true);
    resumeGameButton.Select();
  }

  public void UnPause()
  {
    paused = false;
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
