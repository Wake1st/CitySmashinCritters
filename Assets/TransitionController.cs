using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionController : MonoBehaviour
{
  public string transitionScene;

  private AudioSource music;

  void Start()
  {
    music = GetComponent<AudioSource>();
  }

  void Update()
  {
    if (music.isPlaying) return;

    SceneManager.LoadScene(transitionScene);
  }
}
