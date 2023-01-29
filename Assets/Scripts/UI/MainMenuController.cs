using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
  public GameObject mainScreen;
  public GameObject optionsScreen;
  public GameObject quitScreen;

  public Button playGame;
  public Button optionsButton;
  public Button quitGameButton;
  public Button returnButton;
  public Button audioButton;
  public Button videoButton;
  public Button gameplayButton;
  public Button confirmQuitButton;
  public Button cancelQuitButton;

  public string firstScene;
  private bool otherMenu;


  void Start()
  {
    mainScreen.SetActive(true);
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
      QuitConfirmation();
    }

    Time.timeScale = 0;

    mainScreen.SetActive(!otherMenu);

    if (EventSystem.current.currentSelectedGameObject == null)
    {
      playGame.Select();
    }
  }

  public void Play()
  {
    SceneManager.LoadScene(firstScene);
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

    mainScreen.SetActive(true);
    playGame.Select();
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
