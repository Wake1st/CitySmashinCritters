using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    public GameObject playerPrefab;
    public GameObject buildingPrefab;

    private int maxDestructables = 12;
    private List<GameObject> destructables = new List<GameObject>();

    public float countdown = 3f;
    
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

    void Awake() {
        Instantiate(playerPrefab, transform.position, transform.rotation);
        InitDestructables();
    }

    void InitDestructables() {
        Vector2 gridSize = new Vector2(4, 3);
        Vector2 gridOffset = new Vector2(-1.5f, -1.5f);

        for (int i=0; i < maxDestructables; i++) {
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

    void Update() {
        if (isPlaying) {
            if (!hasWon) {
                CheckWinStatus();

                timeInLevel += Time.deltaTime;
                UpdateTime(timeInLevel);
            }
        } else {
            countdown -= Time.deltaTime;

            isPlaying = countdown < 0;

            if (isPlaying) {
                UpdateIsPlaying(isPlaying);
            }
        }
    }

    void CheckWinStatus() {
        int destroyedEnemies = 0;

        foreach (GameObject destructable in destructables) {
            bool isAlive = GameObject
                .Find(destructable.name + "/Health")
                .GetComponent<DestructableHealthController>()
                .IsAlive();

            if (!isAlive) {
                destroyedEnemies++;

                if (destroyedEnemies >= destructionWinCondition) {
                    print("you win!");
                    hasWon = true;
                    score = (int)(destroyedEnemies / timeInLevel * 10000);
                    print(score);
                }
            }
        }

    }
}
