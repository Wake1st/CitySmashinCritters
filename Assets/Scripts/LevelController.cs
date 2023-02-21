using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour
{
  public GameObject playerPrefab;
  public GameObject buildingPrefab;
  public GameObject scoreCardUI;
  public GameObject HUD;

  private int maxDestructables = 12;
  private List<GameObject> destructables = new List<GameObject>();

  public float countdown = 3.8f;
  private float maxLvlStartMsgTime = 1.6f;
  private string countdownMsg;

  private bool hasWon = false;
  private int score = 0;
  private float timeInLevel = 0f;
  [SerializeField]
  private int destructionWinCondition = 5;

  public delegate void TimeChange(float time);
  public static event TimeChange UpdateTime;

  private bool isPlaying = false;
  public delegate void PlayCheck(bool isPlaying);
  public static event PlayCheck UpdateIsPlaying;

  private int countdownSeconds;
  public delegate void CountdownChange(string text);
  public static event CountdownChange UpdateCountdown;

  [SerializeField]
  public GridBuilderProps gridBuilderProps;
  private GridBuilder gridBuilder;

  void Awake()
  {
    Instantiate(playerPrefab, transform.position, transform.rotation);
    //InitDestructables();

    GridBuilderProps gridBuilderProps = new GridBuilderProps();
    gridBuilderProps.origin = new Vector3(0, 0, 0);
    gridBuilderProps.lineCount = 12;
    gridBuilderProps.surfaceOffset = 0.1f;
    gridBuilderProps.minimumGridBoundary = 0;
    gridBuilderProps.maximumGridBoundary = 5;
    gridBuilderProps.entropy = 0.4f;
    gridBuilderProps.medianLineDist = 4;
    gridBuilderProps.lineDistancePrecisionBoundary = 0.8f;
    gridBuilderProps.minimumIntersectionDistance = 1f;
    gridBuilderProps.crossoverPossibility = 0.2f;

    gridBuilder = new GridBuilder(gridBuilderProps);
    gridBuilder.BuildGrid();

    countdownSeconds = Mathf.CeilToInt(countdown);
  }

  private void Start()
  {
    scoreCardUI.SetActive(false);

    gridBuilder.lines.ForEach((line) => print(
      gridBuilder.PrintLineMsg(line)
    ));
  }

  void InitDestructables()
  {
    Vector2 gridSize = new Vector2(4, 3);
    Vector2 gridOffset = new Vector2(-1.5f, -1.5f);

    for (int i = 0; i < maxDestructables; i++)
    {
      GameObject building = Instantiate(
          buildingPrefab,
          new Vector3(
              gridOffset.x + i % gridSize.x + Random.Range(-0.2f, 0.2f),
              0.5f,
              gridOffset.y + i / (int)gridSize.y + Random.Range(-0.3f, -0.3f)
          ),
          transform.rotation
      );

      //  must rename for a unique name
      building.name += i.ToString();

      destructables.Add(building);
    }
  }

  void Update()
  {
    gridBuilder.DrawLines();

    if (isPlaying)
    {
      if (!hasWon)
      {
        CheckWinStatus();

        timeInLevel += Time.deltaTime;
        UpdateTime(timeInLevel);

        //  Remove the countdown UI
        if (countdownMsg != null)
        {
          countdownMsg = timeInLevel > maxLvlStartMsgTime
              ? null
              : countdownMsg;

          UpdateCountdown(countdownMsg);
        }
      }
    }
    else
    {
      countdown -= Time.deltaTime;
      CheckCountdownSeconds();

      isPlaying = countdown < 0;
      if (isPlaying)
      {
        UpdateIsPlaying(isPlaying);
      }
    }
  }

  void CheckCountdownSeconds()
  {
    //  switch the seconds of the countdown
    if (Mathf.CeilToInt(countdown) != countdownSeconds)
    {
      countdownSeconds = Mathf.CeilToInt(countdown);

      countdownMsg = countdownSeconds > 0
          ? countdownSeconds.ToString()
          : "GO";

      UpdateCountdown(countdownMsg);
    }
  }

  void CheckWinStatus()
  {
    int destroyedEnemies = 0;

    foreach (GameObject destructable in destructables)
    {
      bool isAlive = GameObject
          .Find(destructable.name + "/Health")
          .GetComponent<DestructableHealthController>()
          .IsAlive();

      if (!isAlive)
      {
        destroyedEnemies++;

        if (destroyedEnemies >= destructionWinCondition)
        {
          hasWon = true;
          score = (int)(destroyedEnemies / timeInLevel * 10000);

          HUD.SetActive(false);

          scoreCardUI.SetActive(true);
          scoreCardUI
            .GetComponent<ScoreController>()
            .UpdateScore(score);
        }
      }
    }
  }
}
