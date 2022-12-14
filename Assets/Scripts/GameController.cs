using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    //  Singleton setup
    public static GameController gameController { get; private set; }

    void Awake() {
        if (gameController != null && gameController != this) {
            Destroy(this);
        } else {
            gameController = this;
        }
    }

    //  Operation
    [SerializeField]
    public List<GameObject> destructables;

    void Update() {
        CheckWinStatus();
    }

    void CheckWinStatus() {
        foreach (GameObject destructable in destructables) {
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
