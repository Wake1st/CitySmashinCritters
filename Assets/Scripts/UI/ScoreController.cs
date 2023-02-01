using UnityEngine;
using UnityEngine.SceneManagement;

public class ScoreController : MonoBehaviour
{
  [SerializeField]
  private TMPro.TextMeshProUGUI scoreCard = null;
  [SerializeField]
  private string scoreText = null;

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
    scoreCard.text = scoreText + score.ToString();
  }
}
