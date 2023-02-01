using UnityEngine;
using UnityEngine.SceneManagement;

public class ScoreController : MonoBehaviour
{
  [SerializeField]
  private TMPro.TextMeshProUGUI scoreText = null;

  public string menuScene;

  private void Update()
  {
    if (Input.GetButtonDown("Submit"))
    {
      SceneManager.LoadScene(menuScene);
    }
  }

  public void UpdateScore(int score)
  {
    print(score);
    scoreText.text = "Score: " + score;
  }
}
