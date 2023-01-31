using UnityEngine;

public class FadeCamera : MonoBehaviour
{
  public AnimationCurve FadeCurve = new AnimationCurve(
    new Keyframe(0, 0),
    new Keyframe(1, 1)
    );

  private bool fading = false;
  public float fadeFactor;

  private Texture2D texture;
  private float alpha = 1;
  private float time = 0;
  private bool done = false;

  private void Update()
  {
    if (fading) return;
  }

  public void OnGUI()
  {
    if (!fading) return;
    if (done) return;
    if (texture == null) texture = new Texture2D(1, 1);

    texture.SetPixel(0, 0, new Color(0, 0, 0, alpha));
    texture.Apply();

    time += Time.deltaTime / fadeFactor;
    alpha = FadeCurve.Evaluate(time);
    GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), texture);

    if (alpha <= 0) done = true;
  }

  public void FadeOut()
  {
    fading = true;
  }
}
