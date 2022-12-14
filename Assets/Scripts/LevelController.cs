using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    public GameObject player;

    void Awake() {
        Instantiate(player, transform.position, transform.rotation);
    }
}
