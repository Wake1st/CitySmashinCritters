using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    public GameObject playerPrefab;
    public GameObject buildingPrefab;

    private int maxDestructables = 12;
    private List<GameObject> destructables = new List<GameObject>();

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
        CheckWinStatus();
    }

    void CheckWinStatus() {
        foreach (GameObject destructable in destructables) {
            print(destructable.name);

            bool isAlive = GameObject
                .Find(destructable.name + "/Health")
                .GetComponent<DestructableHealthController>()
                .IsAlive();

            if (isAlive) {
                return;
            }
        }

        print("you win!");
    }
}
