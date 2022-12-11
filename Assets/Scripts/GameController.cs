using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController gameController { get; private set; }

    void Awake() {
        if (gameController != null && gameController != this) {
            Destroy(this);
        } else {
            gameController = this;
        }
    }
}
